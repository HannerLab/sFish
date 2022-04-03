using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Hlug
{
    [ExcelSheet("Events")]
    public class ExportEvent
    {
        [ExcelColumn("EventProjectId")]
        public string EventProjectId { get; set; }

        [ExcelColumn("EventId")]
        public string EventId { get; set; }

        [ExcelColumn("EventStationId")]
        public string EventStationId { get; set; }

        [ExcelColumn("EventCollector")]
        public string EventCollector { get; set; }

        [ExcelColumn("EventRecordedBy")]
        public string EventRecordedBy { get; set; }

        [ExcelColumn("EventEquipmentId")]
        public string EventEquipmentId { get; set; }

        [ExcelColumn("EventTimeFiltering")]
        public string EventTimeFiltering { get; set; }

        [ExcelColumn("EventSamplingDepth")]
        public string EventSamplingDepth { get; set; }

        [ExcelColumn("EventOffshoreDistance")]
        public string EventOffshoreDistance { get; set; }

        [ExcelColumn("EventSamplingVolume")]
        public string EventSamplingVolume { get; set; }

        [ExcelColumn("EventAverageFlowRate")]
        public string EventAverageFlowRate { get; set; }

        [ExcelColumn("EventComments")]
        public string EventComments { get; set; }

        [ExcelColumn("EventTimestamp")]
        public string EventTimestamp { get; set; }
    }
}
