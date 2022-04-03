using System;
using System.IO;
using System.Threading.Tasks;
using HannerLabApp.Configuration;
using Xamarin.Essentials;

namespace HannerLabApp.Services.Media
{
    public class FileShare : IFileShare
    {
        public async Task ShareFile64Async(string file64)
        {
            // Can only share from file in file system. Save file64 to temp dir
            var saveFullPath = Path.Combine(Constants.TempDirectory, Guid.NewGuid().ToString());

            using (var fs = new FileStream(saveFullPath, FileMode.Create, FileAccess.Write))
            {
                var bytes = Convert.FromBase64String(file64);
                await fs.WriteAsync(bytes, 0, bytes.Length);
            }

            var title = "SHARE TITLE DEBUG TEXT";
            var file = new ShareFile(saveFullPath);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = title,
                File = file
            });
        }

        public async Task ShareFileAsync(string filePath)
        {
            var title = "SHARE TITLE DEBUG TEXT";
            var file = new ShareFile(filePath);

            await Share.RequestAsync(new ShareFileRequest
            {
                Title = title,
                File = file
            });
        }
    }
}
