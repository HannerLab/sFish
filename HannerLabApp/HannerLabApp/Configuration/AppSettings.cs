using System;
using Xamarin.Essentials;

namespace HannerLabApp.Configuration
{
    /// <summary>
    /// User managed settings
    /// </summary>
    public class AppSettings
    {
        private Guid _defaultProjectId = Guid.Empty;

        private const string LastSyncTimeKey = "LastSyncTimeKey";
        private const string CurrentCloudApiTokenKey = "CurrentCloudApiTokenKey";
        private const string CurrentRecorderKey = "CurrentRecorderKey";
        private const string CurrentProjectKey = "CurrentProjectKey";
        private const string IsSavePhotosToGalleryEnabledKey = "IsSavePhotosToGalleryEnabledKey";
        private const string IsAdvanceModeDefaultEnabledKey = "IsAdvanceModeDefaultEnabledKey";

        public DateTime? LastSyncTime
        {
            get
            {
                var s = Preferences.Get(LastSyncTimeKey, string.Empty);
                if (string.IsNullOrEmpty(s)) return null;

                DateTime r;
                DateTime.TryParse(s, out r);
                if (r == DateTime.MinValue) return null;

                return r;
            }
            set
            {
                var s = value.ToString();

                Preferences.Set(LastSyncTimeKey, s);
            }
        }

        public bool IsSavePhotosToGalleryEnabled
        {
            get => Preferences.Get(IsSavePhotosToGalleryEnabledKey, false);
            set => Preferences.Set(IsSavePhotosToGalleryEnabledKey, value);
        }

        public bool IsAdvanceModeDefaultEnabled
        {
            get => Preferences.Get(IsAdvanceModeDefaultEnabledKey, false);
            set => Preferences.Set(IsAdvanceModeDefaultEnabledKey, value);
        }

        public string CurrentCloudApiToken
        {
            get => Preferences.Get(CurrentCloudApiTokenKey, string.Empty);
            set => Preferences.Set(CurrentCloudApiTokenKey, value.ToString());
        }

        /// <summary>
        /// The current project recorders. Names of these people, tagged to recorded data. Should default to the set project recorders designated by the current project.
        /// </summary>
        public string CurrentRecorder
        {
            get => Preferences.Get(CurrentRecorderKey, string.Empty);
            set => Preferences.Set(CurrentRecorderKey, value ??= string.Empty);
        }

        /// <summary>
        /// The id of the project that is currently selected
        /// </summary>
        public Guid CurrentProjectId
        {
            get
            {
                var s = Preferences.Get(CurrentProjectKey, _defaultProjectId.ToString());
                try
                {
                    return Guid.Parse(s);
                }
                catch (Exception e)
                {
                    Console.WriteLine("ERROR: PARSING PROJECT ID FROM SETTINGS: " + e);
                    return _defaultProjectId;
                }
            }
            set => Preferences.Set(CurrentProjectKey, value.ToString());
        }
    }
}
