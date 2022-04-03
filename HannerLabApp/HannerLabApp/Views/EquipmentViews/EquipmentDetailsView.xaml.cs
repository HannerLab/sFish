using HannerLabApp.Models;
using HannerLabApp.ViewModels.EquipmentViewModels;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views.EquipmentViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EquipmentDetailsView : DetailsViewBase<Equipment>
    {
        public EquipmentDetailsView(EquipmentViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}