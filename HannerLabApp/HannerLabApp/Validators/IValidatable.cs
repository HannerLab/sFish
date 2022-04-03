using System.Collections.Generic;
using System.ComponentModel;

namespace HannerLabApp.Validators
{
    /// <summary>
    /// An interface for a validatable object, something that is either valid, or invalid, based on specific defined rules (IValidationRule)
    ///
    /// For more information see: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/validation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidatable<T> : INotifyPropertyChanged
    {
        List<IValidationRule<T>> Validations { get; }

        List<string> Errors { get; set; }

        bool Validate();

        bool IsValid { get; set; }

        string Description { get; set; }

        string Title { get; set; }

        T Value { get; set; }
    }
}
