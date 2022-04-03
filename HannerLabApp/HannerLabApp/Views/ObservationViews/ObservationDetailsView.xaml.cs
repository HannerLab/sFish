using System;
using HannerLabApp.Models;
using HannerLabApp.ViewModels.ObservationViewModels;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views.ObservationViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ObservationDetailsView : DetailsViewBase<Observation>
    {
        public ObservationDetailsView(ObservationViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        private void StationPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (StationPicker.SelectedItem != null)
            {
                this.SitePicker.SelectedItem = null;
            }
        }

        private void SitePicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SitePicker.SelectedItem != null)
            {
                this.StationPicker.SelectedItem = null;
            }
        }
    }
}