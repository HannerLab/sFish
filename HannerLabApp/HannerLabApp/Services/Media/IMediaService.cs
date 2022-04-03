namespace HannerLabApp.Services.Media
{
    /// <summary>
    /// Interface for handling platform specific media features such as saving to photo gallery.
    /// </summary>
    public interface IMediaService
    {
        void SaveImageFromByte(byte[] imageByte, string filename);
    }
}
