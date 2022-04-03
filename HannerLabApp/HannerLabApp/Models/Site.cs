using System;

namespace HannerLabApp.Models
{
    public class Site : ISavable
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ActivityId { get; set; }
        public string RecordedBy { get; set; }
        public bool IsAdvancedShown { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime LastUpdated { get; set; }

        public string UserSpecifiedId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public string Notes { get; set; }

        public string Country { get; set; }
        public string StateProvince { get; set; }
        public string Region { get; set; }
        public string Locality { get; set; }
        public string WaterBody { get; set; }
        public string Hydrology { get; set; }
        public string Geology { get; set; }
    }
}
