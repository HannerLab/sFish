using Autofac;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;
using HannerLabApp.ViewModels;
using TinyMvvm.Forms;

namespace HannerLabApp.Views
{
    public abstract class DetailsViewBase<T> : ViewBase, IDetailsView<T> where T : ISavable
    {
        protected DetailsViewBase(IValidableViewModel<T> viewModel)
        {
            IManager<T> manager = App.AppContainer.Resolve<IManager<T>>();
            IPageService pageService = App.AppContainer.Resolve<IPageService>();

            viewModel ??= App.AppContainer.Resolve<IValidableViewModel<T>>();

            var detailsViewModel = App.AppContainer.Resolve<IDetailsViewModel<T>>(
                new NamedParameter("viewModel", viewModel),
                new NamedParameter("manager", manager),
                new NamedParameter("pageService", pageService));

            BindingContext = detailsViewModel;
        }
    }
}
