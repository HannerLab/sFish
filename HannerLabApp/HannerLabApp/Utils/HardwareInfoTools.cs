using Autofac;
using HannerLabApp.Services;

namespace HannerLabApp.Utils
{
    /// <summary>
    /// Interfacing with hardware info
    /// </summary>
    public static class HardwareInfoTools
    {
        /// <summary>
        /// Returns the unique device identifier for the device
        /// </summary>
        /// <returns></returns>
        public static string GetDeviceId()
        {
            return App.AppContainer.Resolve<IDeviceIdentifier>().GetIdentifier();
        }
    }
}
