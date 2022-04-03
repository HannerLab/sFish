using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace HannerLabApp.Controls
{
    /// <summary>
    /// A basic control combining both the date, and time pickers.
    /// </summary>
    public class DateTimePicker : ContentView, INotifyPropertyChanged
    {
        private Entry _entry { get; set; } = new Entry() { WidthRequest = 300 };
        private DatePicker _datePicker { get; set; } = new DatePicker() { MinimumDate = DateTime.MinValue, IsVisible = false };
        private TimePicker _timePicker { get; set; } = new TimePicker() { IsVisible = false };
        private string _stringFormat { get; set; }
        private TimeSpan _time
        {
            get { return TimeSpan.FromTicks(DateTime.Ticks); }
            set { DateTime = new DateTime(DateTime.Date.Ticks).AddTicks(value.Ticks); }
        }
        private DateTime _date
        {
            get { return DateTime.Date; }
            set { DateTime = new DateTime(DateTime.TimeOfDay.Ticks).AddTicks(value.Ticks); }
        }

        public string StringFormat { get { return _stringFormat ?? "dd/MM/yyyy HH:mm"; } set { _stringFormat = value; } }
        public DateTime DateTime
        {
            get { return (DateTime)GetValue(DateTimeProperty); }
            set { SetValue(DateTimeProperty, value); OnPropertyChanged(nameof(DateTime)); }
        }

        public static BindableProperty DateTimeProperty = BindableProperty.Create(nameof(DateTime), typeof(DateTime), typeof(DateTimePicker), DateTime.Now, BindingMode.TwoWay, propertyChanged: DTPropertyChanged);

        public DateTimePicker()
        {
            Content = new StackLayout()
            {
                Children =
            {
                _datePicker,
                _timePicker,
                _entry
            }
            };

            _datePicker.SetBinding(DatePicker.DateProperty, nameof(_date));
            _timePicker.SetBinding(TimePicker.TimeProperty, nameof(_time));
            _timePicker.Unfocused += (sender, args) => _time = _timePicker.Time;
            _datePicker.Focused += (s, a) => UpdateEntryText();

            GestureRecognizers.Add(new TapGestureRecognizer()
            {
                Command = new Command(() => _datePicker.Focus())
            });

            _entry.Focused += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() => _datePicker.Focus());
            };

            _datePicker.Unfocused += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    _timePicker.Focus();
                    _date = _datePicker.Date;
                    UpdateEntryText();
                });
            };
        }

        private void UpdateEntryText()
        {
            _entry.Text = DateTime.ToString(StringFormat);
        }

        private static void DTPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            var timePicker = bindable as DateTimePicker;
            timePicker.UpdateEntryText();
        }
    }
}
