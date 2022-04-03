using HannerLabApp.Models;
using HannerLabApp.ViewModels.ActivityViewModels;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views.ActivityViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ActivityDetailsView : DetailsViewBase<Activity>
    {
        public ActivityDetailsView(ActivityViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}