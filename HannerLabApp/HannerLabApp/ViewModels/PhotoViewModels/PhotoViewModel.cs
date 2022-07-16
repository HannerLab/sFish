using System;
using System.Threading.Tasks;
using HannerLabApp.Models;
using HannerLabApp.Services.Media;
using HannerLabApp.Validators;
using HannerLabApp.Validators.Rules;
using HannerLabApp.ViewModels.EdnaViewModels;
using HannerLabApp.ViewModels.ObservationViewModels;
using HannerLabApp.ViewModels.ReadingViewModels;
using HannerLabApp.ViewModels.SiteViewModels;
using HannerLabApp.ViewModels.StationViewModels;
using TinyMvvm;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HannerLabApp.ViewModels.PhotoViewModels
{
    public class PhotoViewModel : ViewModelBase, IValidableViewModel<Photo>
    {
        private bool _isAdvancedShown;
        private string _thumbnail = string.Empty;

        public bool IsAdvancedShown
        {
            get => _isAdvancedShown;
            set => Set(ref _isAdvancedShown, value);
        }
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public static readonly string TitleBaseStatic = "Photo";
        public string TitleBase => TitleBaseStatic;

        /// <summary>
        /// What the photo is of, could be tagged to either a site, station, observation, or reading
        /// </summary>
        public string PhotoType
        {
            get
            {
                if (this.Site.Value != null) return $"{SiteViewModel.TitleBaseStatic} photo";
                if (this.Station.Value != null) return $"{StationViewModel.TitleBaseStatic} photo";
                if (this.Observation.Value != null) return $"{ObservationViewModel.TitleBaseStatic} photo";
                if (this.Edna.Value != null) return $"{EdnaViewModel.TitleBaseStatic} photo";
                if (this.Reading.Value != null) return $"{ReadingViewModel.TitleBaseStatic} photo";
                
                return string.Empty;
            }
        }

        public Photo Model
        {
            get => new Photo()
            {
                Id = Id,
                IsAdvancedShown = this.IsAdvancedShown,
                ProjectId = this.ProjectId,
                Notes = this.Notes.Value,
                Timestamp = this.Timestamp.Value,
                Station = this.Station.Value,
                Site = this.Site.Value,
                RecordedBy = this.RecordedBy.Value,
                Edna = this.Edna.Value,
                Observation = this.Observation.Value,
                Reading = this.Reading.Value,
                File = this.File.Value,
                Thumbnail = this.Thumbnail
            };
            set
            {
                this.Id = value.Id;
                this.IsAdvancedShown = value.IsAdvancedShown;
                this.ProjectId = value.ProjectId;
                this.Notes.Value = value.Notes;
                this.Timestamp.Value = value.Timestamp;
                this.Station.Value = value.Station;
                this.Site.Value = value.Site;
                this.Edna.Value = value.Edna;
                this.Observation.Value = value.Observation;
                this.Reading.Value = value.Reading;
                this.File.Value = value.File;
                this.Thumbnail = value.Thumbnail;
            }
        }

        public bool IsValid { get; private set; }

        public string Thumbnail
        { 
            get => _thumbnail;
            set => Set(ref _thumbnail, value);
        }

        public ValidatableObject<FileResult> File { get; set; } =
            new ValidatableObject<FileResult>();

        public ValidatableObject<string> Notes { get; set; } =
            new ValidatableObject<string> { Title = "Notes", Description = "Enter any notes related to this photo." };

        public ValidatableObject<string> RecordedBy { get; set; } =
            new ValidatableObject<string> { Title = "Recorded by", Description = "The name of the person who took this photo. Defaults to the current recorder set in the main menu." };

        public ValidatableObject<DateTime> Timestamp { get; set; } =
            new ValidatableObject<DateTime> { Title = "Timestamp", Description = "The date and time of when the photo was taken." };

        public ValidatableObject<Station> Station { get; set; } =
            new ValidatableObject<Station> { Title = "Station", Description = "Tag the photo to a specific station." };

        public ValidatableObject<Site> Site { get; set; } =
            new ValidatableObject<Site> { Title = "Site", Description = "Tag the photo to a specific site." };

        public ValidatableObject<Edna> Edna { get; set; } =
            new ValidatableObject<Edna> { Title = "e-DNA Sample", Description = "Tag the photo to a specific e-DNA sample." };

        public ValidatableObject<Observation> Observation { get; set; } =
            new ValidatableObject<Observation> { Title = "Observation", Description = "Tag the photo to a specific instrumental observation." };

        public ValidatableObject<Reading> Reading { get; set; } =
            new ValidatableObject<Reading> { Title = "Instrumental Reading", Description = "Tag the photo to a specific instrumental reading." };

        public PhotoViewModel()
        {
            AddValidationRules();
            AddDefaults();
        }

        public async Task LoadFileImageThumbnailAsync()
        {
            if (this.File.Value != null)
            {
                var platformSpecificMediaService = DependencyService.Get<IMediaService>();
                var thumbnail = await platformSpecificMediaService.GenerateImageThumbnailAsync(this.File.Value);
                this.Thumbnail = Convert.ToBase64String(thumbnail);
            }
        }

        public bool Validate()
        {
            bool a = Timestamp.Validate();
            bool b = Station.Validate();
            bool c = Site.Validate();
            bool d = Edna.Validate();
            bool e = Observation.Validate();
            bool f = File.Validate();
            bool g = Reading.Validate();
            bool h = RecordedBy.Validate();
            bool i = Notes.Validate();

            IsValid = a && b && c && d && e && f && g && h && i;

            return IsValid;
        }

        private void AddDefaults()
        {
            if (this.Timestamp.Value == DateTime.MinValue)
                this.Timestamp.Value = DateTime.Now;

            if (string.IsNullOrEmpty(this.RecordedBy.Value))
                this.RecordedBy.Value = App.AppSettings.CurrentRecorder;

            if (this.Id == Guid.Empty)
                this.IsAdvancedShown = App.AppSettings.IsAdvanceModeDefaultEnabled;
        }

        private void AddValidationRules()
        {
            File.Validations.Add(new IsFileResultContentNotNullOrEmpty { ValidationMessage = "Must include a photo!" });
        }
    }
}
