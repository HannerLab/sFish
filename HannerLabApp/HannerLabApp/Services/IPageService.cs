using System.Threading.Tasks;
using Xamarin.Forms;

namespace HannerLabApp.Services
{
    public interface IPageService
    {
        Task BackAsync();
        Task ShowAlertAsync(string title, string message, string confirmationText);
        Task<bool> ShowYesNoAlertAsync(string title, string message, string yesText, string noText);
        Task NavigateToAsync(string pageKey, object parameter);
        Task NavigateToAsync(object parameter);

        Task OpenModalAsync(Page parameter);
        Task CloseModalAsync();
    }
}
