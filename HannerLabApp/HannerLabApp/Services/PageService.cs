using System.Threading.Tasks;
using TinyMvvm;
using TinyMvvm.Forms;
using Xamarin.Forms;

namespace HannerLabApp.Services
{
    public class PageService : IPageService
    {
        public async Task BackAsync()
        {
            await NavigationHelper.Current.BackAsync();
        }

        public async Task NavigateToAsync(string pageKey, object parameter)
        { 
            await NavigationHelper.Current.NavigateToAsync(pageKey, parameter);
        }

        public async Task NavigateToAsync(object parameter)
        {
            await NavigationHelper.Current.NavigateToAsync((Page) parameter);
        }

        public async Task OpenModalAsync(Page parameter)
        {
            await NavigationHelper.Current.OpenModalAsync(parameter);
        }

        public async Task CloseModalAsync()
        {
            await NavigationHelper.Current.CloseModalAsync();
        }

        public async Task ShowAlertAsync(string title, string message, string confirmationText)
        {
            await App.Current.MainPage.DisplayAlert(title, message, confirmationText);
        }

        public async Task<bool> ShowYesNoAlertAsync(string title, string message, string yesText = "Yes", string noText = "No")
        {
            return await App.Current.MainPage.DisplayAlert(title, message, yesText, noText);
        }
    }
}
