using System;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace HannerLabApp.Services.Media
{
    public class PhotoCaptureService : IPhotoCaptureService
    {
        /// <summary>
        /// Opens the file/ gallery file picker to allow user to load an existing photo
        /// </summary>
        /// <returns></returns>
        public async Task<FileResult> PickPhotoAsync()
        {
            try
            {
                var photo = await MediaPicker.PickPhotoAsync();

                return photo;
            }
            catch (FeatureNotSupportedException ex)
            {
                Console.WriteLine($"PhotoCaptureService: Feature is not supported on the device: {ex.Message}");
                return null;
            }
            catch (PermissionException ex)
            {
                Console.WriteLine($"PhotoCaptureService: Permissions not granted: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PhotoCaptureService: Unknown error: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Captures or selects a photograph and returns it as a base64 string
        /// </summary>
        /// <param name="saveToGallery">Optionally save the captured photo to the default platform specific gallery with the default name</param>
        /// <returns></returns>
        public async Task<FileResult> CapturePhotoAsync(bool saveToGallery)
        {
            try
            {
                var fileResult = await MediaPicker.CapturePhotoAsync();

                // canceled by user.
                if (fileResult == null)
                    return null;

                if (saveToGallery)
                {
                    var platformSpecificMediaService = DependencyService.Get<IMediaService>();
                    await platformSpecificMediaService.SaveImageFileResultToGalleryAsync(fileResult);
                }

                // Return file as base64 string
                return fileResult;
            }
            catch (FeatureNotSupportedException ex)
            {
                Console.WriteLine($"PhotoCaptureService: Feature is not supported on the device: {ex.Message}");
                return null;
            }
            catch (PermissionException ex)
            {
                Console.WriteLine($"PhotoCaptureService: Permissions not granted: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PhotoCaptureService: Unknown error: {ex.Message}");
                return null;
            }
        }
    }
}
