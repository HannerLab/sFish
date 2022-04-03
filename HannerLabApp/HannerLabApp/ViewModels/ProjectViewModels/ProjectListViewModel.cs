using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Repositorys;

namespace HannerLabApp.ViewModels.ProjectViewModels
{
    public class ProjectListViewModel : ListViewModelBase<Project>
    {
        public ProjectListViewModel(IPageService pageService, IReadOnlyRepository<Project> repository) : base(pageService, repository)
        {
        }
    }
}
