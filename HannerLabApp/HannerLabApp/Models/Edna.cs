using System;

namespace HannerLabApp.Models
{
    /// <summary>
    /// Main data model for a e-DNA filter collection sample or event
    /// </summary>
    public class Edna : ISavable, ISample
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ActivityId { get; set; }
        public string Notes { get; set; }
        public string UserSpecifiedId { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; }
        public DateTime LastUpdated { get; set; }
        public Station Station { get; set; }
        public string RecordedBy { get; set; }
        public string CollectedBy { get; set; }
        public string Name { get; set; } = string.Empty;
        public double? FlowRate { get; set; }
        public long? TimeFiltering { get; set; }
        public double? VolumeFiltered { get; set; }
        public double? Depth { get; set; }
        public double? OffshoreDistance { get; set; }
        public double? Pressure { get; set; }
        public Equipment Equipment { get; set; }
        public bool IsAdvancedShown { get; set; }
    }
}
