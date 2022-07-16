using System;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HannerLabApp.Services.Media
{
    public interface IPhotoStore
    {
        Task<bool> SavePhotoAsync(Guid id, FileResult file);
        Task<FileResult> LoadPhotoAsync(Guid id);
        Task<bool> DeletePhotoAsync(Guid id);
    }
}
