using System;
using System.Threading.Tasks;
using System.Windows.Input;
using HannerLabApp.Services;
using TinyMvvm;

namespace HannerLabApp.ViewModels
{
    // For in app debugging... Disabled on release ver
    public class DebugViewModel : ViewModelBase
    {
        private string _debugText;
        private DateTime _testDateTime;

        public DateTime TestDateTime
        {
            get => _testDateTime;
            set => Set(ref _testDateTime, value);
        }

        public string DebugText
        {
            get => _debugText;
            set => Set(ref _debugText, value);
        }

        public ICommand DebugCommand { get; private set; }

        public DebugViewModel()
        {
            DebugCommand = new TinyCommand(async () => await DebugAsync());
        }

        private async Task DebugAsync()
        {
            await DisplayTodoNotice();
        }

        private async Task DisplayTodoNotice()
        {
            await (new PageService()).ShowAlertAsync("Not available yet!",
                "Please stand by while we develop the export data format. To provide input to the data export format contact the labs data team. In the meantime you can access the applications raw data.", "Ok");
        }
    }
}
