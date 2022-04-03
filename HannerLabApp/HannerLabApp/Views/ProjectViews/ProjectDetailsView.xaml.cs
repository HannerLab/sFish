using HannerLabApp.Models;
using HannerLabApp.ViewModels.ProjectViewModels;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views.ProjectViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectDetailsView : DetailsViewBase<Project>, IDetailsView<Project>
    {
        public ProjectDetailsView(ProjectViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }
    }
}