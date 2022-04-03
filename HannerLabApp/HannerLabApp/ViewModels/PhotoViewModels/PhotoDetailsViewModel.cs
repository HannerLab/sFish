using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;
using HannerLabApp.Services.Media;
using HannerLabApp.Services.Repositorys;
using TinyMvvm;

namespace HannerLabApp.ViewModels.PhotoViewModels
{
    public class PhotoDetailsViewModel : DetailsViewModelBase<Photo>
    {
        private readonly IReadOnlyRepository<Station> _stationRepo;
        private readonly IReadOnlyRepository<Site> _siteRepo;
        private readonly IReadOnlyRepository<Observation> _observationRepo;
        private readonly IReadOnlyRepository<Edna> _ednaRepo;
        private readonly IReadOnlyRepository<Reading> _readingRepo;
        private readonly IPhotoCaptureService _photoService;

        private ObservableCollection<Station> _stations = new ObservableCollection<Station>();
        private Station _selectedStation;

        private ObservableCollection<Site> _sites = new ObservableCollection<Site>();
        private Site _selectedSite;

        private ObservableCollection<Observation> _observations = new ObservableCollection<Observation>();
        private Observation _selectedObservation;

        private ObservableCollection<Edna> _ednas = new ObservableCollection<Edna>();
        private Edna _selectedEdna;

        private ObservableCollection<Reading> _readings = new ObservableCollection<Reading>();
        private Reading _selectedReading;

        public ObservableCollection<Station> Stations
        {
            get => _stations;
            private set => Set(ref _stations, value);
        }

        public ObservableCollection<Site> Sites
        {
            get => _sites;
            private set => Set(ref _sites, value);
        }

        public ObservableCollection<Observation> Observations
        {
            get => _observations;
            private set => Set(ref _observations, value);
        }

        public ObservableCollection<Edna> Ednas
        {
            get => _ednas;
            private set => Set(ref _ednas, value);
        }

        public ObservableCollection<Reading> Readings
        {
            get => _readings;
            private set => Set(ref _readings, value);
        }

        protected bool IsLoaded { get; private protected set; }

        public ICommand CapturePhotoCommand { get; private set; }
        public ICommand SelectPhotoCommand { get; private set; }
        public ICommand RemovePhotoCommand { get; private set; }

        public PhotoDetailsViewModel(IValidableViewModel<Photo> viewModel, IManager<Photo> manager, IPageService pageService, IPhotoCaptureService photoService, IReadOnlyRepository<Station> stationRepo, IReadOnlyRepository<Site> siteRepo, IReadOnlyRepository<Observation> observationRepo, IReadOnlyRepository<Edna> ednaRepo, IReadOnlyRepository<Reading> readingRepo) : base(viewModel, manager, pageService)
        {
            _stationRepo = stationRepo;
            _siteRepo = siteRepo;
            _observationRepo = observationRepo;
            _ednaRepo = ednaRepo;
            _photoService = photoService;
            _readingRepo = readingRepo;

            _selectedStation = viewModel.Model.Station;
            _selectedSite = viewModel.Model.Site;
            _selectedObservation = viewModel.Model.Observation;
            _selectedEdna = viewModel.Model.Edna;
            _selectedReading = viewModel.Model.Reading;

            CapturePhotoCommand = new TinyCommand(async () => await CapturePhotoAsync());
            SelectPhotoCommand = new TinyCommand(async () => await SelectPhotoAsync());
            RemovePhotoCommand = new TinyCommand(async () => await RemovePhotoAsync());
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
                this.Stations = new ObservableCollection<Station>();

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
                this.Sites = new ObservableCollection<Site>();

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

            // Observations
            {
                this.Observations = new ObservableCollection<Observation>();

                var observations = await _observationRepo.GetItemsAsync(App.AppSettings.CurrentProjectId);
                foreach (var p in observations)
                {
                    Observations.Add(p);
                }

                // Set selected site in bindings
                if (_selectedObservation != null)
                {
                    var m = base.ViewModel.Model;
                    // For picker binding to work properly, it must be one of the objects actually within the list.
                    m.Observation = _observations.ToList().Find(x => x.Id == _selectedObservation.Id);
                    if (m.Observation != null) base.ViewModel.Model = m;
                }
            }

            // Ednas
            {
                this.Ednas = new ObservableCollection<Edna>();

                var ednas = await _ednaRepo.GetItemsAsync(App.AppSettings.CurrentProjectId);
                foreach (var p in ednas)
                {
                    Ednas.Add(p);
                }

                // Set selected site in bindings
                if (_selectedEdna != null)
                {
                    var m = base.ViewModel.Model;
                    // For picker binding to work properly, it must be one of the objects actually within the list.
                    m.Edna = _ednas.ToList().Find(x => x.Id == _selectedEdna.Id);
                    if (m.Edna != null) base.ViewModel.Model = m;
                }
            }

            // Readings
            {
                this.Readings = new ObservableCollection<Reading>();

                var readings = await _readingRepo.GetItemsAsync(App.AppSettings.CurrentProjectId);
                foreach (var p in readings)
                {
                    Readings.Add(p);
                }

                // Set selected site in bindings
                if (_selectedReading != null)
                {
                    var m = base.ViewModel.Model;
                    // For picker binding to work properly, it must be one of the objects actually within the list.
                    m.Reading = _readings.ToList().Find(x => x.Id == _selectedReading.Id);
                    if (m.Reading != null) base.ViewModel.Model = m;
                }
            }

        }

        private async Task CapturePhotoAsync()
        {
            var res = await _photoService.CapturePhotoAsync(App.AppSettings.IsSavePhotosToGalleryEnabled);

            if (!string.IsNullOrEmpty(res))
            {
                var m = ViewModel.Model;
                m.File64 = res;
                this.ViewModel.Model = m;
            }
        }

        private async Task SelectPhotoAsync()
        {
            var res = await _photoService.PickPhotoAsync();

            if (!string.IsNullOrEmpty(res))
            {
                var m = ViewModel.Model;
                m.File64 = res;
                this.ViewModel.Model = m;
            }
        }

        private bool _hasPhotoRemovalWarningBeenDisplayed = false;
        private async Task RemovePhotoAsync()
        {
            // Disallow swapping photo... They should just create a new entry and delete this one
            if (ViewModel.Model.Id != Guid.Empty)
            {
                if (!_hasPhotoRemovalWarningBeenDisplayed)
                {
                    await _pageService.ShowAlertAsync("Error!",
                        "You can't edit the photo once submitted. Please delete this record and create a new record for the new photo if this is your intention.",
                        "ok");
                    _hasPhotoRemovalWarningBeenDisplayed = true;
                }

                return;
            }

            var res = await _pageService.ShowYesNoAlertAsync("Are you sure?", "Would you like to remove and select a new image?", "yes",
                "no");

            if (res)
            {
                var m = ViewModel.Model;
                m.File64 = string.Empty;
                this.ViewModel.Model = m;
            }
        }
    }
}
