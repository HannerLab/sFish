using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Hlug
{
    [ExcelSheet("Readings")]
    public class ExportReading
    {
        [ExcelColumn("ReadingProjectId")]
        public string ReadingProjectId { get; set; }

        [ExcelColumn("ReadingId")]
        public string ReadingId { get; set; }

        [ExcelColumn("ReadingStationId")]
        public string ReadingStationId { get; set; }

        [ExcelColumn("ReadingEquipmentId")]
        public string ReadingEquipmentId { get; set; }

        [ExcelColumn("ReadingChlorophyll")]
        public string ReadingChlorophyll { get; set; }

        [ExcelColumn("ReadingTurbidity")]
        public string ReadingTurbidity { get; set; }

        [ExcelColumn("ReadingSecchiTube")]
        public string ReadingSecchiTube { get; set; }

        [ExcelColumn("ReadingSuspendedSolids")]
        public string ReadingSuspendedSolids { get; set; }

        [ExcelColumn("ReadingDisolvedOxygen")]
        public string ReadingDisolvedOxygen { get; set; }

        [ExcelColumn("ReadingWaterConductivity")]
        public string ReadingWaterConductivity { get; set; }

        [ExcelColumn("ReadingWaterPh")]
        public string ReadingWaterPh { get; set; }

        [ExcelColumn("ReadingWaterTemp")]
        public string ReadingWaterTemp { get; set; }

        [ExcelColumn("ReadingWaterVelocity")]
        public string ReadingWaterVelocity { get; set; }

        [ExcelColumn("ReadingTimestamp")]
        public string ReadingTimestamp { get; set; }

        [ExcelColumn("ReadingCollector")]
        public string ReadingCollector { get; set; }

        [ExcelColumn("ReadingComments")]
        public string ReadingComments { get; set; }

        [ExcelColumn("ReadingRecordedBy")]
        public string ReadingRecordedBy { get; set; }
    }
}
