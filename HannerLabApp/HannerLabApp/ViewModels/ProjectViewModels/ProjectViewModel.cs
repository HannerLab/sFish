using System;
using HannerLabApp.Models;
using HannerLabApp.Utils;
using HannerLabApp.Validators;
using HannerLabApp.Validators.Rules;
using TinyMvvm;

namespace HannerLabApp.ViewModels.ProjectViewModels
{
    /// <summary>
    /// A viewmodel representing the actual (view) data of a project
    /// </summary>
    public class ProjectViewModel : ViewModelBase, IValidableViewModel<Project>
    {
        private bool _isAdvancedShown;
        public bool IsAdvancedShown
        {
            get => _isAdvancedShown;
            set => Set(ref _isAdvancedShown, value);
        }
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }

        /// <summary>
        /// The text that is displayed within the details page. I.e add new 'project'
        /// </summary>
        public string TitleBase { get; private set; } = "Project";

        public Project Model
        {
            get => new Project
            {
                Id = Id,
                IsAdvancedShown = this.IsAdvancedShown,
                ProjectId = ProjectId,
                RecordedBy = this.RecordedBy.Value,
                Description = this.Description.Value,
                ContactEmail = this.ContactEmail.Value,
                Institution = this.Institution.Value,
                Notes = this.Notes.Value,
                Timestamp = this.Timestamp.Value,
                UserSpecifiedId = this.UserSpecifiedId.Value,
                Owner = this.Owner.Value,
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
                this.ContactEmail.Value = value.ContactEmail;
                this.RecordedBy.Value = value.RecordedBy;
                this.Institution.Value = value.Institution;
                this.UserSpecifiedId.Value = value.UserSpecifiedId;
                this.Owner.Value = value.Owner;
                this.Timestamp.Value = value.Timestamp;
            }
        }

        public bool IsValid { get; private set; }

        
        public ValidatableObject<string> Name { get; set; } = 
            new ValidatableObject<string>{ Title = "Name", Description = "A human readable name given to the project." };
        
        public ValidatableObject<string> Description { get; set; } = 
            new ValidatableObject<string> { Title = "Description", Description = "A general description of the project." };

        public ValidatableObject<string> Owner { get; set; } =
            new ValidatableObject<string> { Title = "Owner", Description = "The name of person who is responsible for the project." };

        public ValidatableObject<string> ContactEmail { get; set; } = 
            new ValidatableObject<string> { Title = "Primary contact", Description = "An E-mail address for the project's owner." };
        
        public ValidatableObject<string> RecordedBy { get; set; } = 
            new ValidatableObject<string> { Title = "Recorders", Description = "The name(s) of the default data recorders. Only applies when setting the project and can be overwritten from the main side bar menu at any time." };
        
        public ValidatableObject<string> Institution { get; set; } = 
            new ValidatableObject<string> { Title = "Institution", Description = "The institution which the project belongs to." };
        
        public ValidatableObject<string> Notes { get; set; } = 
            new ValidatableObject<string> { Title = "Notes", Description = "Enter any notes related to this project." };

        public ValidatableObject<string> UserSpecifiedId { get; set; } =
            new ValidatableObject<string> { Title = "Identifier (ID)", Description = "An identification string for your project. Should be unique to this project, however is not verified to be unique. May not contains spaces." };
        
        public ValidatableObject<DateTime> Timestamp { get; set; } = 
            new ValidatableObject<DateTime> { Title = "Creation Date", Description = "The date in which the project was created or when it started." };


        public ProjectViewModel()
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
            bool a = Name.Validate();
            bool b = Description.Validate();
            bool c = ContactEmail.Validate();
            bool d = RecordedBy.Validate();
            bool e = Institution.Validate();
            bool f = Notes.Validate();
            bool g = Owner.Validate();
            bool h = UserSpecifiedId.Validate();
            bool i = Timestamp.Validate();

            IsValid = a && b && c && d && e && f && g && h && i;

            return IsValid;
        }

        private void AddDefaults()
        {
            if (this.Timestamp.Value == DateTime.MinValue)
                this.Timestamp.Value = DateTime.Now;

            if (string.IsNullOrEmpty(this.UserSpecifiedId.Value))
                this.UserSpecifiedId.Value = IdGenerator.GetNewRandomId();

            if (this.Id == Guid.Empty)
                this.IsAdvancedShown = App.AppSettings.IsAdvanceModeDefaultEnabled;
        }

        private void AddValidationRules()
        {
            Name.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a project name." });
            UserSpecifiedId.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a project identifier." });

            //Description.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a general description of the project." });
            //Owner.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply the name of the project owner." });
            //RecordedBy.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply the names of the projects data recorders. This can still be set daily or per event." });
            //Institution.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a institution that the project belongs to." });
            //ContactEmail.Validations.Add(new IsNotNullOrEmptyRule<string> { ValidationMessage = "Must supply a primary contact E-mail." });
            //ContactEmail.Validations.Add(new IsValidEmailRule<string> { ValidationMessage = "Not a valid E-mail address." });
        }
    }
}
