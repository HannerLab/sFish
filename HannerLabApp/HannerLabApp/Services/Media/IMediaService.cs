using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HannerLabApp.Services.Media
{
    /// <summary>
    /// Interface for handling platform specific media features such as saving to photo gallery.
    /// </summary>
    public interface IMediaService
    {
        Task SaveImageFileResultToGalleryAsync(FileResult file);
        Task<byte[]> GenerateImageThumbnailAsync(FileResult file);
    }
}
