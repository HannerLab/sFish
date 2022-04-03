using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Hlug
{
    [ExcelSheet("Attachments")]
    public class ExportAttachment
    {
        [ExcelColumn("AttachmentProjectId")]
        public string AttachmentProjectId { get; set; }

        [ExcelColumn("AttachmentId")]
        public string AttachmentId { get; set; }

        [ExcelColumn("AttachmentFileName")]
        public string AttachmentFileName { get; set; }

        [ExcelColumn("AttachmentTimestamp")]
        public string AttachmentTimestamp { get; set; }

        [ExcelColumn("AttachmentComments")]
        public string AttachmentComments { get; set; }

        [ExcelColumn("AttachmentEventId")]
        public string AttachmentEventId { get; set; }

        [ExcelColumn("AttachmentReadingId")]
        public string AttachmentReadingId { get; set; }

        [ExcelColumn("AttachmentObservationId")]
        public string AttachmentObservationId { get; set; }

        [ExcelColumn("AttachmentStationId")]
        public string AttachmentStationId { get; set; }

        [ExcelColumn("AttachmentSiteId")]
        public string AttachmentSiteId { get; set; }

        [ExcelColumn("AttachmentRecordedBy")]
        public string AttachmentRecordedBy { get; set; }

    }
}
