using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;
using HannerLabApp.Services.Repositorys;
using TinyMvvm;
using Xamarin.Essentials;

namespace HannerLabApp.ViewModels.StationViewModels
{
    /// <summary>
    /// Non generic because it contains references to parent objects (Site)
    /// </summary>
    public class StationDetailsViewModel : DetailsViewModelBase<Station>
    {
        private readonly IReadOnlyRepository<Site> _repository;
        private readonly IGeoLocator _geoLocator;

        private Site _selectedSite;
        private Location _location;

        protected bool IsLoaded { get; private set; }

        /// <summary>
        /// List of available sites that can be selected from when creating a station
        /// </summary>
        public ObservableCollection<Site> Sites
        {
            get => _sites;
            private set => Set(ref _sites, value);
        }
        private ObservableCollection<Site> _sites = new ObservableCollection<Site>();

        public ICommand DetermineGpsLocationCommand { get; private set; }

        public StationDetailsViewModel(IValidableViewModel<Station> viewModel, IManager<Station> manager, IPageService pageService, IReadOnlyRepository<Site> repository, IGeoLocator geoLocator) : base(viewModel, manager, pageService)
        {
            _repository = repository;
            _geoLocator = geoLocator;

            DetermineGpsLocationCommand = new TinyCommand(async () => await DetermineGpsLocationAsync());

            _selectedSite = viewModel.Model.Site;
        }

        public override async Task Initialize()
        {
            IsBusy = true;

            if (!IsLoaded)
            {
                await LoadData();
            }

            await base.Initialize();
            IsBusy = false;
        }

        private async Task LoadData()
        {
            IsLoaded = true;

            Sites = new ObservableCollection<Site>();

            var sites = await _repository.GetItemsAsync(App.AppSettings.CurrentProjectId);
            foreach (var p in sites)
            {
                Sites.Add(p);
            }

            if (_selectedSite != null)
            {
                var m = base.ViewModel.Model;
                m.Site = _sites.ToList().Find(x => x.Id == _selectedSite.Id);
                if (m.Site !=null) base.ViewModel.Model = m;
            }

            _location = await _geoLocator.GetGpsLocationAsync();
        }

        private async Task DetermineGpsLocationAsync()
        {
            _location ??= await _geoLocator.GetGpsLocationAsync();

            var m = this.ViewModel.Model;
            m.Latitude = _location.Latitude;
            m.Longitude = _location.Longitude;
            m.Elevation = _location.Altitude;
            this.ViewModel.Model = m;
        }
    }
}
