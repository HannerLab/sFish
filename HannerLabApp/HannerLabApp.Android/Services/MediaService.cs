using Android.Content;
using HannerLabApp.Droid.Services;
using HannerLabApp.Services.Media;
using Plugin.CurrentActivity;
using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using System.IO;
using Android.Graphics;
using HannerLabApp.Extensions;
using Path = System.IO.Path;

[assembly: Xamarin.Forms.Dependency(typeof(MediaService))]
namespace HannerLabApp.Droid.Services
{
    public class MediaService : IMediaService
    {
        Context CurrentContext => CrossCurrentActivity.Current.Activity;

        public async Task SaveImageFileResultToGalleryAsync(FileResult file)
        {
            try
            {
                var fileName = file.FileName;

                var storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
                string path = Path.Combine(storagePath, fileName);

                using (var stream = await file.OpenReadAsync())
                {
                    using (var fileStream = new FileStream(path, FileMode.Create, FileAccess.Write))
                    {
                        stream.CopyTo(fileStream);
                    }
                }
                
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
                CurrentContext.SendBroadcast(mediaScanIntent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save image to gallery", ex);
            }
        }

        public async Task<byte[]> GenerateImageThumbnailAsync(FileResult file)
        {
            using (var stream = await file.OpenReadAsync())
            {
                using (var memoryStream = new MemoryStream())
                {
                    await stream.CopyToAsync(memoryStream);
                    byte[] bytes = memoryStream.ToArray();

                    if (bytes.Length <= 0) return null;

                    return await ResizeImageAndroid(bytes, 0.5f, 90);
                }
            }
        }


        public static async Task<byte[]> ResizeImageAndroid(byte[] imageData, float scaleFactor, int quality)
        {
            // Load the bitmap
            Bitmap originalImage = BitmapFactory.DecodeByteArray(imageData, 0, imageData.Length);

            float oldWidth = (float)originalImage.Width;
            float oldHeight = (float)originalImage.Height;

            float newHeight = oldHeight * scaleFactor;
            float newWidth = oldWidth * scaleFactor;

            Bitmap resizedImage = Bitmap.CreateScaledBitmap(originalImage, (int)newWidth, (int)newHeight, false);

            using (MemoryStream ms = new MemoryStream())
            {
                await resizedImage.CompressAsync(Bitmap.CompressFormat.Jpeg, quality, ms);
                return ms.ToArray();
            }
        }
    }
}