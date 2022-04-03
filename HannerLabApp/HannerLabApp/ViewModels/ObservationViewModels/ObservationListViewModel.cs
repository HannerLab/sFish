using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Repositorys;

namespace HannerLabApp.ViewModels.ObservationViewModels
{
    public class ObservationListViewModel : ProjectAndActivitySpecificListViewModel<Observation>
    {
        public ObservationListViewModel(IPageService pageService, IReadOnlyRepository<Observation> repository) : base(pageService, repository)
        {
        }
    }
}
