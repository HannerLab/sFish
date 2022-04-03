using System;
using HannerLabApp.Models;
using HannerLabApp.Utils;
using HannerLabApp.Validators;
using HannerLabApp.Validators.Rules;
using TinyMvvm;

namespace HannerLabApp.ViewModels.StationViewModels
{
    public class StationViewModel : ViewModelBase, IValidableViewModel<Station>
    {
        private bool _isAdvancedShown;
        public bool IsAdvancedShown
        {
            get => _isAdvancedShown;
            set => Set(ref _isAdvancedShown, value);
        }
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public static string TitleBaseStatic = "Station";
        public string TitleBase { get; private set; } = TitleBaseStatic;

        public Station Model
        {
            get =>
                new Station()
                {
                    Id = Id,
                    IsAdvancedShown = this.IsAdvancedShown,
                    ProjectId = ProjectId,
                    Description = this.Description.Value,
                    Notes = this.Notes.Value,
                    UserSpecifiedId = this.UserSpecifiedId.Value,
                    Site = this.Site.Value,
                    WayPoint = this.WayPoint.Value,
                    Latitude = this.Latitude.Value,
                    Longitude = this.Longitude.Value,
                    Elevation = this.Elevation.Value,
                    Habitat = this.Habitat.Value,
                    WaterBody = this.WaterBody.Value,
                    FloodPlain = this.FloodPlain.Value,
                    Substrate = this.Substrate.Value,
                    Geology = this.Geology.Value,
                    Hydrology = this.Hydrology.Value,
                    Stratification = this.Stratification.Value,
                    VegetationAquatic = this.VegetationAquatic.Value,
                    VegetationTerrestrial = this.VegetationTerrestrial.Value,
                    Name = this.Name.Value
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
                this.Site.Value = value.Site;
                this.WayPoint.Value = value.WayPoint;
                this.Latitude.Value = value.Latitude;
                this.Longitude.Value = value.Longitude;
                this.Elevation.Value = value.Elevation;
                this.Habitat.Value = value.Habitat;
                this.WaterBody.Value = value.WaterBody;
                this.FloodPlain.Value = value.FloodPlain;
                this.Substrate.Value = value.Substrate;
                this.Geology.Value = value.Geology;
                this.Hydrology.Value = value.Hydrology;
                this.Stratification.Value = value.Stratification;
                this.VegetationAquatic.Value = value.VegetationAquatic;
                this.VegetationTerrestrial.Value = value.VegetationTerrestrial;
            }
        }

        public bool IsValid { get; set; }

        public ValidatableObject<string> Name { get; set; } = 
            new ValidatableObject<string>{ Title = "Name", Description = "The name of the station" };

        public ValidatableObject<string> Description { get; set; } =
            new ValidatableObject<string> { Title = "Description", Description = "A general description of the station" };

        public ValidatableObject<string> Notes { get; set; } =
            new ValidatableObject<string> { Title = "Notes", Description = "Any notes you may have related to the station." };

        public ValidatableObject<string> WayPoint { get; set; } =
            new ValidatableObject<string> { Title = "GPS Waypoint", Description = "The location of the station as a way point within an external GPS unit." };

        public ValidatableObject<string> UserSpecifiedId { get; set; } =
            new ValidatableObject<string> { Title = "Identifier (ID)", Description = "An identification string for your station." };

        public ValidatableObject<DateTime> Timestamp { get; set; } =
            new ValidatableObject<DateTime> { Title = "Timestamp", Description = "The date and time of when this station was added." };
        public ValidatableObject<double?> Longitude { get; set; } =
            new ValidatableObject<double?> { Title = "Longitude", Description = "The longitude of the station (decimal degrees)." };

        public ValidatableObject<double?> Latitude { get; set; } =
            new ValidatableObject<double?> { Title = "Latitude", Description = "The latitude of the station (decimal degrees)" };

        public ValidatableObject<double?> Elevation { get; set; } =
            new ValidatableObject<double?> { Title = "Elevation", Description = "The elevation of the station (m)." };

        public ValidatableObject<Site> Site { get; set; } =
            new ValidatableObject<Site> { Title = "Site", Description = "The site in which the station belongs to." };

        public ValidatableObject<string> Habitat { get; set; } =
            new ValidatableObject<string> { Title = "Habitat", Description = "The type of habitat(s) present in this station." };
        public ValidatableObject<string> FloodPlain { get; set; } =
            new ValidatableObject<string> { Title = "Flood plain", Description = "A general description of the flood plain for this station." };
        public ValidatableObject<string> Substrate { get; set; } =
            new ValidatableObject<string> { Title = "Substrate", Description = "The type of substrate(s) present in this station." };
        public ValidatableObject<string> WaterBody { get; set; } =
            new ValidatableObject<string> { Title = "Waterbody Type", Description = "The type of waterbody mainly contained within this station." };
        public ValidatableObject<string> Hydrology { get; set; } =
            new ValidatableObject<string> { Title = "Hydrology", Description = "A general description of the hydrology of this station." };
        public ValidatableObject<string> Geology { get; set; } =
            new ValidatableObject<string> { Title = "Geology", Description = "A general description of the geology of this station." };
        public ValidatableObject<string> Stratification { get; set; } =
            new ValidatableObject<string> { Title = "Stratification", Description = "A general description of the stratification of this station." };
        public ValidatableObject<string> VegetationAquatic { get; set; } =
            new ValidatableObject<string> { Title = "Aquatic Vegetation", Description = "A general description of the aquatic vegetation of this station." };
        public ValidatableObject<string> VegetationTerrestrial { get; set; } =
            new ValidatableObject<string> { Title = "Terrestrial Vegetation", Description = "A general description of the terrestrial vegetation of this station." };
        public ValidatableObject<string> RecordedBy { get; set; } =
            new ValidatableObject<string> { Title = "Recorded By", Description = "The name of the person who recorded this station." };

        public StationViewModel()
        {
            AddValidationRules();
            AddDefaults();
        }

        public bool Validate()
        {
            bool a = Name.Validate();
            bool b = Description.Validate();
            bool c = Notes.Validate();
            bool d = WayPoint.Validate();
            bool e = UserSpecifiedId.Validate();
            bool f = Timestamp.Validate();
            bool g = Longitude.Validate();
            bool h = Latitude.Validate();
            bool i = Elevation.Validate();
            bool j = Site.Validate();
            bool k = Habitat.Validate();
            bool l = FloodPlain.Validate();
            bool m = Substrate.Validate();
            bool n = WaterBody.Validate();
            bool o = Hydrology.Validate();
            bool p = Geology.Validate();
            bool q = Stratification.Validate();
            bool r = VegetationAquatic.Validate();
            bool s = VegetationTerrestrial.Validate();
            bool t = RecordedBy.Validate();

            IsValid = a && b && c && d && e && f && g && h && i && j && k && l && m && n && o && p && q && r && s && t;

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
            UserSpecifiedId.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a station identifier." });
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a station name." });

            Site.Validations.Add(new IsNotNullOrEmptyRule<Site> { ValidationMessage = "Must supply a site." });
        }
    }
}
