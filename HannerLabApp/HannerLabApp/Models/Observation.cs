using System;
using System.ComponentModel;

namespace HannerLabApp.Models
{
    public class Observation : ISavable
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
        public Site Site { get; set; }
        public string RecordedBy { get; set; }
        public string ObservedBy { get; set; }
        public string Name { get; set; } = string.Empty;
        public double? Temperature { get; set; }
        public WindLevel WindLevel { get; set; }
        public PrecipitationLevel PrecipitationLevel { get; set; }
        public CloudCoverLevel CloudCoverLevel { get; set; }
        public StormYesterday StormYesterday { get; set; }
        public CloudCover CloudCover { get; set; }
        public string Phenology { get; set; }
        public string Wildlife { get; set; }
        public string Anthropogenic { get; set; }
    }

    public enum WindLevel
    {

        [Description("None")]
        None,
        [Description("Low")]
        Low,
        [Description("Medium")]
        Medium,
        [Description("High")]
        High
    }

    public enum PrecipitationLevel
    {
        [Description("None")]
        None,
        [Description("Low")]
        Low,
        [Description("Medium")]
        Medium,
        [Description("High")]
        High
    }

    public enum CloudCoverLevel
    {
        [Description("None")]
        None,
        [Description("Low")]
        Low,
        [Description("Medium")]
        Medium,
        [Description("High")]
        High
    }

    public enum StormYesterday
    {
        [Description("No")]
        No,
        [Description("Yes")]
        Yes,
        [Description("Unknown")]
        Unknown
    }

    public enum CloudCover
    {
        [Description("Sunny")]
        Sunny,
        [Description("PartlySunny")]
        PartlySunny,
        [Description("Cloudy")]
        Cloudy,
        [Description("PartlyRainy")]
        PartlyRainy,
        [Description("Rainy")]
        Rainy
    }
}
