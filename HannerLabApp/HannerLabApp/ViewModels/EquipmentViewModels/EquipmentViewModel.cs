using System;
using System.Collections.ObjectModel;
using System.Linq;
using HannerLabApp.Models;
using HannerLabApp.Utils;
using HannerLabApp.Validators;
using HannerLabApp.Validators.Rules;
using TinyMvvm;

namespace HannerLabApp.ViewModels.EquipmentViewModels
{
    public class EquipmentViewModel : ViewModelBase, IValidableViewModel<Equipment>
    {
        private bool _isAdvancedShown;
        public bool IsAdvancedShown
        {
            get => _isAdvancedShown;
            set => Set(ref _isAdvancedShown, value);
        }

        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public static string TitleBaseStatic => "Equipment Device";
        public string TitleBase => TitleBaseStatic;

        public Equipment Model
        {
            get
            {
                var e = new Equipment()
                {
                    Id = Id,
                    IsAdvancedShown = this.IsAdvancedShown,
                    ProjectId = this.ProjectId,
                    Description = this.Description.Value,
                    Notes = this.Notes.Value,
                    UserSpecifiedId = this.UserSpecifiedId.Value,
                    Parameters = this.Parameters.Value,
                    Manufacturer = this.Manufacturer.Value,
                    Vendor = this.Vendor.Value,
                    SerialNumber = this.SerialNumber.Value,
                    DeviceModel = this.DeviceModel.Value,
                    Category = this.Category.Value,
                    Name = this.Name.Value,
                    RecordedBy = this.RecordedBy.Value,
                    Timestamp = this.Timestamp.Value
                };

                // Flatten units
                foreach (var i in this.Units.Value)
                {
                    switch (i.UnitType)
                    {
                        case UnitType.Chlorophyll:
                            e.UnitChlorophyll = i.UnitValue;
                            break;
                        case UnitType.Temperature:
                            e.UnitTemperature = i.UnitValue;
                            break;
                        case UnitType.Depth:
                            e.UnitDepth = i.UnitValue;
                            break;
                        case UnitType.Conductivity:
                            e.UnitConductivity = i.UnitValue;
                            break;
                        case UnitType.DissolvedOxygen:
                            e.UnitDissolvedOxygen = i.UnitValue;
                            break;
                        case UnitType.FlowRate:
                            e.UnitFlowRate = i.UnitValue;
                            break;
                        case UnitType.Gps:
                            e.UnitGps = i.UnitValue;
                            break;
                        case UnitType.OffshoreDistance:
                            e.UnitOffshoreDistance = i.UnitValue;
                            break;
                        case UnitType.Ph:
                            e.UnitPh = i.UnitValue;
                            break;
                        case UnitType.Pressure:
                            e.UnitPressure = i.UnitValue;
                            break;
                        case UnitType.Secchi:
                            e.UnitSecchi = i.UnitValue;
                            break;
                        case UnitType.SuspendedSolids:
                            e.UnitSuspendedSolids = i.UnitValue;
                            break;
                        case UnitType.VolumeFiltered:
                            e.UnitVolumeFiltered = i.UnitValue;
                            break;
                        case UnitType.TimeFiltering:
                            e.UnitTimeFiltering = i.UnitValue;
                            break;
                        case UnitType.Velocity:
                            e.UnitVelocity = i.UnitValue;
                            break;
                        case UnitType.Turbidity:
                            e.UnitTurbidity = i.UnitValue;
                            break;
                    }
                }

                return e;
            }
            set
            {
                this.Id = value.Id;
                this.RecordedBy.Value = value.RecordedBy;
                this.IsAdvancedShown = value.IsAdvancedShown;
                this.ProjectId = value.ProjectId;
                this.Name.Value = value.Name;
                this.Description.Value = value.Description;
                this.Notes.Value = value.Notes;
                this.UserSpecifiedId.Value = value.UserSpecifiedId;
                this.Parameters.Value = value.Parameters;
                this.Manufacturer.Value = value.Manufacturer;
                this.Vendor.Value = value.Vendor;
                this.SerialNumber.Value = value.SerialNumber;
                this.DeviceModel.Value = value.DeviceModel;
                this.Category.Value = value.Category;
                this.RecordedBy.Value = value.RecordedBy;
                this.Timestamp.Value = value.Timestamp;

                // Un-flatten units
                var units = new ObservableCollection<UnitViewModel>();
                if (!string.IsNullOrEmpty(value.UnitDepth)) units.Add(new UnitViewModel() { UnitType = UnitType.Depth, UnitValue = value.UnitDepth });
                if (!string.IsNullOrEmpty(value.UnitFlowRate)) units.Add(new UnitViewModel() { UnitType = UnitType.FlowRate, UnitValue = value.UnitFlowRate });
                if (!string.IsNullOrEmpty(value.UnitTemperature)) units.Add(new UnitViewModel() { UnitType = UnitType.Temperature, UnitValue = value.UnitTemperature });
                if (!string.IsNullOrEmpty(value.UnitPh)) units.Add(new UnitViewModel() { UnitType = UnitType.Ph, UnitValue = value.UnitPh });
                if (!string.IsNullOrEmpty(value.UnitConductivity)) units.Add(new UnitViewModel() { UnitType = UnitType.Conductivity, UnitValue = value.UnitConductivity });
                if (!string.IsNullOrEmpty(value.UnitDissolvedOxygen)) units.Add(new UnitViewModel() { UnitType = UnitType.DissolvedOxygen, UnitValue = value.UnitDissolvedOxygen });
                if (!string.IsNullOrEmpty(value.UnitGps)) units.Add(new UnitViewModel() { UnitType = UnitType.Gps, UnitValue = value.UnitGps });
                if (!string.IsNullOrEmpty(value.UnitChlorophyll)) units.Add(new UnitViewModel() { UnitType = UnitType.Chlorophyll, UnitValue = value.UnitChlorophyll });
                if (!string.IsNullOrEmpty(value.UnitSecchi)) units.Add(new UnitViewModel() { UnitType = UnitType.Secchi, UnitValue = value.UnitSecchi });
                if (!string.IsNullOrEmpty(value.UnitOffshoreDistance)) this.Units.Value.Add(new UnitViewModel() { UnitType = UnitType.OffshoreDistance, UnitValue = value.UnitOffshoreDistance });
                if (!string.IsNullOrEmpty(value.UnitPressure)) units.Add(new UnitViewModel() { UnitType = UnitType.Pressure, UnitValue = value.UnitPressure });
                if (!string.IsNullOrEmpty(value.UnitVolumeFiltered)) units.Add(new UnitViewModel() { UnitType = UnitType.VolumeFiltered, UnitValue = value.UnitVolumeFiltered });
                if (!string.IsNullOrEmpty(value.UnitVelocity)) units.Add(new UnitViewModel() { UnitType = UnitType.Velocity, UnitValue = value.UnitVelocity });
                if (!string.IsNullOrEmpty(value.UnitTurbidity)) units.Add(new UnitViewModel() { UnitType = UnitType.Turbidity, UnitValue = value.UnitTurbidity });
                if (!string.IsNullOrEmpty(value.UnitTimeFiltering)) units.Add(new UnitViewModel() { UnitType = UnitType.TimeFiltering, UnitValue = value.UnitTimeFiltering });
                if (!string.IsNullOrEmpty(value.UnitSuspendedSolids)) units.Add(new UnitViewModel() { UnitType = UnitType.SuspendedSolids, UnitValue = value.UnitSuspendedSolids });


                this.Units.Value = units;
            }
        }

