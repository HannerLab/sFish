using System;
using HannerLabApp.Models;
using HannerLabApp.Utils;
using HannerLabApp.Validators;
using HannerLabApp.Validators.Rules;
using TinyMvvm;

namespace HannerLabApp.ViewModels.ReadingViewModels
{
    public class ReadingViewModel : ViewModelBase, IValidableViewModel<Reading>
    {
        private bool _isAdvancedShown;
        public bool IsAdvancedShown
        {
            get => _isAdvancedShown;
            set => Set(ref _isAdvancedShown, value);
        }
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public static readonly string TitleBaseStatic = "Instrumental Reading";
        public string TitleBase => TitleBaseStatic;

        public Reading Model
        {
            get => new Reading
            {
                Id = Id,
                IsAdvancedShown = this.IsAdvancedShown,
                ProjectId = this.ProjectId,
                Notes = this.Notes.Value,
                UserSpecifiedId = this.UserSpecifiedId.Value,
                Timestamp = this.Timestamp.Value,
                Station = this.Station.Value,
                Depth = this.Depth.Value,
                OffshoreDistance = this.OffshoreDistance.Value,
                CollectedBy = this.CollectedBy.Value,
                RecordedBy = this.RecordedBy.Value,
                Velocity = this.Velocity.Value,
                Temperature = this.Temperature.Value,
                Conductivity = this.Conductivity.Value,
                DissolvedOxygen = this.DissolvedOxygen.Value,
                Ph = this.Ph.Value,
                Secchi = this.Secchi.Value,
                Turbidity = this.Turbidity.Value,
                SuspendedSolids = this.SuspendedSolids.Value,
                Chlorophyll = this.Chlorophyll.Value,
                Equipment = this.Equipment.Value,
                Name = this.Name.Value
            };
            set
            {
                this.Id = value.Id;
                this.IsAdvancedShown = value.IsAdvancedShown;
                this.ProjectId = value.ProjectId;
                this.Notes.Value = value.Notes;
                this.UserSpecifiedId.Value = value.UserSpecifiedId;
                this.Timestamp.Value = value.Timestamp;
                this.Station.Value = value.Station;
                this.Depth.Value = value.Depth;
                this.OffshoreDistance.Value = value.OffshoreDistance;
                this.CollectedBy.Value = value.CollectedBy;
                this.RecordedBy.Value = value.RecordedBy;
                this.Name.Value = value.Name;
                this.Velocity.Value = value.Velocity;
                this.Temperature.Value = value.Temperature;
                this.Ph.Value = value.Ph;
                this.Conductivity.Value = value.Conductivity;
                this.DissolvedOxygen.Value = value.DissolvedOxygen;
                this.SuspendedSolids.Value = value.SuspendedSolids;
                this.Secchi.Value = value.Secchi;
                this.Turbidity.Value = value.Turbidity;
                this.Chlorophyll.Value = value.Chlorophyll;
                this.Equipment.Value = value.Equipment;
            }
        }

        public bool IsValid { get; private set; }

        public ValidatableObject<string> Notes { get; set; } =
            new ValidatableObject<string> { Title = "Notes", Description = "Enter any notes related to this sample." };

        public ValidatableObject<string> Name { get; set; } =
            new ValidatableObject<string> { Title = "Name", Description = "A name for this sample." };

        public ValidatableObject<string> UserSpecifiedId { get; set; } =
            new ValidatableObject<string> { Title = "Identifier (ID)", Description = "An identification string for this sample. Should be unique. Press the scan button to scan from a barcode." };

        public ValidatableObject<DateTime> Timestamp { get; set; } =
            new ValidatableObject<DateTime> { Title = "Sample collection time", Description = "The date and time of when the sample was collected." };

        public ValidatableObject<Station> Station { get; set; } =
            new ValidatableObject<Station> { Title = "Station", Description = "The station that this sample was obtained from." };

        // Actual readings..
        public ValidatableObject<double?> Depth { get; set; } =
            new ValidatableObject<double?> { Title = "Sampling Depth", Description = "The depth at which the sample was read from." };

        public ValidatableObject<double?> Velocity { get; set; } =
            new ValidatableObject<double?> { Title = "Velocity", Description = "The water velocity as read from the device" };

