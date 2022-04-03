using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;

namespace HannerLabApp.ViewModels.ActivityViewModels
{
    public class ActivityDetailsViewModel : DetailsViewModelBase<Activity>
    {
        public ActivityDetailsViewModel(IValidableViewModel<Activity> viewModel, IManager<Activity> manager, IPageService pageService) : base(viewModel, manager, pageService)
        {
        }
    }
}
