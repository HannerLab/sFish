using System;
using System.IO;
using System.Threading.Tasks;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Exporters;
using HannerLabApp.Services.Media;
using HannerLabApp.Services.Repositorys;
using Xamarin.Essentials;

namespace HannerLabApp.ViewModels.ActivityViewModels
{
    public class ActivityHistoryListViewModel : ListViewModelBase<Export>
    {
        private readonly IFileShare _fileShare;
        private bool _isLoading;

        public bool IsLoading
        {
            get => _isLoading;
            set => Set(ref _isLoading, value);
        }

        public ActivityHistoryListViewModel(IPageService pageService, IReadOnlyRepository<Export> repository, IFileShare fileShare) : base(pageService, repository)
        {
            _fileShare = fileShare;
        }


        private protected override async Task EditItem(IValidableViewModel<Export> viewModel)
        {
            if (viewModel == null) return;

            var b = await _pageService.ShowYesNoAlertAsync("Re-create export package?", "Would you like to export this activity to file again?", "Yes", "No");
            if (!b) return;

            bool includeAttachments = await _pageService.ShowYesNoAlertAsync("Re-creating export package", "Include attachments?", "Yes", "No");
            bool useMdmaprFormat = await _pageService.ShowYesNoAlertAsync("Re-creating export package", "Use MDMAPR format?", "Yes", "No");

            IsLoading = true;

            Export export = viewModel.Model;

            // Generate export package file
            string exportFilePath = await ActivityExportCreator.GenerateExportPackageAsync(export, includeAttachments, useMdmaprFormat);

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

            // Finally Share the file to user
            await _fileShare.ShareFileAsync(exportFilePath);

            IsLoading = false;
        }
    }
}
