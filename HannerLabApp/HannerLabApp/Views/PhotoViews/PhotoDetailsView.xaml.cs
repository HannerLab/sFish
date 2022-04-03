using System;
using HannerLabApp.Models;
using HannerLabApp.ViewModels.PhotoViewModels;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views.PhotoViews
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PhotoDetailsView : DetailsViewBase<Photo>
    {
        public PhotoDetailsView(PhotoViewModel viewModel) : base(viewModel)
        {
            InitializeComponent();
        }

        private void StationPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (StationPicker.SelectedItem != null)
            {
                this.SitePicker.SelectedItem = null;
                this.ObservationPicker.SelectedItem = null;
                this.EdnaPicker.SelectedItem = null;
                this.ReadingPicker.SelectedItem = null;
            }
        }

        private void SitePicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (SitePicker.SelectedItem != null)
            {
                this.StationPicker.SelectedItem = null;
                this.ObservationPicker.SelectedItem = null;
                this.EdnaPicker.SelectedItem = null;
                this.ReadingPicker.SelectedItem = null;
            }
        }

        private void ObservationPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ObservationPicker.SelectedItem != null)
            {
                this.SitePicker.SelectedItem = null;
                this.StationPicker.SelectedItem = null;
                this.EdnaPicker.SelectedItem = null;
                this.ReadingPicker.SelectedItem = null;
            }
        }

        private void EdnaPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (EdnaPicker.SelectedItem != null)
            {
                this.SitePicker.SelectedItem = null;
                this.StationPicker.SelectedItem = null;
                this.ObservationPicker.SelectedItem = null;
                this.ReadingPicker.SelectedItem = null;
            }
        }

        private void ReadingPicker_OnSelectedIndexChanged(object sender, EventArgs e)
        {
            if (ReadingPicker.SelectedItem != null)
            {
                this.SitePicker.SelectedItem = null;
                this.StationPicker.SelectedItem = null;
                this.ObservationPicker.SelectedItem = null;
                this.EdnaPicker.SelectedItem = null;
            }
        }
    }
}