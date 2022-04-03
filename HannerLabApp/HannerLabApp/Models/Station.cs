using System;

namespace HannerLabApp.Models
{
    public class Station : ISavable
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ActivityId { get; set; }
        public string RecordedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsAdvancedShown { get; set; }
        public DateTime LastUpdated { get; set; }

        public string UserSpecifiedId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public double? Elevation { get; set; }
        public string WayPoint { get; set; }
        public string Notes { get; set; }

        public string Habitat { get; set; }
        public string WaterBody { get; set; }
        public string FloodPlain { get; set; }
        public string Substrate { get; set; }
        public string Geology { get; set; }
        public string Hydrology { get; set; }
        public string Stratification { get; set; }
        public string VegetationAquatic { get; set; }
        public string VegetationTerrestrial { get; set; }

        public Site Site { get; set; }
    }
}
