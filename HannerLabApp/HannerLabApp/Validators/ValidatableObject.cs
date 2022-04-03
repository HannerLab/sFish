using System.Collections.Generic;
using System.Linq;
using TinyMvvm;

namespace HannerLabApp.Validators
{
    /// <summary>
    /// The main implementation of the validatable object. Contains INotifyPropertyChange for MVVM binding.
    ///
    /// For more information see: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/validation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ValidatableObject<T> : ViewModelBase, IValidatable<T>
    {
        
        /// <summary>
        /// The description of what this value is, usually displayed underneath
        /// </summary>
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }
        private string _description;

        /// <summary>
        /// The title for this entry validable object. Shown in place holder for this application
        /// </summary>
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }
        private string _title;

        /// <summary>
        /// A list of all the validation rules that will be used to validate against.
        /// </summary>
        public List<IValidationRule<T>> Validations { get; protected set; } = new List<IValidationRule<T>>();

        private List<string> _errors = new List<string>();

        /// <summary>
        /// A list of all validation error messages
        /// </summary>
        public List<string> Errors
        {
            get => _errors; 
            set => Set(ref _errors, value);
        }

        /// <summary>
        /// If true, sets IsValid to true whenever the value if changed.
        /// </summary>
        public bool CleanOnChange { get; set; } = true;

        private T _value;
        public T Value
        {
            get => _value;
            set
            {
                Set(ref this._value, value);

                if (CleanOnChange)
                    IsValid = true;
            }
        }

        private bool _isValid = true;

        public bool IsValid
        {
            get => _isValid; 
            set => Set(ref _isValid, value);
        }

        public virtual bool Validate()
        {
            Errors.Clear();

            IEnumerable<string> errors = Validations.Where(v => !v.Check(Value))
                .Select(v => v.ValidationMessage);

            Errors = errors.ToList();
            IsValid = !Errors.Any();

            return this.IsValid;
        }
        public override string ToString()
        {
            return $"{Value}";
        }
    }
}