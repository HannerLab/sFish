using System;
using System.Threading.Tasks;
using HannerLabApp.Utils;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HannerLabApp.Services.Media
{
    public class PhotoCaptureService : IPhotoCaptureService
    {
        public async Task<string> PickPhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();

                // canceled by user.
                if (photo == null)
                    return string.Empty;

                return await PhotoTools.GetPhotoAsString64Async(photo);
            }
            catch (FeatureNotSupportedException ex)
            {
                Console.WriteLine($"PhotoCaptureService: Feature is not supported on the device: {ex.Message}");
                return string.Empty;
            }
            catch (PermissionException ex)
            {
                Console.WriteLine($"PhotoCaptureService: Permissions not granted: {ex.Message}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PhotoCaptureService: Unknown error: {ex.Message}");
                return string.Empty;
            }
        }

        /// <summary>
        /// Captures or selects a photograph and returns it as a base64 string
        /// </summary>
        /// <param name="saveToGallery">Optionally save the captured photo to the default platform specific gallery with the default name</param>
        /// <returns></returns>
        public async Task<string> CapturePhotoAsync(bool saveToGallery)
        {
            try
            {
                var fileResult = await MediaPicker.CapturePhotoAsync();

                // canceled by user.
                if (fileResult == null)
                    return string.Empty;
                
                // Convert to base64string
                var file64 = await PhotoTools.GetPhotoAsString64Async(fileResult);
                var defaultFileName = fileResult.FileName;

                if (string.IsNullOrEmpty(file64))
                    return string.Empty;

                if (saveToGallery)
                {
                    var platformSpecificMediaService = DependencyService.Get<IMediaService>();
                    platformSpecificMediaService.SaveImageFromByte(System.Convert.FromBase64String(file64), defaultFileName);
                }

                // Return file as base64 string
                return file64;
            }
            catch (FeatureNotSupportedException ex)
            {
                Console.WriteLine($"PhotoCaptureService: Feature is not supported on the device: {ex.Message}");
                return string.Empty;
            }
            catch (PermissionException ex)
            {
                Console.WriteLine($"PhotoCaptureService: Permissions not granted: {ex.Message}");
                return string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PhotoCaptureService: Unknown error: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
