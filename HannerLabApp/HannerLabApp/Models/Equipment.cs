using System;
using System.ComponentModel;

namespace HannerLabApp.Models
{
    public class Equipment : ISavable
    {
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ActivityId { get; set; }
        public string RecordedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UserSpecifiedId { get; set; }
        public bool IsAdvancedShown { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public string Parameters { get; set; }
        public string Manufacturer { get; set; }
        public string Vendor { get; set; }
        public string SerialNumber { get; set; }
        public string DeviceModel { get; set; }
        public EquipmentType Category { get; set; }

        // Common units
        public string UnitDepth { get; set; }
        public string UnitOffshoreDistance { get; set; }

        // eDNA units
        public string UnitPressure { get; set; }
        public string UnitVolumeFiltered { get; set; }
        public string UnitTimeFiltering { get; set; }
        public string UnitFlowRate { get; set; }

        // Reading Units
        public string UnitVelocity { get; set; }
        public string UnitTemperature { get; set; }
        public string UnitPh { get; set; }
        public string UnitConductivity { get; set; }
        public string UnitDissolvedOxygen { get; set; }
        public string UnitSuspendedSolids { get; set; }
        public string UnitSecchi { get; set; }
        public string UnitTurbidity { get; set; }
        public string UnitChlorophyll { get; set; }

        // GPS
        public string UnitGps { get; set; }
    }

    public enum EquipmentType
    {
        [Description("eDNA")]
        Edna,
        [Description("Water chemistry probe")]
        Reading,
        [Description("GPS unit")]
        Gps,
        [Description("Other")]
        Other
    }

    /// <summary>
    /// Available unit types that can be defined. Matches the property names values they relate to.
    /// </summary>
    public enum UnitType
    {
        [Description("")]
        Empty,
        [Description("Depth")]
        Depth,
        [Description("Offshore Distance")]
        OffshoreDistance,
        [Description("Pressure")]
        Pressure,
        [Description("Volume Filtered")]
        VolumeFiltered,
        [Description("Time Filtering")]
        TimeFiltering,
        [Description("Flowrate")]
        FlowRate,
        [Description("Velocity")]
        Velocity,
        [Description("Temperature")]
        Temperature,
        [Description("pH")]
        Ph,
        [Description("Conductivity")]
        Conductivity,
        [Description("Dissolved Oxygen")]
        DissolvedOxygen,
        [Description("Total Suspended Solids")]
        SuspendedSolids,
        [Description("Sechhi")]
        Secchi,
        [Description("Turbidity")]
        Turbidity,
        [Description("Chlorophyll")]
        Chlorophyll,
        [Description("GPS Datum")]
        Gps
    }
}
