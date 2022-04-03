using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Repositorys;

namespace HannerLabApp.ViewModels.EdnaViewModels
{
    public class EdnaListViewModel : ProjectAndActivitySpecificListViewModel<Edna>
    {
        public EdnaListViewModel(IPageService pageService, IReadOnlyRepository<Edna> repository) : base(pageService, repository)
        {
        }
    }
}
