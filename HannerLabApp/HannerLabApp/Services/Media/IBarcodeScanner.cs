using System.Threading.Tasks;

namespace HannerLabApp.Services.Media
{
    public interface IBarcodeScanner
    {
        Task<string> ScanBarcodeAsync();
    }
}
