using System;
using System.Threading.Tasks;

namespace HannerLabApp.Services.Media
{
    public interface IPhotoStore
    {
        Task<bool> SavePhotoAsync(Guid id, string file64);
        Task<string> LoadPhotoAsync(Guid id);
        Task<bool> DeletePhotoAsync(Guid id);
    }
}
