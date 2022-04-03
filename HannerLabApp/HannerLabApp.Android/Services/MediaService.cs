using Android.Content;
using HannerLabApp.Droid.Services;
using HannerLabApp.Services.Media;
using Plugin.CurrentActivity;
using System;

[assembly: Xamarin.Forms.Dependency(typeof(MediaService))]
namespace HannerLabApp.Droid.Services
{
    public class MediaService : IMediaService
    {
        Context CurrentContext => CrossCurrentActivity.Current.Activity;
        public void SaveImageFromByte(byte[] imageByte, string fileName)
        {
            try
            {
                var storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures).AbsolutePath;
                string path = System.IO.Path.Combine(storagePath, fileName);
                System.IO.File.WriteAllBytes(path, imageByte);
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
                CurrentContext.SendBroadcast(mediaScanIntent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to save image to gallery", ex);
            }
        }
    }
}