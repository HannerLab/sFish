namespace HannerLabApp.Validators
{
    /// <summary>
    /// An interface for a validation rule for a validatable object.
    ///
    /// For more information see: https://docs.microsoft.com/en-us/xamarin/xamarin-forms/enterprise-application-patterns/validation
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValidationRule<T>
    {
        string ValidationMessage { get; set; }
        bool Check(T value);
    }
}
