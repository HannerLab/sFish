using HannerLabApp.Models;
using HannerLabApp.ViewModels.StationViewModels;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views.StationViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StationDetailsView : DetailsViewBase<Station>
    {
        public StationDetailsView(StationViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}