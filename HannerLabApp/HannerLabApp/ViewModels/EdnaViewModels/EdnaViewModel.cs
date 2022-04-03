using System;
using HannerLabApp.Models;
using HannerLabApp.Utils;
using HannerLabApp.Validators;
using HannerLabApp.Validators.Rules;
using TinyMvvm;

namespace HannerLabApp.ViewModels.EdnaViewModels
{
    public class EdnaViewModel : ViewModelBase, IValidableViewModel<Edna>
    {
        private bool _isAdvancedShown;
        public bool IsAdvancedShown
        {
            get => _isAdvancedShown;
            set => Set(ref _isAdvancedShown, value);
        }

        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public static string TitleBaseStatic = "e-DNA Filter Sample";
        public string TitleBase => TitleBaseStatic;

        public Edna Model
        {
            get => new Edna
            {
                Id = Id,
                IsAdvancedShown = this.IsAdvancedShown,
                ProjectId = this.ProjectId,
                Notes = this.Notes.Value,
                UserSpecifiedId = this.UserSpecifiedId.Value,
                Timestamp = this.Timestamp.Value,
                Station = this.Station.Value,
                FlowRate = this.FlowRate.Value,
                TimeFiltering = this.TimeFiltering.Value,
                VolumeFiltered = this.VolumeFiltered.Value,
                Depth = this.Depth.Value,
                OffshoreDistance = this.OffshoreDistance.Value,
                CollectedBy = this.CollectedBy.Value,
                RecordedBy = this.RecordedBy.Value,
                Pressure = this.Pressure.Value,
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
                this.FlowRate.Value = value.FlowRate;
                this.TimeFiltering.Value = value.TimeFiltering;
                this.VolumeFiltered.Value = value.VolumeFiltered;
                this.Depth.Value = value.Depth;
                this.OffshoreDistance.Value = value.OffshoreDistance;
                this.CollectedBy.Value = value.CollectedBy;
                this.RecordedBy.Value = value.RecordedBy;
                this.Name.Value = value.Name;
                this.Pressure.Value = value.Pressure;
                this.Equipment.Value = value.Equipment;
            }
        }

        public bool IsValid { get; private set; }

        public ValidatableObject<string> Notes { get; set; } =
            new ValidatableObject<string> { Title = "Notes", Description = "Enter any notes related to this sample." };

        public ValidatableObject<string> Name { get; set; } =
            new ValidatableObject<string> { Title = "Name", Description = "A name for this sample." };

        public ValidatableObject<string> CollectedBy { get; set; } =
            new ValidatableObject<string> { Title = "Collected by", Description = "The name of the person who collected this sample." };

        public ValidatableObject<string> RecordedBy { get; set; } =
            new ValidatableObject<string> { Title = "Recorded by", Description = "The name of the person who recorded this sample." };

        public ValidatableObject<string> UserSpecifiedId { get; set; } =
            new ValidatableObject<string> { Title = "Identifier (ID)", Description = "An identification string for this sample. Should be unique. Press the scan button to scan from a barcode." };

        public ValidatableObject<DateTime> Timestamp { get; set; } =
            new ValidatableObject<DateTime> { Title = "Sample collection time", Description = "The date and time of when the sample was collected." };

        public ValidatableObject<Station> Station { get; set; } = 
            new ValidatableObject<Station>{ Title = "Station", Description = "The station that this sample was obtained from."};

        // Actual measurements
        public ValidatableObject<double?> FlowRate { get; set; } =
            new ValidatableObject<double?> { Title = "Flow Rate", Description = "The average flow rate of the sampling." };

        public ValidatableObject<double?> Pressure { get; set; } =
            new ValidatableObject<double?> { Title = "Pressure", Description = "The Pressure of the sampling." };

        public ValidatableObject<double?> VolumeFiltered { get; set; } =
            new ValidatableObject<double?> { Title = "Volume Filtered", Description = "The total volume of water which was filtered." };

        public ValidatableObject<long?> TimeFiltering { get; set; } =
            new ValidatableObject<long?> { Title = "Time Filtering", Description = "The total time which the sampled was filtered through for." };

        public ValidatableObject<double?> Depth { get; set; } =
            new ValidatableObject<double?> { Title = "Sampling Depth", Description = "The depth at which the sample was read from." };

        public ValidatableObject<double?> OffshoreDistance { get; set; } =
            new ValidatableObject<double?> { Title = "Distance From Shore", Description = "The distance from shore which the sample was obtained from." };

        public ValidatableObject<Equipment> Equipment { get; set; } =
            new ValidatableObject<Equipment> { Title = "Equipment", Description = "The equipment that was used to collect this sample." };


        public EdnaViewModel()
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
            bool c = CollectedBy.Validate();
            bool d = RecordedBy.Validate();
            bool e = UserSpecifiedId.Validate();
            bool f = Timestamp.Validate();
            bool g = Station.Validate();
            bool h = FlowRate.Validate();
            bool i = Pressure.Validate();
            bool j = VolumeFiltered.Validate();
            bool k = TimeFiltering.Validate();
            bool l = Depth.Validate();
            bool m = OffshoreDistance.Validate();
            bool n = Equipment.Validate();

            IsValid = a && b && c && d && e && f && g && h && i && j && k && l && m && n;

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
            UserSpecifiedId.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a sample identifier" });
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a sample name." });

            Station.Validations.Add(new IsNotNullOrEmptyRule<Station> { ValidationMessage = "Must supply a station." });
        }
    }
}
