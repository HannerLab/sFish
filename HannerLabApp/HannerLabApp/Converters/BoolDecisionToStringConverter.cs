using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Converters
{
    /// <summary>
    /// https://stackoverflow.com/questions/23641688/changing-a-label-when-a-bool-variable-turns-true
    /// </summary>
    public class BoolDecisionToStringConverter : IValueConverter, IMarkupExtension
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                string[] args = ((string) parameter).Split(';');
                if ((bool) value)
                    return args[0];

                return args[1];
            }
            catch (IndexOutOfRangeException ex)
            {
                Console.WriteLine($"Failed to convert bool decision to string: {ex}");
                return parameter;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //We don't care about this one for this converter
            throw new NotImplementedException();
        }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
