using System;
using System.Drawing;
using System.IO;
using System.Threading.Tasks;
using CoreGraphics;
using Foundation;
using HannerLabApp.iOS.Services;
using HannerLabApp.Services.Media;
using UIKit;
using Xamarin.Essentials;

[assembly: Xamarin.Forms.Dependency(typeof(MediaService))]
namespace HannerLabApp.iOS.Services
{
    public class MediaService : IMediaService
    {
        public async Task SaveImageFileResultToGalleryAsync(FileResult fileResult)
        {
            //await Task.Delay(1);
            try
            {
                var imageData = new UIImage(NSData.FromStream(await fileResult.OpenReadAsync()));

                imageData.SaveToPhotosAlbum((image, error) =>
                {
                    //you can retrieve the saved UI Image as well if needed using  
                    //var i = image as UIImage;  
                    if (error != null)
                    {
                        Console.WriteLine(error.ToString());
                    }
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cloud not save image to gallery");
                Console.WriteLine(ex);
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

                    return ResizeImageIOS(bytes, 0.5f, 90);
                }
            }
        }

        public static byte[] ResizeImageIOS(byte[] imageData, float scaleFactor, int quality)
        {
            UIImage originalImage = ImageFromByteArray(imageData);


            float oldWidth = (float)originalImage.Size.Width;
            float oldHeight = (float)originalImage.Size.Height;

            float newHeight = oldHeight * scaleFactor;
            float newWidth = oldWidth * scaleFactor;

            //create a 24bit RGB image
            using (CGBitmapContext context = new CGBitmapContext(IntPtr.Zero,
                       (int)newWidth, (int)newHeight, 8,
                       (int)(4 * newWidth), CGColorSpace.CreateDeviceRGB(),
                       CGImageAlphaInfo.PremultipliedFirst))
            {

                RectangleF imageRect = new RectangleF(0, 0, newWidth, newHeight);

                // draw the image
                context.DrawImage(imageRect, originalImage.CGImage);

                UIKit.UIImage resizedImage = UIKit.UIImage.FromImage(context.ToImage());

                // save the image as a jpeg
                return resizedImage.AsJPEG((float)quality).ToArray();
            }
        }

        public static UIKit.UIImage ImageFromByteArray(byte[] data)
        {
            if (data == null)
            {
                return null;
            }

            UIKit.UIImage image;
            try
            {
                image = new UIKit.UIImage(Foundation.NSData.FromArray(data));
            }
            catch (Exception e)
            {
                Console.WriteLine("Image load failed: " + e.Message);
                return null;
            }
            return image;
        }
    }
}