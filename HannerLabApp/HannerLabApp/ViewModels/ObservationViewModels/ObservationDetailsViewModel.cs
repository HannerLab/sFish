using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;
using HannerLabApp.Services.Repositorys;

namespace HannerLabApp.ViewModels.ObservationViewModels
{
    public class ObservationDetailsViewModel : DetailsViewModelBase<Observation>
    {
        private readonly IReadOnlyRepository<Station> _stationRepo;
        private readonly IReadOnlyRepository<Site> _siteRepo;

        private ObservableCollection<Station> _stations = new ObservableCollection<Station>();
        private Station _selectedStation;

        private ObservableCollection<Site> _sites = new ObservableCollection<Site>();
        private Site _selectedSite;

        /// <summary>
        /// List of available stations that can be selected from
        /// </summary>
        public ObservableCollection<Station> Stations
        {
            get => _stations;
            private set => Set(ref _stations, value);
        }

        /// <summary>
        /// List of available sites that can be selected from
        /// </summary>
        public ObservableCollection<Site> Sites
        {
            get => _sites;
            private set => Set(ref _sites, value);
        }

        protected bool IsLoaded { get; private protected set; }

        public ObservationDetailsViewModel(IValidableViewModel<Observation> viewModel, IManager<Observation> manager, IPageService pageService, IReadOnlyRepository<Station> stationRepo, IReadOnlyRepository<Site> siteRepo) : base(viewModel, manager, pageService)
        {
            _stationRepo = stationRepo;
            _siteRepo = siteRepo;

            _selectedStation = viewModel.Model.Station;
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

        protected virtual async Task LoadData()
        {
            IsLoaded = true;

            // Stations
            {
                Stations = new ObservableCollection<Station>();

                var stations = await _stationRepo.GetItemsAsync(App.AppSettings.CurrentProjectId);
                foreach (var p in stations)
                {
                    Stations.Add(p);
                }

                // Set selected station in bindings
                if (_selectedStation != null)
                {
                    var m = base.ViewModel.Model;
                    // For picker binding to work properly, it must be one of the objects actually within the list.
                    m.Station = _stations.ToList().Find(x => x.Id == _selectedStation.Id);
                    if (m.Station != null) base.ViewModel.Model = m;
                }
            }

            // Sites
            {
                Sites = new ObservableCollection<Site>();

                var sites = await _siteRepo.GetItemsAsync(App.AppSettings.CurrentProjectId);
                foreach (var p in sites)
                {
                    Sites.Add(p);
                }

                // Set selected site in bindings
                if (_selectedSite != null)
                {
                    var m = base.ViewModel.Model;
                    // For picker binding to work properly, it must be one of the objects actually within the list.
                    m.Site = _sites.ToList().Find(x => x.Id == _selectedSite.Id);
                    if (m.Site != null) base.ViewModel.Model = m;
                }
            }

        }
    }
}
