using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Repositorys;

namespace HannerLabApp.ViewModels.StationViewModels
{
    public class StationListViewModel : ProjectSpecificListViewModel<Station>
    {
        public StationListViewModel(IPageService pageService, IReadOnlyRepository<Station> repository) : base(pageService, repository)
        {
        }
    }
}
