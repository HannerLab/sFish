using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Repositorys;

namespace HannerLabApp.ViewModels.ReadingViewModels
{
    public class ReadingListViewModel : ProjectAndActivitySpecificListViewModel<Reading>
    {
        public ReadingListViewModel(IPageService pageService, IReadOnlyRepository<Reading> repository) : base(pageService, repository)
        {
        }
    }
}
