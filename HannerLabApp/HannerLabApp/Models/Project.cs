using System;

namespace HannerLabApp.Models
{
    public class Project : ISavable
    {
        public Guid Id { get; set; }
        public Guid ActivityId { get; set; }
        public bool IsAdvancedShown { get; set; }
        public Guid ProjectId { get => Id; set => Id = value; }
        public DateTime Timestamp { get; set; }
        public DateTime LastUpdated { get; set; }
        public DateTime LastAccessed { get; set; }
        public string UserSpecifiedId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }
        public string Owner { get; set; }
        public string ContactEmail { get; set; }
        public string RecordedBy { get; set; }
        public string Institution { get; set; }
        public string Notes { get; set; }
    }
}
