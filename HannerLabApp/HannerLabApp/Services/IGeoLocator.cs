using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HannerLabApp.Services
{
    public interface IGeoLocator
    {
        Task<Location> GetGpsLocationAsync();
    }
}
