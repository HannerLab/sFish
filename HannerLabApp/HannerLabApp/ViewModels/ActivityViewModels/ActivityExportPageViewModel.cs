using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Exporters;
using HannerLabApp.Services.Managers;
using HannerLabApp.Services.Media;
using HannerLabApp.Utils;
using HannerLabApp.Views;
using TinyMvvm;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace HannerLabApp.ViewModels.ActivityViewModels   
{
    /// <summary>
    /// Main page where the user creates an activity and exports it along with all data.
    /// The flow:
    ///     - Invoke flow routine (ExportDataAsync()) from ExportToFileCommand/ Button
    ///     - Spawn an Activity Details page (SpawnActivityDetailsViewModelAsync()) on completion the model is sent via message bus
    ///     - Model is received back at this page via message bus (ReceiveCompletedActivityDetailsAsync())
    ///     - Export is generated based on the activity model (GenerateExportAsync()). On completion the file is shared to user via native share menu.
    ///
    /// </summary>
    public class ActivityExportPageViewModel : ViewModelBase
    {
        private readonly IActivityExportCreator _activityExportCreator;
        private readonly IPageService _pageService;
        private readonly IFileShare _fileShare;

        private bool _isLoading = false;
        private bool _isIncludeAttachmentsSelected;
        private bool _isUseMdmaprFormatSelected;
        private bool _isSoftExportSelected;

        public bool IsIncludeAttachmentsSelected
        {
            get => _isIncludeAttachmentsSelected;
            set => Set(ref _isIncludeAttachmentsSelected, value);
        }

        public bool IsUseMdmaprFormatSelected
        {
            get => _isUseMdmaprFormatSelected;
            set => Set(ref _isUseMdmaprFormatSelected, value);
        }

        public bool IsSoftExportSelected
        {
            get => _isSoftExportSelected;
            set => Set(ref _isSoftExportSelected, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            private set => Set(ref _isLoading, value);
        }

        public ICommand ExportToFileCommand { get; private set; }
        public ICommand ActivityHistoryCommand { get; private set; }
        public ActivityExportPageViewModel(IPageService pageService, IFileShare fileShare, IActivityExportCreator activityExportCreator)
        {
            _pageService = pageService;
            _activityExportCreator = activityExportCreator;
            _fileShare = fileShare;

            ExportToFileCommand = new TinyCommand(async () => await ExportDataAsync());
            ActivityHistoryCommand = new TinyCommand(async () => await _pageService.NavigateToAsync("ActivityHistoryListView", null));
        }

        private async Task ExportDataAsync()
        {
            // Prompt user to confirm before
            if (!IsSoftExportSelected)
            {
                if (!await _pageService.ShowYesNoAlertAsync("Export Activity",
                        "Once exported, all exported samples, instrumental readings, observations, and photos will no longer be shown in the app. \n\nUse soft export to prevent these items from being tagged as exported or enable \"Show exported samples\" within settings.",
                        "Continue", "Cancel")) return;
            }
            
            // Alert user that MDMAPR does not support custom units.
            if (IsUseMdmaprFormatSelected)
            {
                await _pageService.ShowAlertAsync("Warning!",
                    "You have selected to export using the MDMAPR2.0 format. MDMAPR does not support custom units. Please ensure you have recorded using the correct units, or convert the units after export. For more information visit the MDMAPR homepage.",
                    "I understand");
            }

            await SpawnActivityDetailsViewModelAsync();
        }


        /// <summary>
        /// Opens up an ActivityDetailsView to enter details on the activity.
        /// Instead of saving to db like other DetailsViewModels, the ActivityDetailsViewModel will obtain the required information and pass it along to the exporter.
        /// </summary>
        /// <returns></returns>
        private async Task SpawnActivityDetailsViewModelAsync()
        {
            // Subscribe to the response for when the user completes the activity details
            MessagingCenter.Subscribe<ActivityManager, Activity>
                (this, MsgEvents.GetModel(typeof(Activity), MsgEvents.Event.Addition), 
                    async (ActivityManager source, Activity parameter) => await ReceiveCompletedActivityDetailsAsync(source, parameter));

            var vm = App.AppContainer.Resolve<IValidableViewModel<Activity>>();
            var v = App.AppContainer.Resolve<IDetailsView<Activity>>();

            await _pageService.NavigateToAsync(v.GetType().Name, vm);
        }

        /// <summary>
        /// Triggered when an activity is completed through the details page
        /// </summary>
        /// <param name="source"></param>
        /// <param name="parameter"></param>
        private async Task ReceiveCompletedActivityDetailsAsync(ActivityManager source, Activity parameter)
        {
            // unsubscribe from message
            MessagingCenter.Unsubscribe<ActivityManager, Activity>(this,
                MsgEvents.GetModel(typeof(Activity), MsgEvents.Event.Addition));

            await GenerateExportAsync(parameter);

        }

        /// <summary>
        /// Given the received activity, generate the export and send to user for download 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        private async Task GenerateExportAsync(Activity activity)
        {
            IsLoading = true;

            // Generate an activity export package from the activity.
            Export export = await _activityExportCreator.CreateActivityExportAsync(activity);

            // If there isn't anything to export, ask user if they are sure.
            if (export.Ednas.Count + export.Observations.Count + export.Readings.Count + export.Photos.Count == 0)
            {
                if (!await _pageService.ShowYesNoAlertAsync("Warning!",
                        "Exported activity contains no samples, observations, or photos. Continue export anyway?",
                        "Continue", "Cancel"))
                {
                    IsLoading = false; 
                    return;
                }
            }

            // Generate export package file
            string exportFilePath = await ActivityExportCreator.GenerateExportPackageAsync(export, IsIncludeAttachmentsSelected, IsUseMdmaprFormatSelected);

            // Check for success
            if (string.IsNullOrEmpty(exportFilePath))
            {
                var ans = await _pageService.ShowYesNoAlertAsync("Error!", "Could not generate export package. Copy diagnostic data to clipboard instead?", "Yes", "No");
                if (ans)
                    await Clipboard.SetTextAsync(export.ToString());

                IsLoading = false;
                return;
            }

#if DEBUG
            // For extracting file in debugger without having to go through adb
            var xxxxx = Convert.ToBase64String(File.ReadAllBytes(exportFilePath));
#endif


            // Only tag samples as exported if export was successful and soft export is not enabled.
            // Only save to db if soft export is not enabled.
            if (!IsSoftExportSelected)
            {
                await _activityExportCreator.SaveExportAsync(export);
                await _activityExportCreator.TagItemsAsExportedAsync(export);
            }


            // Finally Share the file to user
            await _fileShare.ShareFileAsync(exportFilePath);

            IsLoading = false;
        }
    }
}
