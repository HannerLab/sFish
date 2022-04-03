using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Repositorys;

namespace HannerLabApp.ViewModels.PhotoViewModels
{
    public class PhotoListViewModel : ProjectAndActivitySpecificListViewModel<Photo>
    {
        public PhotoListViewModel(IPageService pageService, IReadOnlyRepository<Photo> repository) : base(pageService, repository)
        {
        }
    }
}
