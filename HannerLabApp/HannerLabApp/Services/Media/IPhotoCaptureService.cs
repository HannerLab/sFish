using System.Threading.Tasks;

namespace HannerLabApp.Services.Media
{
    public interface IPhotoCaptureService
    {
        Task<string> CapturePhotoAsync(bool saveToGallery);
        Task<string> PickPhotoAsync();
    }
}
