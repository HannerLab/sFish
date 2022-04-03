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

namespace HannerLabApp.ViewModels
{
    /// <summary>
    /// Similar to the generic DetailsViewModel, except contains the list of available stations to choose from
    /// </summary>
    public class SampleDetailsViewModel<T> : DetailsViewModelBase<T> where T : ISample
    {
        private readonly IReadOnlyRepository<Station> _stationRepo;
        private readonly IReadOnlyRepository<Equipment> _equipmentRepo;
        private readonly IBarcodeScanner _barcodeScanner;

        private ObservableCollection<Station> _stations = new ObservableCollection<Station>();
        private ObservableCollection<Equipment> _equipments = new ObservableCollection<Equipment>();

        private Station _selectedStation;
        private Equipment _selectedEquipment;

        /// <summary>
        /// List of available sites that can be selected from when creating a station
        /// </summary>
        public ObservableCollection<Station> Stations
        {
            get => _stations;
            private set => Set(ref _stations, value);
        }

        public ObservableCollection<Equipment> Equipments
        {
            get => _equipments;
            private set => Set(ref _equipments, value);
        }

        protected bool IsLoaded { get; private protected set; }


        /// <summary>
        /// Command to open the scanner and populate the UserGeneratedId field with the scanned value
        /// </summary>
        public ICommand ScanBarcodeCommand { get; private set; }

        public SampleDetailsViewModel(IValidableViewModel<T> viewModel, IManager<T> manager, IPageService pageService, IReadOnlyRepository<Station> stationRepo, IReadOnlyRepository<Equipment> equipmentRepo, IBarcodeScanner barcodeScanner) : base(viewModel, manager, pageService)
        {
            _stationRepo = stationRepo;
            _equipmentRepo = equipmentRepo;
            _barcodeScanner = barcodeScanner;

            _selectedStation = viewModel.Model.Station;
            _selectedEquipment = viewModel.Model.Equipment;

            ScanBarcodeCommand = new TinyCommand(async () => await ScanBarcodeAsync());
        }


        // Set user specified Id to what is scanned from barcode
        private async Task ScanBarcodeAsync()
        {
            var result = await _barcodeScanner.ScanBarcodeAsync();

            if (string.IsNullOrEmpty(result)) return;

            var m = ViewModel.Model;
            m.UserSpecifiedId = result;
            ViewModel.Model = m;
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

            // Equipments
            {
                Equipments = new ObservableCollection<Equipment>();

                var equipments = await _equipmentRepo.GetItemsAsync(App.AppSettings.CurrentProjectId);
                foreach (var p in equipments)
                {
                    Equipments.Add(p);
                }

                // Set selected station in bindings
                if (_selectedEquipment != null)
                {
                    var m = base.ViewModel.Model;
                    // For picker binding to work properly, it must be one of the objects actually within the list.
                    m.Equipment = _equipments.ToList().Find(x => x.Id == _selectedEquipment.Id);
                    if (m.Equipment != null) base.ViewModel.Model = m;
                }
            }

        }
    }
}
