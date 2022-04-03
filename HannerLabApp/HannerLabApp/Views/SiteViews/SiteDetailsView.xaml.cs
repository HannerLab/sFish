using HannerLabApp.Models;
using HannerLabApp.ViewModels.SiteViewModels;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views.SiteViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SiteDetailsView : DetailsViewBase<Site>
    {
        public SiteDetailsView(SiteViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}