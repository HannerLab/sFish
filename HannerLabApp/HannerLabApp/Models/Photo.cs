using System;
using HannerLabApp.Configuration;
using HannerLabApp.Utils;
using LiteDB;
using Xamarin.Essentials;

namespace HannerLabApp.Models
{
    public class Photo : ISavable
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ActivityId { get; set; }
        public string Notes { get; set; }
        public string UserSpecifiedId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public DateTime LastUpdated { get; set; }
        public Station Station { get; set; }
        public Site Site { get; set; }
        public string RecordedBy { get; set; }
        public Edna Edna { get; set; }
        public Observation Observation { get; set; }
        public Reading Reading { get; set; }
        public bool IsAdvancedShown { get; set; }


        [BsonIgnoreAttribute] 
        public FileResult File { get; set; }

        [BsonIgnoreAttribute]
        public string Thumbnail { get; set; } // Base 64 encoded image thumbnail

        [BsonIgnoreAttribute]
        public string GetFileName()
        {
            if (File == null) return string.Empty;

            string fileExtension = string.Empty;

            if (File.ContentType.ToLower().Contains("jpg") || File.ContentType.ToLower().Contains("jpeg"))
                fileExtension = "jpg";
            if (File.ContentType.ToLower().Contains("png"))
                fileExtension = "png";

            string n;

            if (Station != null)
                n = Station.UserSpecifiedId;
            else if (Site != null)
                n = Site.UserSpecifiedId;
            else if (Edna != null)
                n = Edna.UserSpecifiedId;
            else if (Reading != null)
                n = Reading.UserSpecifiedId;
            else if (Observation != null)
                n = Observation.UserSpecifiedId;
            else
                n = IdGenerator.GetNewRandomId();

            return $"{n}_{Timestamp.ToString(Constants.ExportPhotoTimeFormat)}.{fileExtension}";
        }
    }
}
