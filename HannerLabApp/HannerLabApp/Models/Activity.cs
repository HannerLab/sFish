using System;

namespace HannerLabApp.Models
{
    public class Activity : ISavable
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ActivityId { get; set; }
        public string Name { get; set; }
        public string RecordedBy { get; set; }
        public DateTime Timestamp { get; set; } // Submission timestamp
        public DateTime LastUpdated { get; set; }
        public string UserSpecifiedId { get; set; }
        public bool IsAdvancedShown { get; set; }
        public DateTime TimestampEnd { get; set; } // Activity start/end
        public DateTime TimestampStart { get; set; } // Activity start/end
        public string Organization { get; set; }
        public string LeadMember { get; set; }
        public string OtherMembers { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
    }
}
