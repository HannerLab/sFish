using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Hlug
{
    [ExcelSheet("Observations")]
    public class ExportObservation
    {
        [ExcelColumn("ObservationProjectId")]
        public string ObservationProjectId { get; set; }

        [ExcelColumn("ObservationId")]
        public string ObservationId { get; set; }

        [ExcelColumn("ObservationTimestamp")]
        public string ObservationTimestamp { get; set; }

        [ExcelColumn("ObservationSiteId")]
        public string ObservationSiteId { get; set; }

        [ExcelColumn("ObservationStationId")]
        public string ObservationStationId { get; set; }

        [ExcelColumn("ObservationRecordedBy")]
        public string ObservationRecordedBy { get; set; }

        [ExcelColumn("ObservationComments")]
        public string ObservationComments { get; set; }

        [ExcelColumn("ObservationWind")]
        public string ObservationWind { get; set; }

        [ExcelColumn("ObservationPrecipitation")]
        public string ObservationPrecipitation { get; set; }

        [ExcelColumn("ObservationWeatherAirTemperature")]
        public string ObservationWeatherAirTemperature { get; set; }

        [ExcelColumn("ObservationCloudCoverLevel")]
        public string ObservationCloudCoverLevel { get; set; }

        [ExcelColumn("ObservationStormYesterday")]
        public string ObservationStormYesterday { get; set; }

        [ExcelColumn("ObservationCloudCover")]
        public string ObservationCloudCover { get; set; }

        [ExcelColumn("ObservationWildLife")]
        public string ObservationWildLife { get; set; }

        [ExcelColumn("ObservationAnthopogenic")]
        public string ObservationAnthopogenic { get; set; }

        [ExcelColumn("ObservationPhenology")]
        public string ObservationPhenology { get; set; }
    }
}