        public ValidatableObject<double?> Temperature { get; set; } =
            new ValidatableObject<double?> { Title = "Temperature", Description = "The water temperature as read from the device" };

        public ValidatableObject<double?> Ph { get; set; } =
            new ValidatableObject<double?> { Title = "Ph", Description = "The water pH as read from the device" };

        public ValidatableObject<double?> Conductivity { get; set; } =
            new ValidatableObject<double?> { Title = "Conductivity", Description = "The water conductivity as read from the device" };

        public ValidatableObject<double?> DissolvedOxygen { get; set; } =
            new ValidatableObject<double?> { Title = "Dissolved oxygen", Description = "The water dissolved oxygen as read from the device" };

        public ValidatableObject<double?> SuspendedSolids { get; set; } =
            new ValidatableObject<double?> { Title = "SuspendedSolids", Description = "The water suspended solids as read from the device" };

        public ValidatableObject<double?> Secchi { get; set; } =
            new ValidatableObject<double?> { Title = "Secchi", Description = "The water secchi as read from the device" };

        public ValidatableObject<double?> Turbidity { get; set; } =
            new ValidatableObject<double?> { Title = "Turbidity", Description = "The water turbidity as read from the device" };

        public ValidatableObject<double?> Chlorophyll { get; set; } =
            new ValidatableObject<double?> { Title = "Chlorophyll", Description = "The water chlorophyll as read from the device" };

        public ValidatableObject<double?> OffshoreDistance { get; set; } =
            new ValidatableObject<double?> { Title = "Distance from shore", Description = "The distance from shore which the sample was recorded from." };

        public ValidatableObject<string> CollectedBy { get; set; } =
            new ValidatableObject<string> { Title = "Collected by", Description = "The name of the person who collected this sample." };

        public ValidatableObject<string> RecordedBy { get; set; } =
            new ValidatableObject<string> { Title = "Recorded by", Description = "The name of the person who recorded this sample. Defaults to the current recorder set in the main menu." };

        public ValidatableObject<Equipment> Equipment { get; set; } =
            new ValidatableObject<Equipment> { Title = "Equipment", Description = "The equipment that was used to collect this sample." };

        public ReadingViewModel()
        {
            AddValidationRules();
            AddDefaults();
        }

        /// <summary>
        /// Validates each ValidatableObject property in this class, and returns IsValid.
        /// </summary>
        /// <returns></returns>
        public bool Validate()
        {
            bool a = Notes.Validate();
            bool b = Name.Validate();
            bool c = UserSpecifiedId.Validate();
            bool d = Timestamp.Validate();
            bool e = Station.Validate();
            bool f = Depth.Validate();
            bool g = Velocity.Validate();
            bool h = Temperature.Validate();
            bool i = Ph.Validate();
            bool j = Conductivity.Validate();
            bool k = DissolvedOxygen.Validate();
            bool l = SuspendedSolids.Validate();
            bool m = Secchi.Validate();
            bool n = Turbidity.Validate();
            bool o = Chlorophyll.Validate();
            bool p = OffshoreDistance.Validate();
            bool q = CollectedBy.Validate();
            bool r = RecordedBy.Validate();
            bool s = Equipment.Validate();

            IsValid = a && b && c && d && e && f && g && h && i && j && k && l && m && n && o && p && q && r && s;

            return IsValid;
        }

        private void AddDefaults()
        {
            if (this.Timestamp.Value == DateTime.MinValue)
                this.Timestamp.Value = DateTime.Now;

            if (string.IsNullOrEmpty(this.UserSpecifiedId.Value))
                this.UserSpecifiedId.Value = IdGenerator.GetNewRandomId();

            if (string.IsNullOrEmpty(this.RecordedBy.Value))
                this.RecordedBy.Value = App.AppSettings.CurrentRecorder;

            if (this.Id == Guid.Empty)
                this.IsAdvancedShown = App.AppSettings.IsAdvanceModeDefaultEnabled;

            if (string.IsNullOrEmpty(this.Name.Value))
                this.Name.Value = this.UserSpecifiedId.Value;
        }

        private void AddValidationRules()
        {
            UserSpecifiedId.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a sample identifier." });
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a sample name." });

            Station.Validations.Add(new IsNotNullOrEmptyRule<Station> { ValidationMessage = "Must supply a station." });
        }
    }
}
