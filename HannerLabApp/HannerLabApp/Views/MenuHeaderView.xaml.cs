using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HannerLabApp.Models;
using HannerLabApp.Services.Managers;
using HannerLabApp.Utils;
using TinyMvvm;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace HannerLabApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuHeaderView : ContentView
    {
        private string _currentRecorder;
        private string _currentProjectName;
        private Guid _currentProjectId;

        public ICommand RecordersEntryCommand { get; private set; }

        public MenuHeaderView()
        {
            InitializeComponent();

            GetDefaultLabels();
            SetLabels();

            RecordersEntryCommand = new TinyCommand(async () => await SetRecordersAsync());

            // Reload whenever project is changed
            MessagingCenter.Subscribe<GenericManager<Project>, Project>
                (this, MsgEvents.GetModel(typeof(Project), MsgEvents.Event.Update),
                    (sender, arg) => OnProjectChanged(arg));
        }

        private void OnProjectChanged(Project project)
        {
            this._currentProjectName = project.Name;
            this._currentRecorder = project.RecordedBy;
            this._currentProjectId = project.Id;

            this.HelpLabel.IsVisible = true;

            SetLabels();
        }

        private void OnTapGestureRecognizerTapped(object sender, EventArgs args)
        {
            this.HelpLabel.IsVisible = false;
            if (_currentProjectId != Guid.Empty)
                RecordersEntryCommand.Execute(null);
        }

        private async Task SetRecordersAsync()
        {
            var result = await App.Current.MainPage.DisplayPromptAsync("Change data collectors", "Enter the name(s)") ?? string.Empty;

            if (string.IsNullOrEmpty(result)) return;

            _currentRecorder = result.Trim();

            App.AppSettings.CurrentRecorder = _currentRecorder;

            SetLabels();
        }

        private void SetLabels()
        {
            if (_currentProjectId != Guid.Empty)
            {
                this.RecordersLabel.Text = $"Recorder(s): {_currentRecorder}";
            }
            else
            {
                this.RecordersLabel.Text = string.Empty;
                this.HelpLabel.Text = string.Empty;
            }
            this.NameLabel.Text = _currentProjectName;
        }

        private void GetDefaultLabels()
        {
            _currentProjectName = "No Project Selected";
            _currentRecorder = App.AppSettings.CurrentRecorder;
        }
    }
}