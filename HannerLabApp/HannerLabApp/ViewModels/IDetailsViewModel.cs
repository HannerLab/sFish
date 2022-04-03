using System.Windows.Input;
using HannerLabApp.Models;

namespace HannerLabApp.ViewModels
{
    public interface IDetailsViewModel<T> where T : ISavable
    { 
        string Title { get; set; }
        bool IsEdit { get; set; }

        IValidableViewModel<T> ViewModel { get; }
        ICommand SaveCommand { get; }
        ICommand DeleteCommand { get; }
        ICommand ToggleModeCommand { get; }
    }
}
