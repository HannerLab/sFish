using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Hlug
{
    [ExcelSheet("Stations")]
    public class ExportStation
    {
        [ExcelColumn("StationId")]
        public string StationId { get; set; }

        [ExcelColumn("StationSiteId")]
        public string StationSiteId { get; set; }

        [ExcelColumn("StationProjectId")]
        public string StationProjectId { get; set; }

        [ExcelColumn("StationWayPoint")]
        public string StationWayPoint { get; set; }

        [ExcelColumn("StationLatitude")]
        public string StationLatitude { get; set; }

        [ExcelColumn("StationLongitude")]
        public string StationLongitude { get; set; }

        [ExcelColumn("StationElevation")]
        public string StationElevation { get; set; }

        [ExcelColumn("StationHabitat")]
        public string StationHabitat { get; set; }

        [ExcelColumn("StationWaterBody")]
        public string StationWaterBody { get; set; }

        [ExcelColumn("StationFloodPlain")]
        public string StationFloodPlain { get; set; }

        [ExcelColumn("StationSubstrate")]
        public string StationSubstrate { get; set; }

        [ExcelColumn("StationStratification")]
        public string StationStratification { get; set; }

        [ExcelColumn("StationAquaticVegetation")]
        public string StationAquaticVegetation { get; set; }

        [ExcelColumn("StationTerrestrialVegetation")]
        public string StationTerrestrialVegetation { get; set; }

        [ExcelColumn("StationHydrology")]
        public string StationHydrology { get; set; }

        [ExcelColumn("StationGeology")]
        public string StationGeology { get; set; }

        [ExcelColumn("StationTimestamp")]
        public string StationTimestamp { get; set; }

        [ExcelColumn("StationRecordedBy")]
        public string StationRecordedBy { get; set; }

        [ExcelColumn("StationComments")]
        public string StationComments { get; set; }
    }
}
