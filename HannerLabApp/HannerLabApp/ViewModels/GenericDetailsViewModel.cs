using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;

namespace HannerLabApp.ViewModels
{
    /// <summary>
    /// A generic implementation of the DetailsViewModel, which represents inputting details of an ISavable Model. Contains no dependencies and can be used for data models with no children.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericDetailsViewModel<T> : DetailsViewModelBase<T> where T : ISavable
    {
        public GenericDetailsViewModel(IValidableViewModel<T> viewModel, IManager<T> manager, IPageService pageService) : base(viewModel, manager, pageService)
        {
        }
    }
}
