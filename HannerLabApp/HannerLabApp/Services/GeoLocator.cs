using System;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HannerLabApp.Services
{
    public class GeoLocator : IGeoLocator
    {
        private CancellationTokenSource _cts;

        public void TimeOut()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
                _cts.Cancel();
        }

        public async Task<Location> GetGpsLocationAsync()
        {
            Location location = null;

            try
            {
                var request = new GeolocationRequest(GeolocationAccuracy.Medium, TimeSpan.FromSeconds(10));
                _cts = new CancellationTokenSource();
                location = await Geolocation.GetLocationAsync(request, _cts.Token);

                if (location != null)
                {
                    Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
                }
            }
            catch (FeatureNotSupportedException ex)
            {
                // Handle not supported on device exception
                Console.WriteLine("Error obtaining GPS location", ex);
                return null;
            }
            catch (FeatureNotEnabledException ex)
            {
                // Handle not enabled on device exception
                Console.WriteLine("Error obtaining GPS location", ex);
                return null;
            }
            catch (PermissionException ex)
            {
                // Handle permission exception
                Console.WriteLine("Error obtaining GPS location", ex);
                return null;
            }
            catch (Exception ex)
            {
                // Unable to get location
                Console.WriteLine("Error obtaining GPS location", ex);
                return null;
            }

            return location;
        }
    }
}
