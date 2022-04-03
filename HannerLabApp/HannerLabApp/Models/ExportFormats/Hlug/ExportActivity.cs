using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Hlug
{
    [ExcelSheet("Activities")]
    public class ExportActivity
    {
        [ExcelColumn("ActivityId")]
        public string ActivityId { get; set; }

        [ExcelColumn("ActivityProjectId")]
        public string ActivityProjectId { get; set; }

        [ExcelColumn("ActivitySiteId")]
        public string ActivitySiteId { get; set; }

        [ExcelColumn("ActivityStartTimestamp")]
        public string ActivityStartTimestamp { get; set; }

        [ExcelColumn("ActivityEndTimestamp")]
        public string ActivityEndTimestamp { get; set; }

        [ExcelColumn("ActivityOrganization")]
        public string ActivityOrganization { get; set; }

        [ExcelColumn("ActivityLead")]
        public string ActivityLead { get; set; }

        [ExcelColumn("ActivityFieldCrew")]
        public string ActivityFieldCrew { get; set; }

        [ExcelColumn("ActivityDescription")]
        public string ActivityDescription { get; set; }

        [ExcelColumn("ActivityComments")]
        public string ActivityComments { get; set; }

        [ExcelColumn("ActivityExportTimestamp")]
        public string ActivityExportTimestamp { get; set; }

        [ExcelColumn("ActivityRecordedBy")]
        public string ActivityRecordedBy { get; set; }
    }
}
