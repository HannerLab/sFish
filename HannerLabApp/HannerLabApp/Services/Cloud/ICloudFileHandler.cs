using System.Threading.Tasks;

namespace HannerLabApp.Services.Cloud
{
    public interface ICloudFileHandler
    {
        string AccessToken { get; set; }
        Task<byte[]> ReadFileAsync(string filePath);
        Task<bool> WriteFileAsync(byte[] fileContent, string remoteFileName, string remoteFilePath);
        Task<string> GetAccountAsync();
    }
}
