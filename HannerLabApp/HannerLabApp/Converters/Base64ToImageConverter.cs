using System;
using System.Globalization;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Converters
{
    public class Base64ToImageConverter : IValueConverter, IMarkupExtension
    {
        private ImageSource _image;
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string)
            {
                _image = null;
                byte[] bytes = System.Convert.FromBase64String(value.ToString());
                _image = ImageSource.FromStream(() => new MemoryStream(bytes));
                return _image;
            }
            return null;
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
