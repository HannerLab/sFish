using System;
using System.Globalization;
using HannerLabApp.Extensions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Converters
{
    public class EnumToDescriptionConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var desc = value as Enum;
                return desc.GetDescription();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error! Attempting to convert a non enum value.", ex);
                return string.Empty;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
