using System.Collections.ObjectModel;
using System.Windows.Input;
using HannerLabApp.Models;

namespace HannerLabApp.ViewModels
{
    public interface IListViewModel<T> where T : ISavable
    {
        string SearchText { get; set; }
        IValidableViewModel<T> SelectedItem { get; set; }
        ObservableCollection<IValidableViewModel<T>> AllItems { get; set; }
        ObservableCollection<IValidableViewModel<T>> FilteredItems { get; set; }
        public ICommand SelectionChangeCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand EditCommand { get; }
        public ICommand DeleteCommand { get; }
    }
}
