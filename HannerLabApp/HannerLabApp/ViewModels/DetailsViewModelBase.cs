using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;
using TinyMvvm;

namespace HannerLabApp.ViewModels
{
    /// <summary>
    /// Base class for details viewmodels.
    /// </summary>
    public abstract class DetailsViewModelBase<T> : ViewModelBase, IDetailsViewModel<T> where T : ISavable
    {
        private readonly IManager<T> _manager;
        private protected readonly IPageService _pageService;

        /// <summary>
        /// The Title that is shown in the title bar, either edit {item}, or new {item}.
        /// </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        private string _title;

        /// <summary>
        /// Whether or not this details page is for a new item or editing an existing item.
        /// </summary>
        public bool IsEdit
        {
            get => _isEdit;
            set => Set(ref _isEdit, value);
        }

        private bool _isEdit;

        public IValidableViewModel<T> ViewModel { get; protected set; }
        public ICommand SaveCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }
        public ICommand ToggleModeCommand { get; private set; }

        protected DetailsViewModelBase(IValidableViewModel<T> viewModel, IManager<T> manager, IPageService pageService)
        {
            ViewModel = viewModel ?? throw new ArgumentNullException(nameof(viewModel));

            _manager = manager;
            _pageService = pageService;

            SaveCommand = new TinyCommand(async () => await Save());
            DeleteCommand = new TinyCommand(async () => await Delete());
            ToggleModeCommand = new TinyCommand(() => ViewModel.IsAdvancedShown = !ViewModel.IsAdvancedShown);

            // Determine whether this is a new or existing entry and set the title accordingly
            IsEdit = viewModel.Id != Guid.Empty;
            Title = IsEdit ? $"Edit {ViewModel.TitleBase}" : $"New {ViewModel.TitleBase}";
        }

        /// <summary>
        /// Validates the data, and then saves the view models model into the data store, only if it is valid, otherwise shows error message. 
        /// </summary>
        /// <returns></returns>
        private async Task Save()
        {
            if (!ViewModel.Validate())
            {
                await _pageService.ShowAlertAsync("Oops!", "Some or all of the entered data is in-valid. Please make corrections as needed.", "Ok");
                return;
            }

            T model = ViewModel.Model;

            if (model.Id == Guid.Empty)
            {
                await _manager.AddItemAsync(model);
            }
            else
            {
                // Are we sure we want to edit an already exported activity?
                if (model.ActivityId != Guid.Empty)
                {
                    if (!await _pageService.ShowYesNoAlertAsync("Warning!",
                            "This item appears to have been previously export. Are you sure you would like to edit it anyway?",
                            "Edit", "Go Back")) return;
                }

                await _manager.UpdateItemAsync(model);
            }

            await _pageService.BackAsync();
        }

        /// <summary>
        /// Deletes the model from the data store which is being represented by the view model.
        /// </summary>
        /// <returns></returns>
        private protected virtual async Task Delete()
        {
            var ret = await _pageService.ShowYesNoAlertAsync($"Delete {ViewModel.TitleBase}?", "Are you sure?", "Yes", "No");

            if (!ret)
                return;

            T model = ViewModel.Model;

            if (model.Id != Guid.Empty)
            {
                await _manager.DeleteItemAsync(model);
            }

            await _pageService.BackAsync();
        }
    }
}
