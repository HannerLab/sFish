using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Repositorys;

namespace HannerLabApp.ViewModels.EquipmentViewModels
{
    public class EquipmentListViewModel : ProjectSpecificListViewModel<Equipment>
    {
        public EquipmentListViewModel(IPageService pageService, IReadOnlyRepository<Equipment> repository) : base(pageService, repository)
        {
        }
    }
}
