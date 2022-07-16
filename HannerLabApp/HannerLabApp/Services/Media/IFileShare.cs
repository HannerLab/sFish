using System.Threading.Tasks;

namespace HannerLabApp.Services.Media
{
    /// <summary>
    /// 'Shares' a file using the native sharing dialog menu. Allows user to send this file via email, save to local dir, etc
    /// </summary>
    public interface IFileShare
    {
        Task ShareFileAsync(string filePath);
    }
}
