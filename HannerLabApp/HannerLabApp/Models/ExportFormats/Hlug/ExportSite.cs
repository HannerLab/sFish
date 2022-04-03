using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Hlug
{
    [ExcelSheet("Sites")]
    public class ExportSite
    {
        [ExcelColumn("SiteProjectId")]
        public string SiteProjectId { get; set; }

        [ExcelColumn("SiteId")]
        public string SiteId { get; set; }

        [ExcelColumn("SiteDescription")]
        public string SiteDescription { get; set; }

        [ExcelColumn("SiteName")]
        public string SiteName { get; set; }

        [ExcelColumn("SiteCountry")]
        public string SiteCountry { get; set; }

        [ExcelColumn("SiteProvince")]
        public string SiteProvince { get; set; }

        [ExcelColumn("SiteRegion")]
        public string SiteRegion { get; set; }

        [ExcelColumn("SiteLocality")]
        public string SiteLocality { get; set; }

        [ExcelColumn("SiteWaterBody")]
        public string SiteWaterBody { get; set; }

        [ExcelColumn("SiteHydrology")]
        public string SiteHydrology { get; set; }

        [ExcelColumn("SiteGeology")]
        public string SiteGeology { get; set; }

        [ExcelColumn("SiteComments")]
        public string SiteComments { get; set; }

        [ExcelColumn("SiteTimestamp")]
        public string SiteTimestamp { get; set; }

        [ExcelColumn("SiteRecordedBy")]
        public string SiteRecordedBy { get; set; }
    }
}
