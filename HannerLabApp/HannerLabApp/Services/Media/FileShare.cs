using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HannerLabApp.Services.Media
{
    public class FileShare : IFileShare
    {
        public async Task ShareFileAsync(string filePath)
        {
            var title = "Send Exported Data";
            var file = new ShareFile(filePath);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = title,
                File = file
            });
        }
    }
}
