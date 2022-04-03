using System;
using System.IO;
using System.Threading.Tasks;
using HannerLabApp.Configuration;
using HannerLabApp.Extensions;

namespace HannerLabApp.Services.Media
{
    public class PhotoStore : IPhotoStore
    {
        private static string GetPhotoFullPath(Guid id)
        {
            var fileName = $"{id}.jpg";
            var fullSavePath = Path.Combine(Constants.MediaDirectory, fileName);

            return fullSavePath;
        }

        public async Task<bool> SavePhotoAsync(Guid id, string file64)
        {
            var fullSavePath = GetPhotoFullPath(id);

            if (string.IsNullOrEmpty(file64)) return false;
            if (File.Exists(fullSavePath)) return false;

            var bytes = Convert.FromBase64String(file64);

            using (var stream = new MemoryStream(bytes))
            {
                using (var newStream = File.OpenWrite(fullSavePath))
                {
                    await stream.CopyToAsync(newStream);

                    return true;
                }
            }
        }

        public async Task<string> LoadPhotoAsync(Guid id)
        {
            var fullSavePath = GetPhotoFullPath(id);

            using (FileStream stream = File.Open(fullSavePath, FileMode.Open))
            {
                byte[] result = new byte[stream.Length];
                await stream.ReadAsync(result, 0, (int)stream.Length);

                return Convert.ToBase64String(result);
            }
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
