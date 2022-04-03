using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;
using HannerLabApp.Services.Repositorys;
using HannerLabApp.Utils;
using HannerLabApp.Views;
using TinyMvvm;
using Xamarin.Forms;

namespace HannerLabApp.ViewModels
{
    /// <summary>
    /// A generic view model that can display a list of IValidableViewModels of type T, and allow for loading them from the data store, editing, adding, or deleting
    /// /// </summary>
    /// <typeparam name="T">The model in which is ultimately represented by this view model</typeparam>
    public abstract class ListViewModelBase<T> : ViewModelBase, IListViewModel<T> where T: ISavable
    {
        private const int SearchMinLength = 3;

        private protected readonly IPageService _pageService;
        private protected readonly IReadOnlyRepository<T> _repository;

        /// <summary>
        /// Whether or not the data to populate the list has been loaded or not. 
        /// </summary>
        protected bool IsLoaded { get; set; }

        /// <summary>
        /// The text to search through the list with
        /// </summary>
        public string SearchText
        {
            get => _searchText;
            set
            {
                if (value == null) return;
                Set(ref _searchText, value);
                SearchAndFilter();
            }
        }
        private string _searchText;

        /// <summary>
        /// The currently selected item in the observation 
        /// </summary>
        public IValidableViewModel<T> SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private IValidableViewModel<T> _selectedItem;

        /// <summary>
        /// The collection of all available data
        /// </summary>
        public ObservableCollection<IValidableViewModel<T>> AllItems
        {
            get => _allItems;
            set
            {
                if (value == null) return;
                Set(ref _allItems, value);
            }
        }
        private ObservableCollection<IValidableViewModel<T>> _allItems = new ObservableCollection<IValidableViewModel<T>>();

        /// <summary>
        /// The filtered collection based on the search string.
        /// </summary>
        public ObservableCollection<IValidableViewModel<T>> FilteredItems
        {
            get => _filteredItems;
            set
            {
                if (value == null) return;
                Set(ref _filteredItems, value);
            }
        }
        private ObservableCollection<IValidableViewModel<T>> _filteredItems = new ObservableCollection<IValidableViewModel<T>>();

        public ICommand SelectionChangeCommand { get; private set; }
        public ICommand AddCommand { get; private set; }
        public ICommand EditCommand { get; private set; }
        public ICommand DeleteCommand { get; private set; }

        public ListViewModelBase(IPageService pageService, IReadOnlyRepository<T> repository)
        {
            _pageService = pageService;
            _repository = repository;

            SelectionChangeCommand = new TinyCommand<IValidableViewModel<T>>(async (x) => await EditItem(x));
            AddCommand = new TinyCommand(async () => await AddItem());
            DeleteCommand = new TinyCommand<IValidableViewModel<T>>(async (x) => await DeleteItem(x));
            EditCommand = new TinyCommand<IValidableViewModel<T>>(async (x) => await EditItem(x));

            // Subscribe to message service. Updated without reloading. Much more efficient.
            MessagingCenter.Subscribe<GenericManager<T>, T>
                (this, MsgEvents.GetModel(typeof(T), MsgEvents.Event.Addition), OnAdded);
            MessagingCenter.Subscribe<GenericManager<T>, T>
                (this, MsgEvents.GetModel(typeof(T), MsgEvents.Event.Update), OnUpdated);
            MessagingCenter.Subscribe<GenericManager<T>, T>
                (this, MsgEvents.GetModel(typeof(T), MsgEvents.Event.Deletion), OnDeleted);
        }

        /// <summary>
        /// Initialization of this viewmodel. Async alternative to constructor. Loads up the data from the data store.
        /// </summary>
        /// <returns></returns>
        public override async Task Initialize()
        {
            IsBusy = true;

            if (!IsLoaded)
                await LoadData();

            await base.Initialize();
            IsBusy = false;
        }

        protected virtual async Task LoadData()
        {
            IsLoaded = true;

            AllItems = new ObservableCollection<IValidableViewModel<T>>();

            var ts = await _repository.GetItemsAsync();
            foreach (var t in ts)
            {
                var vm = App.AppContainer.Resolve<IValidableViewModel<T>>();
                vm.Model = t;

                AllItems.Add(vm);
            }

            FilteredItems = AllItems;
        }


        private void OnAdded(GenericManager<T> source, T parameter)
        {
            var vm = App.AppContainer.Resolve<IValidableViewModel<T>>();
            vm.Model = parameter;

            AllItems.Add(vm);
        }

        private void OnUpdated(GenericManager<T> source, T parameter)
        {
            var i = AllItems.IndexOf(AllItems.Where(x => x.Id == parameter.Id).FirstOrDefault());

            if (i < 0) return;

            var vm = App.AppContainer.Resolve<IValidableViewModel<T>>();
            vm.Model = parameter;

            AllItems[i] = vm;
        }

        private void OnDeleted(GenericManager<T> source, T parameter)
        {
            var i = AllItems.IndexOf(AllItems.Where(x => x.Id == parameter.Id).FirstOrDefault());

            if (i < 0) return;

            AllItems.RemoveAt(i);
        }

        private protected virtual async Task AddItem()
        {
            var vm = App.AppContainer.Resolve<IValidableViewModel<T>>();
            var v = App.AppContainer.Resolve<IDetailsView<T>>();

            await _pageService.NavigateToAsync(v.GetType().Name, vm);
        }

        private async Task DeleteItem(IValidableViewModel<T> viewModel)
        {
            await Task.Delay(1);
            // Only support deleting from within the details page... :)
            throw new NotImplementedException();
            
            //if (viewModel == null) return;
            //if ((await _pageService.ShowYesNoAlertAsync($"Generic delete message?", "Are you sure?", "Yes", "No")))
            //{
            //    await _manager.DeleteItemAsync(viewModel.Model);
            //}
        }

        private protected virtual async Task EditItem(IValidableViewModel<T> viewModel)
        {
            if (viewModel == null) return;
            var v = App.AppContainer
                .Resolve<IDetailsView<T>>(new NamedParameter("viewModel", viewModel));
            await _pageService.NavigateToAsync(v);

            // Clear selection
            this.SelectedItem = null;
        }

        private void SearchAndFilter()
        {
            var searchText = _searchText.Trim();

            // Do nothing if the search text isn't long enough.
            if (searchText.Length < SearchMinLength && searchText != string.Empty)
            {
                return;
            }

            // Filter/search collection. Any property that contains the search string.
            var filteredItems = AllItems
                .Where(m => m.GetType()
                    .GetProperties()
                    .Any(x => x.GetValue(m, null) != null && x.GetValue(m, null)
                        .ToString()
                        .Contains(searchText)));

            FilteredItems = new ObservableCollection<IValidableViewModel<T>>(filteredItems);
        }

        public override Task OnAppearing()
        {
            // Clear selection
            this.SelectedItem = null;
            return base.OnAppearing();
        }

        public override Task OnDisappearing()
        {
            // Clear selection
            this.SelectedItem = null;
            return base.OnDisappearing();
        }
    }
}
