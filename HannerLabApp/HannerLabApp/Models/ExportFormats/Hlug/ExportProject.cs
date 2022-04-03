using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Hlug
{
    [ExcelSheet("Projects")]
    public class ExportProject
    {
        [ExcelColumn("ProjectId")]
        public string ProjectId { get; set; }

        [ExcelColumn("ProjectName")]
        public string ProjectName { get; set; }

        [ExcelColumn("ProjectLeader")]
        public string ProjectLeader { get; set; }

        [ExcelColumn("ProjectContactEmail")]
        public string ProjectContactEmail { get; set; }

        [ExcelColumn("ProjectLeadOrganization")]
        public string ProjectLeadOrganization { get; set; }

        [ExcelColumn("ProjectDescription")]
        public string ProjectDescription { get; set; }

        [ExcelColumn("ProjectTimestamp")]
        public string ProjectTimestamp { get; set; }

        [ExcelColumn("ProjectRecordedBy")]
        public string ProjectRecordedBy { get; set; }
    }
}
