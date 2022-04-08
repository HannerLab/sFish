using System;
using HannerLabApp.Configuration;
using HannerLabApp.Utils;
using LiteDB;

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


        [BsonIgnoreAttribute] // Image in base64 format only transiently
        public string File64 { get; set; } = string.Empty;

        [BsonIgnoreAttribute]
        public string ContentType => "media/jpeg"; // Mimetype of the photo file

        [BsonIgnoreAttribute]
        public string FileName
        {
            get
            {
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

                return $"{n}_{Timestamp.ToString(Constants.ExportPhotoTimeFormat)}.jpg";
            }
        }
    }
}
