using System;
using HannerLabApp.Models;
using HannerLabApp.Services.Managers;
using HannerLabApp.Utils;
using Xamarin.Forms;

namespace HannerLabApp
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        // If no project is selected, hide the nav bar item to sites, stations, etc
        private bool _isProjectSelected = false;
        public bool IsProjectSelected
        {
            get => _isProjectSelected;
            private set => _isProjectSelected = value;
        }

        public AppShell()
        {
            InitializeComponent();

            UpdateFlyoutItemVisibility();

            // Reload whenever project is changed
            MessagingCenter.Subscribe<GenericManager<Project>, Project>
            (this, MsgEvents.GetModel(typeof(Project), MsgEvents.Event.Update),
                (sender, arg) => OnProjectChanged(arg));
        }

        private void OnProjectChanged(Project project)
        {
            IsProjectSelected = project.Id != Guid.Empty;

            UpdateFlyoutItemVisibility();
        }

        private void UpdateFlyoutItemVisibility()
        {
            MenuItemExport.IsVisible = IsProjectSelected;
            MenuItemSitesStations.IsVisible = IsProjectSelected;
            MenuItemSamples.IsVisible = IsProjectSelected;
            MenuItemObservations.IsVisible = IsProjectSelected;
            MenuItemEquipment.IsVisible = IsProjectSelected;

            MenuItemSettings.IsVisible = true;
        }
    }
}
