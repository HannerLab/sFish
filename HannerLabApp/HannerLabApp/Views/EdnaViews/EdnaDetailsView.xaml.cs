using HannerLabApp.Models;
using HannerLabApp.ViewModels.EdnaViewModels;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views.EdnaViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EdnaDetailsView : DetailsViewBase<Edna>
    {
        public EdnaDetailsView(EdnaViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}