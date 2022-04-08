using System;
using HannerLabApp.Models;
using HannerLabApp.Utils;
using HannerLabApp.Validators;
using HannerLabApp.Validators.Rules;
using TinyMvvm;

namespace HannerLabApp.ViewModels.ObservationViewModels
{
    public class ObservationViewModel : ViewModelBase, IValidableViewModel<Observation>
    {
        private bool _isAdvancedShown;
        public bool IsAdvancedShown
        {
            get => _isAdvancedShown;
            set => Set(ref _isAdvancedShown, value);
        }
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public static string TitleBaseStatic = "Observation";
        public string TitleBase => TitleBaseStatic;

        public Observation Model
        {
            get => new Observation()
            {
                Id = Id,
                IsAdvancedShown = this.IsAdvancedShown,
                UserSpecifiedId = this.UserSpecifiedId.Value,
                ProjectId = this.ProjectId,
                Notes = this.Notes.Value,
                Timestamp = this.Timestamp.Value,
                Station = this.Station.Value,
                Temperature = this.Temperature.Value,
                PrecipitationLevel = this.PrecipitationLevel.Value,
                CloudCover = this.CloudCover.Value,
                CloudCoverLevel = this.CloudCoverLevel.Value,
                StormYesterday = this.StormYesterday.Value,
                WindLevel = this.WindLevel.Value,
                ObservedBy = this.ObservedBy.Value,
                Phenology = this.Phenology.Value,
                Wildlife = this.Wildlife.Value,
                Anthropogenic = this.Anthropogenic.Value,
                Site = this.Site.Value,
                RecordedBy = this.RecordedBy.Value,
                Name = this.Name.Value
            };
            set
            {
                this.Id = value.Id;
                this.IsAdvancedShown = value.IsAdvancedShown;
                this.UserSpecifiedId.Value = value.UserSpecifiedId;
                this.ProjectId = value.ProjectId;
                this.Notes.Value = value.Notes;
                this.Timestamp.Value = value.Timestamp;
                this.Station.Value = value.Station;
                this.Temperature.Value = value.Temperature;
                this.CloudCover.Value = value.CloudCover;
                this.CloudCoverLevel.Value = value.CloudCoverLevel;
                this.PrecipitationLevel.Value = value.PrecipitationLevel;
                this.StormYesterday.Value = value.StormYesterday;
                this.WindLevel.Value = value.WindLevel;
                this.Phenology.Value = value.Phenology;
                this.Wildlife.Value = value.Wildlife;
                this.Anthropogenic.Value = value.Anthropogenic;
                this.Site.Value = value.Site;
                this.Name.Value = value.Name;
                this.RecordedBy.Value = value.RecordedBy;
                this.ObservedBy.Value = value.ObservedBy;
            }
        }

        public bool IsValid { get; private set; }


        public ValidatableObject<string> UserSpecifiedId { get; set; } =
            new ValidatableObject<string> { Title = "Identifier (ID)", Description = "An identification string for this observation." };

        public ValidatableObject<string> ObservedBy { get; set; } =
            new ValidatableObject<string> { Title = "Observed by", Description = "The name of the person who made this observation." };

        public ValidatableObject<string> RecordedBy { get; set; } =
            new ValidatableObject<string> { Title = "Recorded by", Description = "The name of the person who recorded this observation." };

        public ValidatableObject<string> Notes { get; set; } =
            new ValidatableObject<string> { Title = "Notes", Description = "Enter any notes related to this observation." };

        public ValidatableObject<string> Name { get; set; } =
            new ValidatableObject<string> { Title = "Name", Description = "Provide a name for this observation." };

        public ValidatableObject<DateTime> Timestamp { get; set; } =
            new ValidatableObject<DateTime> { Title = "Timestamp", Description = "The date and time of when the observation was made." };

        public ValidatableObject<Station> Station { get; set; } =
            new ValidatableObject<Station> { Title = "Station", Description = "The station where this was observed from." };

        public ValidatableObject<Site> Site { get; set; } =
            new ValidatableObject<Site> { Title = "Site", Description = "The site where this was observed from." };

        public ValidatableObject<double?> Temperature { get; set; } =
            new ValidatableObject<double?> { Title = "Temperature", Description = "The air temperature (°C)." };

        public ValidatableObject<PrecipitationLevel> PrecipitationLevel { get; set; } =
            new ValidatableObject<PrecipitationLevel> { Title = "Precipitation Level", Description = "The level of precipitation." };

        public ValidatableObject<CloudCoverLevel> CloudCoverLevel { get; set; } =
            new ValidatableObject<CloudCoverLevel> { Title = "Cloud Coverage", Description = "The amount of cloud coverage." };

        public ValidatableObject<StormYesterday> StormYesterday { get; set; } =
            new ValidatableObject<StormYesterday> { Title = "Storm Yesterday", Description = "Whether or not there was a storm at this location on the previous day." };

        public ValidatableObject<CloudCover> CloudCover { get; set; } =
            new ValidatableObject<CloudCover> { Title = "Cloud Cover", Description = "The type of cloud cover." };

        public ValidatableObject<WindLevel> WindLevel { get; set; } =
            new ValidatableObject<WindLevel> { Title = "Wind Level", Description = "The level of wind." };

        public ValidatableObject<string> Phenology { get; set; } =
            new ValidatableObject<string> { Title = "Phenology", Description = "A general description of the phenology observed." };

        public ValidatableObject<string> Wildlife { get; set; } =
            new ValidatableObject<string> { Title = "Wildlife", Description = "A general description of the wildlife observed." };

        public ValidatableObject<string> Anthropogenic { get; set; } =
            new ValidatableObject<string> { Title = "Anthropogenic", Description = "A general description of any anthropogenic observations." };

        public ObservationViewModel()
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
            bool a = UserSpecifiedId.Validate();
            bool b = ObservedBy.Validate();
            bool c = RecordedBy.Validate();
            bool d = Notes.Validate();
            bool e = Name.Validate();
            bool f = Timestamp.Validate();
            bool g = Station.Validate();
            bool h = Site.Validate();
            bool i = Temperature.Validate();
            bool j = PrecipitationLevel.Validate();
            bool k = CloudCoverLevel.Validate();
            bool l = StormYesterday.Validate();
            bool m = CloudCover.Validate();
            bool n = WindLevel.Validate();
            bool o = Phenology.Validate();
            bool p = Wildlife.Validate();
            bool q = Anthropogenic.Validate();

            IsValid = a && b && c && d && e && f && g && h && i && j && k && l && m && n && o && p && q;

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
            UserSpecifiedId.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply an observation identifier." });
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a observation name." });
        }
    }
}
