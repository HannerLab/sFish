using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Repositorys;

namespace HannerLabApp.ViewModels.SiteViewModels
{
    public class SiteListViewModel : ProjectSpecificListViewModel<Site>
    {
        public SiteListViewModel(IPageService pageService, IReadOnlyRepository<Site> repository) : base(pageService, repository)
        {
        }
    }
}
