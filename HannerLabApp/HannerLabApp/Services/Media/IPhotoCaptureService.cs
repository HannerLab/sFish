using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HannerLabApp.Services.Media
{
    public interface IPhotoCaptureService
    {
        Task<FileResult> CapturePhotoAsync(bool saveToGallery);
        Task<FileResult> PickPhotoAsync();
    }
}
