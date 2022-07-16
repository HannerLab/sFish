using System;
using HannerLabApp.ViewModels.ProjectViewModels;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views.ProjectViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ProjectPageView
    {
        public ProjectPageView()
        {
            InitializeComponent();
        }

        void OnProjectPickerClicked(object sender, EventArgs e)
        {
            ProjectPicker.Focus();
        }

        void OnProjectPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            // Execute project selection command
            try
            {
                (BindingContext as ProjectPageViewModel).LoadProjectCommand.Execute(null);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            
        }
    }
}