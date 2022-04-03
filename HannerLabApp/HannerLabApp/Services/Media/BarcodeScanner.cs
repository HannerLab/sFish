using System.Threading.Tasks;
using Xamarin.Forms;
using ZXing.Net.Mobile.Forms;

namespace HannerLabApp.Services.Media
{
    public class BarcodeScanner : IBarcodeScanner
    {
        private readonly IPageService _pageService;

        public BarcodeScanner(IPageService pageService)
        {
            _pageService = pageService;
        }

        public async Task<string> ScanBarcodeAsync()
        {
            var barcode = string.Empty;
            var scanPage = new CustomBarcodeScanPage();

            scanPage.OnScanResult += (result) => {
                // Stop scanning
                scanPage.IsScanning = false;

                // Pop the page and show the result
                Device.BeginInvokeOnMainThread(async () =>
                {
                    barcode = result.Text;
                    //await _pageService.CloseModalAsync();
                    await _pageService.BackAsync();
                });
            };

            //await _pageService.OpenModalAsync(scanPage);
            await _pageService.NavigateToAsync(scanPage);

            while (scanPage.IsScanning)
            {
                await Task.Delay(1);
            }

            return barcode;
        }
    }

    public class CustomBarcodeScanPage : ZXingScannerPage
    {
        protected override void OnDisappearing()
        {
            IsScanning = false;

            base.OnDisappearing();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            IsScanning = true;
        }
    }
}
