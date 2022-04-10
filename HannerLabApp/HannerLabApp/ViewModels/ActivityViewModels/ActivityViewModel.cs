using System;
using HannerLabApp.Models;
using HannerLabApp.Utils;
using HannerLabApp.Validators;
using HannerLabApp.Validators.Rules;
using TinyMvvm;

namespace HannerLabApp.ViewModels.ActivityViewModels
{
    public class ActivityViewModel : ViewModelBase, IValidableViewModel<Activity>
    {
        private bool _isAdvancedShown;
        public bool IsAdvancedShown
        {
            get => _isAdvancedShown;
            set => Set(ref _isAdvancedShown, value);
        }

        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        public static readonly string TitleBaseStatic = "Collecting Activity";
        public string TitleBase => TitleBaseStatic;

        public Activity Model
        {
            get => new Activity
            {
                Id = Id,
                IsAdvancedShown = this.IsAdvancedShown,
                ProjectId = this.ProjectId,
                Notes = this.Notes.Value,
                Name = this.Name.Value,
                UserSpecifiedId = this.UserSpecifiedId.Value,
                Timestamp = this.Timestamp.Value,
                RecordedBy = this.RecordedBy.Value,
                TimestampStart = this.TimestampStart.Value.GetValueOrDefault(),
                TimestampEnd = this.TimestampEnd.Value.GetValueOrDefault(),
                Organization = this.Organization.Value,
                LeadMember = this.LeadMember.Value,
                OtherMembers = this.OtherMembers.Value,
                Description = this.Description.Value
            };
            set
            {
                this.Id = value.Id;
                this.Name.Value = value.Name;
                this.IsAdvancedShown = value.IsAdvancedShown;
                this.ProjectId = value.ProjectId;
                this.Notes.Value = value.Notes;
                this.UserSpecifiedId.Value = value.UserSpecifiedId;
                this.Timestamp.Value = value.Timestamp;
                this.RecordedBy.Value = value.RecordedBy;
                this.TimestampEnd.Value = value.TimestampEnd;
                this.TimestampStart.Value = value.TimestampStart;
                this.Organization.Value = value.Organization;
                this.LeadMember.Value = value.LeadMember;
                this.OtherMembers.Value = value.OtherMembers;
                this.Description.Value = value.Description;
            }
        }

        public bool IsValid { get; private set; }

        public ValidatableObject<string> Notes { get; set; } =
            new ValidatableObject<string> { Title = "Notes", Description = "Any notes related to the activity." };

        public ValidatableObject<string> OtherMembers { get; set; } =
            new ValidatableObject<string> { Title = "Other Members", Description = "Any other people involved in the sampling activity." };

        public ValidatableObject<string> LeadMember { get; set; } =
            new ValidatableObject<string> { Title = "Lead Member", Description = "The lead member of the activity." };

        public ValidatableObject<string> Organization { get; set; } =
            new ValidatableObject<string> { Title = "Organization", Description = "The organization responsible for the activity" };

        public ValidatableObject<string> Name { get; set; } =
            new ValidatableObject<string> { Title = "Name", Description = "The name of the activity." };

        public ValidatableObject<string> Description { get; set; } =
            new ValidatableObject<string> { Title = "Description", Description = "A general description of the activity." };

        public ValidatableObject<string> RecordedBy { get; set; } =
            new ValidatableObject<string> { Title = "Recorded By", Description = "The name of the person who exported this activity." };

        public ValidatableObject<DateTime> Timestamp { get; set; } =
            new ValidatableObject<DateTime> { Title = "Timestamp", Description = "The date and time the activity was exported at. Only samples or observations taken between this time will be exported." };

        public ValidatableObject<DateTime?> TimestampStart { get; set; } =
            new ValidatableObject<DateTime?> { Title = "Start Time", Description = "The date and time of when the activity actually started. Only samples or observations taken between this time will be exported." };

        public ValidatableObject<DateTime?> TimestampEnd { get; set; } =
            new ValidatableObject<DateTime?> { Title = "End Time", Description = "The date and time of when the activity actually ended." };

        public ValidatableObject<string> UserSpecifiedId { get; set; } =
            new ValidatableObject<string> { Title = "Identifier (ID)", Description = "An identification string for this activity." };

        public ActivityViewModel()
        {
            AddValidationRules();
            AddDefaults();
        }

        public bool Validate()
        {
            bool a = Notes.Validate();
            bool b = OtherMembers.Validate();
            bool c = LeadMember.Validate();
            bool d = Organization.Validate();
            bool e = Name.Validate();
            bool f = Description.Validate();
            bool g = RecordedBy.Validate();
            bool h = Timestamp.Validate();
            bool i = TimestampStart.Validate();
            bool j = TimestampEnd.Validate();
            bool k = UserSpecifiedId.Validate();

            IsValid = a && b && c && d && e && f && g && h && i && j && k;

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
            UserSpecifiedId.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply an activity identifier." });
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply an activity name." });

            TimestampStart.Validations.Add(new IsNotNullOrEmptyRule<DateTime?> { ValidationMessage = "Must supply an activity start date and time." });
            TimestampEnd.Validations.Add(new IsNotNullOrEmptyRule<DateTime?> { ValidationMessage = "Must supply an activity end date and time." });
        }
    }
}