        public bool IsValid { get; set; }

        public ValidatableObject<ObservableCollection<UnitViewModel>> Units { get; set; } =
            new ValidatableObject<ObservableCollection<UnitViewModel>> { Title = "Units", Description = "Enter all units used by this equipment as well as what they are measuring. An equipment may only have a single value per measurement type." };

        public ValidatableObject<string> UserSpecifiedId { get; set; } =
            new ValidatableObject<string> { Title = "Identifier (ID)", Description = "An identification string for this equipment." };
        public ValidatableObject<DateTime> Timestamp { get; set; } =
            new ValidatableObject<DateTime> { Title = "Timestamp", Description = "The date and time of when this equipment was added." };

        public ValidatableObject<string> Name { get; set; } =
            new ValidatableObject<string> { Title = "Name", Description = "The name of the equipment." };

        public ValidatableObject<string> Description { get; set; } =
            new ValidatableObject<string> { Title = "Description", Description = "A general description of the equipment." };

        public ValidatableObject<string> Notes { get; set; } =
            new ValidatableObject<string> { Title = "Notes", Description = "Any notes related to the equipment can be added here." };

        public ValidatableObject<string> Parameters { get; set; } =
            new ValidatableObject<string> { Title = "Parameters", Description = "Any settings or parameters this equipment is using." };

        public ValidatableObject<string> Manufacturer { get; set; } =
            new ValidatableObject<string> { Title = "Manufacturer", Description = "The equipments manufacturer." };

        public ValidatableObject<string> Vendor { get; set; } =
            new ValidatableObject<string> { Title = "Vendor", Description = "The equipments vendor." };

        public ValidatableObject<string> SerialNumber { get; set; } =
            new ValidatableObject<string> { Title = "Serial Number", Description = "The equipments serial number." };

        public ValidatableObject<string> DeviceModel { get; set; } =
            new ValidatableObject<string> { Title = "Model", Description = "The specific model of the equipment." };

        public ValidatableObject<EquipmentType> Category { get; set; } =
            new ValidatableObject<EquipmentType> { Title = "Category", Description = "The category that this device belongs to." };

        public ValidatableObject<string> RecordedBy { get; set; } =
            new ValidatableObject<string> { Title = "Recorded by", Description = "The name of the person who added this equipment." };


        public EquipmentViewModel()
        {
            AddValidationRules();
            AddDefaults();
        }


        public bool Validate()
        {
            bool a = Name.Validate();
            bool b = Description.Validate();
            bool c = Notes.Validate();
            bool d = UserSpecifiedId.Validate();
            bool e = Units.Validate();
            bool f = Parameters.Validate();
            bool g = Manufacturer.Validate();
            bool h = Vendor.Validate();
            bool i = SerialNumber.Validate();
            bool j = DeviceModel.Validate();
            bool k = Category.Validate();
            bool l = RecordedBy.Validate();

            IsValid = a && b && c && d && e && f && g && h && i && j && k && l;

            return IsValid;
        }

        private void AddDefaults()
        {
            if (string.IsNullOrEmpty(this.UserSpecifiedId.Value))
                this.UserSpecifiedId.Value = IdGenerator.GetNewRandomId();

            if (this.Timestamp.Value == DateTime.MinValue)
                this.Timestamp.Value = DateTime.Now;

            if (string.IsNullOrEmpty(this.RecordedBy.Value))
                this.RecordedBy.Value = App.AppSettings.CurrentRecorder;

            if (this.Id == Guid.Empty)
                this.IsAdvancedShown = App.AppSettings.IsAdvanceModeDefaultEnabled;

            // Start with an empty unit
            if (Units.Value == null || Units.Value.Count == 0)
            {
                Units.Value = new ObservableCollection<UnitViewModel>();
                Units.Value.Add(new UnitViewModel());
            }
                
        }

        private void AddValidationRules()
        {
            UserSpecifiedId.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply an identifier." });
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply an device name." });
        }

    }
}
