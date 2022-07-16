using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using HannerLabApp.Configuration;
using HannerLabApp.Services;
using HannerLabApp.Services.Cloud;
using TinyMvvm;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HannerLabApp.ViewModels.SettingsViewModels
{
    public class SettingsPageViewModel : ViewModelBase
    {
        private readonly ICloudAuthenticator _cloudAuthenticator;
        private readonly ICloudFileHandler _cloudFileHandler;

        private const string DefaultLoginMessage = "Click to login";
        private const string LoginLoadingMessage = "Fetching credentials...";
        private const string LoginFailedMessage = "ERROR: credentials invalid or expired. Please click here to log-out and try again.";
        private const string DefaultSyncMessage =
            "Backup all application data, including all media, and export history and export file packages.";

        private readonly string _aboutText = $"{Constants.AppDescription}\n{Constants.AppCopyright}\nVersion: {Constants.AppVersionString}";
        private readonly string _appUrl = $"https://github.com/{Constants.AppGithubBaseRepoUrl}";
        private readonly string _appLicenseUrl = $"https://raw.githubusercontent.com/{Constants.AppGithubBaseRepoUrl}/main/LICENSE";

        private readonly IPageService _pageService;

        private string _accountEmail;
        private bool _isAuthenticated;
        private bool _isLoading;
        private string _syncMessage;

        public string SyncMessage
        {
            get => _syncMessage;
            set => Set(ref _syncMessage, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public bool IsSavePhotosToGalleryEnabled
        {
            get => App.AppSettings.IsSavePhotosToGalleryEnabled;
            set => App.AppSettings.IsSavePhotosToGalleryEnabled = value;
        }
        public bool IsAdvanceModeDefaultEnabled
        {
            get => App.AppSettings.IsAdvanceModeDefaultEnabled;
            set => App.AppSettings.IsAdvanceModeDefaultEnabled = value;
        }

        public bool IsAuthenticated
        {
            get => _isAuthenticated;
            private set => Set(ref _isAuthenticated, value);
        }

        public string AccountEmail
        {
            get => _accountEmail;
            set => Set(ref _accountEmail, value);
        }

        public ICommand SyncCommand { get; private set; }
        public ICommand AuthenticateCommand { get; private set; }

        public ICommand HelpFeedbackCommand { get; private set; }
        public ICommand AboutCommand { get; private set; }
        public ICommand LicenseCommand { get; private set; }

        public SettingsPageViewModel(IPageService pageService, ICloudAuthenticator cloudAuthenticator, ICloudFileHandler cloudFileHandler)
        {
            _pageService = pageService;
            _cloudAuthenticator = cloudAuthenticator;
            _cloudFileHandler = cloudFileHandler;

            IsLoading = false;

            UpdateSyncMessage();

            // Auth/sync
            AccountEmail = DefaultLoginMessage;

            AuthenticateCommand = new TinyCommand(async () => await AuthenticateAsync());
            SyncCommand = new TinyCommand(async () => await SyncAsync());


            // Other
            AboutCommand = new TinyCommand(async () => 
                await _pageService.ShowAlertAsync($"{Constants.AppName}", _aboutText, "Ok"));

            HelpFeedbackCommand = new TinyCommand(async () =>
                await Browser.OpenAsync(_appUrl));

            LicenseCommand = new TinyCommand(async () =>
                await Browser.OpenAsync(_appLicenseUrl));
        }

        public override async Task Initialize()
        {
            _cloudFileHandler.AccessToken = App.AppSettings.CurrentCloudApiToken;
            CheckAuthentication();

            await LoadCloudAccountName();
        }

        private void UpdateSyncMessage()
        {
            SyncMessage = DefaultSyncMessage;

            var lastSyncTime = App.AppSettings.LastSyncTime;
            if (lastSyncTime != null)
            {
                SyncMessage = SyncMessage + "\n\nLast synced: " + lastSyncTime?.ToString();
            }
        }

        private async Task LoadCloudAccountName()
        {
            AccountEmail = LoginLoadingMessage;

            if (IsAuthenticated)
            {
                var email = await _cloudFileHandler.GetAccountAsync();

                if (string.IsNullOrEmpty(email))
                    AccountEmail = LoginFailedMessage;
                else
                    AccountEmail = $"Logged in as: {email}";
            }
            else
            {
                AccountEmail = DefaultLoginMessage;
            }
        }

        private bool CheckAuthentication()
        {
            IsAuthenticated = App.AppSettings.CurrentCloudApiToken != string.Empty;

            return IsAuthenticated;
        }

        private void Logout()
        {
            UpdateToken(string.Empty);
            IsAuthenticated = false;
            AccountEmail = DefaultLoginMessage;
        }

        private void UpdateToken(string token)
        {
            _cloudFileHandler.AccessToken = token;
            App.AppSettings.CurrentCloudApiToken = token;
        }

        private async Task AuthenticateAsync()
        {
            if (IsAuthenticated)
            {
                if (!await _pageService.ShowYesNoAlertAsync("Log out?", "Are you sure?", "Yes", "No")) return;
                Logout();
            }
            else
            {
                // Show login screen....
                var account = await _cloudAuthenticator.Authenticate();
                UpdateToken(account);

                // Find account name
                CheckAuthentication();
                await LoadCloudAccountName();
            }
        }

        /// <summary>
        /// Copies data to cloud folder
        /// </summary>
        /// <returns></returns>
        private async Task SyncAsync()
        {
            IsLoading = true;
            await Task.Delay(1);

            // Get Device ID
            string deviceId = DependencyService.Get<IDeviceIdentifier>().GetIdentifier();

            // sync logs
            SyncMessage = "Syncing application logs...";
            await UploadFolder(Constants.LogDirectory, Path.Combine(deviceId, "logs/"));

            // sync appdata
            SyncMessage = "Syncing database...";
            await UploadFolder(Constants.AppDataDirectory, Path.Combine(deviceId, "appdata/"));

            // Sync media
            SyncMessage = "Syncing media...";
            await UploadFolder(Constants.MediaDirectory, Path.Combine(deviceId, "media/"));

            // Sync exports
            SyncMessage = "Syncing previous exports...";
            await UploadFolder(Constants.ExportDirectory, "exports/");

            App.AppSettings.LastSyncTime = DateTime.Now;

            UpdateSyncMessage();
            IsLoading = false;
        }

        /// <summary>
        /// Uploads a folder and all its contents recursively using the cloud provider 
        /// </summary>
        /// <param name="localFolderPath"></param>
        /// <param name="remoteFolderPath"></param>
        /// <returns></returns>
        private async Task UploadFolder(string localFolderPath, string remoteFolderPath)
        {
            // Get file paths of all files in dir
            string[] allfiles = Directory.GetFiles(localFolderPath, "*", SearchOption.AllDirectories);

            foreach (string file in allfiles)
            {
                var bytes = File.ReadAllBytes(file);
                if (bytes != null)
                    await _cloudFileHandler.WriteFileAsync(bytes, Path.GetFileName(file), remoteFolderPath);
            }
        }
    }
}
