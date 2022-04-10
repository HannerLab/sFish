using System;
using HannerLabApp.Models;
using HannerLabApp.Utils;
using HannerLabApp.Validators;
using HannerLabApp.Validators.Rules;
using TinyMvvm;

namespace HannerLabApp.ViewModels.SiteViewModels
{
    public class SiteViewModel : ViewModelBase, IValidableViewModel<Site>
    {
        private bool _isAdvancedShown;
        public bool IsAdvancedShown
        {
            get => _isAdvancedShown;
            set => Set(ref _isAdvancedShown, value);
        }
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public static readonly string TitleBaseStatic = "Site";
        public string TitleBase => TitleBaseStatic;

        public Site Model
        {
            get => new Site
            {
                Id = Id,
                IsAdvancedShown = this.IsAdvancedShown,
                ProjectId = this.ProjectId,
                Description = this.Description.Value,
                Notes = this.Notes.Value,
                UserSpecifiedId = this.UserSpecifiedId.Value,
                Country = this.Country.Value,
                StateProvince = this.StateProvince.Value,
                Region = this.Region.Value,
                Locality = this.Locality.Value,
                WaterBody = this.WaterBody.Value,
                Hydrology = this.Hydrology.Value,
                Geology = this.Geology.Value,
                Name = this.Name.Value,
                RecordedBy = this.RecordedBy.Value,
                Timestamp = this.Timestamp.Value
            };
            set
            {
                this.Id = value.Id;
                this.IsAdvancedShown = value.IsAdvancedShown;
                this.ProjectId = value.ProjectId;
                this.Name.Value = value.Name;
                this.Description.Value = value.Description;
                this.Notes.Value = value.Notes;
                this.UserSpecifiedId.Value = value.UserSpecifiedId;
                this.Country.Value = value.Country;
                this.StateProvince.Value = value.StateProvince;
                this.Region.Value = value.Region;
                this.Locality.Value = value.Locality;
                this.WaterBody.Value = value.WaterBody;
                this.Hydrology.Value = value.Hydrology;
                this.Geology.Value = value.Geology;
                this.RecordedBy.Value = value.RecordedBy;
                this.Timestamp.Value = value.Timestamp;
            }
        }

        public bool IsValid { get; set; }

        public ValidatableObject<string> UserSpecifiedId { get; set; } = 
            new ValidatableObject<string>{ Title = "Identifier (ID)", Description = "An identification string for your site." };
        public ValidatableObject<DateTime> Timestamp { get; set; } =
            new ValidatableObject<DateTime> { Title = "Timestamp", Description = "The date and time of when this site was added." };

        public ValidatableObject<string> Name { get; set; } =
            new ValidatableObject<string> { Title = "Name", Description = "The name of the site." };

        public ValidatableObject<string> Description { get; set; } =
            new ValidatableObject<string> { Title = "Description", Description = "A general description of the site." };

        public ValidatableObject<string> Notes { get; set; } =
            new ValidatableObject<string> { Title = "Notes", Description = "Any notes related to the site can be added here." };

        public ValidatableObject<string> Country { get; set; } =
            new ValidatableObject<string> { Title = "Containing Country", Description = "The country where this site resides in." };
        public ValidatableObject<string> StateProvince { get; set; } =
            new ValidatableObject<string> { Title = "Containing State/Province", Description = "The state/province where this site resides in." };

        public ValidatableObject<string> Region { get; set; } =
            new ValidatableObject<string> { Title = "Containing Region", Description = "The region where this site resides in." };

        public ValidatableObject<string> Locality { get; set; } =
            new ValidatableObject<string> { Title = "Containing Locality", Description = "The locality where this site resides in." };

        public ValidatableObject<string> WaterBody { get; set; } =
            new ValidatableObject<string> { Title = "Waterbody Type", Description = "The type of water body mainly contained within this site." };

        public ValidatableObject<string> Hydrology { get; set; } =
            new ValidatableObject<string> { Title = "Hydrology", Description = "A general description of the hydrology of this site." };

        public ValidatableObject<string> Geology { get; set; } =
            new ValidatableObject<string> { Title = "Geology", Description = "A general description of the geology of this site." };

        public ValidatableObject<string> RecordedBy { get; set; } =
            new ValidatableObject<string> { Title = "Recorded By", Description = "The name of the person who recorded this site." };

        public SiteViewModel()
        {
            AddValidationRules();
            AddDefaults();
        }


        public bool Validate()
        {
            bool a = UserSpecifiedId.Validate();
            bool b = Timestamp.Validate();
            bool c = Name.Validate();
            bool d = Description.Validate();
            bool e = Notes.Validate();
            bool f = Country.Validate();
            bool g = StateProvince.Validate();
            bool h = Region.Validate();
            bool i = Locality.Validate();
            bool j = WaterBody.Validate();
            bool k = Hydrology.Validate();
            bool l = Geology.Validate();
            bool m = RecordedBy.Validate();

            IsValid = a && b && c && d && e && f && g && h && i && j && k && l && m;

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
        }

        private void AddValidationRules()
        {
            UserSpecifiedId.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a site identifier" });
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a site name." });
        }
    }
}
