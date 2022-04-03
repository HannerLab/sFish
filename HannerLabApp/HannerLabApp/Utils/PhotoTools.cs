using System;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace HannerLabApp.Utils
{
    public static class PhotoTools
    {
        /// <summary>
        /// Converts the photo FileResult object stream to a base64 string. 
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static async Task<string> GetPhotoAsString64Async(FileResult file)
        {
            using (var stream = await file.OpenReadAsync())
            {
                byte[] bytes;

                // The file is buffered entirely in memory. Not suitable for large files. Good enough for pictures.
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    bytes = memoryStream.ToArray();
                }

                return Convert.ToBase64String(bytes);
            }
        }
    }
}
