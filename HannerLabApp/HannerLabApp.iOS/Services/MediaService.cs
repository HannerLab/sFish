using System;
using Foundation;
using HannerLabApp.iOS.Services;
using HannerLabApp.Services.Media;
using UIKit;

[assembly: Xamarin.Forms.Dependency(typeof(MediaService))]
namespace HannerLabApp.iOS.Services
{
    public class MediaService : IMediaService
    {
        public void SaveImageFromByte(byte[] imageByte, string fileName)
        {
            var imageData = new UIImage(NSData.FromArray(imageByte));
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
    }
}