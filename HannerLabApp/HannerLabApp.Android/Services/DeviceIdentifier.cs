using HannerLabApp.Droid.Services;
using HannerLabApp.Services;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceIdentifier))]
namespace HannerLabApp.Droid.Services
{
    public class DeviceIdentifier : IDeviceIdentifier
    {
        public string GetIdentifier()
        {
             return Android.Provider.Settings.Secure.GetString(Android.App.Application.Context.ContentResolver, Android.Provider.Settings.Secure.AndroidId);
        }
    }
}