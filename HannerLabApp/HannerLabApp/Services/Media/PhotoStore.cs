using System;
using System.IO;
using System.Threading.Tasks;
using HannerLabApp.Configuration;
using HannerLabApp.Extensions;
using Xamarin.Essentials;

namespace HannerLabApp.Services.Media
{
    public class PhotoStore : IPhotoStore
    {
        private static string GetPhotoFullPath(Guid id)
        {
            // Save file to disk without file extension, all in 1 folder.
            var fileName = $"{id}";
            var fullSavePath = Path.Combine(Constants.MediaDirectory, fileName);

            return fullSavePath;
        }

        public async Task<bool> SavePhotoAsync(Guid id, FileResult file)
        {
            if (file == null) return false;

            var fullSavePath = GetPhotoFullPath(id);

            if (File.Exists(fullSavePath)) return false;
            try
            {
                using (var stream = await file.OpenReadAsync())
                {
                    using (var fileStream = new FileStream(fullSavePath, FileMode.Create, FileAccess.Write))
                    {
                        await stream.CopyToAsync(fileStream);
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Could not save photo", ex);
                return false;
            }
        }

        public async Task<FileResult> LoadPhotoAsync(Guid id)
        {
            await Task.Delay(1);

            var fullSavePath = GetPhotoFullPath(id);

            var fileResult = new FileResult(fullSavePath);

            return fileResult;
        }

        public async Task<bool> DeletePhotoAsync(Guid id)
        {
            var fullSavePath = GetPhotoFullPath(id);

            if (!File.Exists(fullSavePath)) return false;

            FileInfo fi = new FileInfo(fullSavePath);
            await fi.DeleteAsync();

            return true;
        }
    }
}
