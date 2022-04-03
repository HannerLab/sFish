using System;

namespace HannerLabApp.Models
{
    public class Reading : ISavable, ISample
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ActivityId { get; set; }
        public bool IsAdvancedShown { get; set; }
        public string Notes { get; set; }
        public string UserSpecifiedId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public DateTime LastUpdated { get; set; }
        public Station Station { get; set; }
        public string RecordedBy { get; set; }
        public string CollectedBy { get; set; }
        public string Name { get; set; } = string.Empty;
        public double? Depth { get; set; }
        public double? Velocity { get; set; }
        public double? Temperature { get; set; }
        public double? Ph { get; set; }
        public double? Conductivity { get; set; }
        public double? DissolvedOxygen { get; set; }
        public double? SuspendedSolids { get; set; }
        public double? Secchi { get; set; }
        public double? Turbidity { get; set; }
        public double? Chlorophyll { get; set; }
        public double? OffshoreDistance { get; set; }
        public Equipment Equipment { get; set; }
    }
}
