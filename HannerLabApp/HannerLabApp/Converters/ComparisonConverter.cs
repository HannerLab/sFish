using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Converters
{
    public class ComparisonConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return value?.Equals(true) == true ? parameter : Binding.DoNothing;
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
