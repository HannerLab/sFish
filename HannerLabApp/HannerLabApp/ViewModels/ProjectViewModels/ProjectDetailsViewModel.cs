using System;
using System.Threading.Tasks;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;

namespace HannerLabApp.ViewModels.ProjectViewModels
{
    public class ProjectDetailsViewModel : DetailsViewModelBase<Project>
    {
        private readonly IManager<Project> _manager;

        public ProjectDetailsViewModel(IValidableViewModel<Project> viewModel, IManager<Project> manager, IPageService pageService) : base(viewModel, manager, pageService)
        {
            this._manager = manager;
        }

        /// <summary>
        /// Deletes the model from the data store which is being represented by the view model.
        /// </summary>
        /// <returns></returns>
        private protected override async Task Delete()
        {
            var ret = await _pageService.ShowYesNoAlertAsync($"Delete {ViewModel.TitleBase}?", "Are you sure?", "Yes", "No");

            if (!ret)
                return;

            Project model = ViewModel.Model;

            // Prevent user from deleting the current project
            if (model.Id == App.AppSettings.CurrentProjectId)
            {
                await _pageService.ShowAlertAsync("Error!",
                    "You can't delete the current project. Select a different project in order to delete this one.",
                    "Ok");
                return;
            }

            if (model.Id != Guid.Empty)
            {
                await _manager.DeleteItemAsync(model);
            }

            await _pageService.BackAsync();
        }
    }
}
