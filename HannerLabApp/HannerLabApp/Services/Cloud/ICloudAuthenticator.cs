using System.Threading.Tasks;

namespace HannerLabApp.Services.Cloud
{
    public interface ICloudAuthenticator
    {
        Task<string> Authenticate();
    }
}
