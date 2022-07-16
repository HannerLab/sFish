using System.IO;
using Xamarin.Essentials;

namespace HannerLabApp.Configuration
{
    public static class Constants
    {
        public const string DatabaseFilename = "data.db";

        public static readonly string TempDirectory = FileSystem.CacheDirectory;

        public static readonly string LogDirectory = Path.Combine(FileSystem.AppDataDirectory, "logs/");
        public static readonly string MediaDirectory = Path.Combine(FileSystem.AppDataDirectory, "media/");
        public static readonly string AppDataDirectory = Path.Combine(FileSystem.AppDataDirectory, "appdata/");
        public static readonly string ExportDirectory = Path.Combine(FileSystem.AppDataDirectory, "exports/");

        public const string AppGithubBaseRepoUrl = "HannerLab/sFish";
        public const string AppVersionString = "1.0.2";
        public const string AppDescription = "The Sample and Field data collection Information System for the Hanner Lab.";
        public const string AppCopyright = "© 2022 The Hanner Lab, University of Guelph";
        public const string AppName = "sFish";

        public const string ExportDateFormat = "yyyy-MM-dd";
        public const string ExportTimeFormat = "HH:mm";
        public const string ExportDateTimeFormat = "yyyy-MM-dd HH:mm";
        public const string ExportPhotoTimeFormat = "yyyy-MM-dd-HHmmss";
    }
}
