using HannerLabApp.Models;
using HannerLabApp.ViewModels.ReadingViewModels;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views.ReadingViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ReadingDetailsView : DetailsViewBase<Reading>
    {
        public ReadingDetailsView(ReadingViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}