using HannerLabApp.iOS.Services;
using HannerLabApp.Services;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(DeviceIdentifier))]
namespace HannerLabApp.iOS.Services
{
    public class DeviceIdentifier : IDeviceIdentifier
    {
        public string GetIdentifier()
        {
            return UIDevice.CurrentDevice.IdentifierForVendor.ToString();
        }
    }
}