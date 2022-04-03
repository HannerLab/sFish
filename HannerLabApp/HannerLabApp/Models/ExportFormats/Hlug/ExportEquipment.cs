using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Hlug
{
    [ExcelSheet("Equipments")]
    public class ExportEquipment
    {
        [ExcelColumn("EquipmentProjectId")]
        public string EquipmentProjectId { get; set; }

        [ExcelColumn("EquipmentId")]
        public string EquipmentId { get; set; }

        [ExcelColumn("EquipmentCategory")]
        public string EquipmentCategory { get; set; }

        [ExcelColumn("EquipmentVendor")]
        public string EquipmentVendor { get; set; }

        [ExcelColumn("EquipmentManufacturer")]
        public string EquipmentManufacturer { get; set; }

        [ExcelColumn("EquipmentSerialNumber")]
        public string EquipmentSerialNumber { get; set; }

        [ExcelColumn("EquipmentDescription")]
        public string EquipmentDescription { get; set; }

        [ExcelColumn("EquipmentModel")]
        public string EquipmentModel { get; set; }

        [ExcelColumn("EquipmentNotes")]
        public string EquipmentNotes { get; set; }

        [ExcelColumn("EquipmentParameters")]
        public string EquipmentParameters { get; set; }

        [ExcelColumn("EquipmentName")]
        public string EquipmentName { get; set; }

        [ExcelColumn("EquipmentTimestamp")]
        public string EquipmentTimestamp { get; set; }

        [ExcelColumn("EquipmentRecordedBy")]
        public string EquipmentRecordedBy { get; set; }

        [ExcelColumn("EquipmentUnitDepth")]
        public string EquipmentUnitDepth { get; set; }

        [ExcelColumn("EquipmentUnitOffshoreDistance")]
        public string EquipmentUnitOffshoreDistance { get; set; }

        [ExcelColumn("EquipmentUnitPressure")]
        public string EquipmentUnitPressure { get; set; }

        [ExcelColumn("EquipmentUnitVolume")]
        public string EquipmentUnitVolume { get; set; }

        [ExcelColumn("EquipmentUnitTime")]
        public string EquipmentUnitTime { get; set; }

        [ExcelColumn("EquipmentUnitVelocity")]
        public string EquipmentUnitVelocity { get; set; }

        [ExcelColumn("EquipmentUnitTemperature")]
        public string EquipmentUnitTemperature { get; set; }

        [ExcelColumn("EquipmentUnitConductivity")]
        public string EquipmentUnitConductivity { get; set; }

        [ExcelColumn("EquipmentUnitDissolvedOxygen")]
        public string EquipmentUnitDissolvedOxygen { get; set; }

        [ExcelColumn("EquipmentUnitSuspendedSolids")]
        public string EquipmentUnitSuspendedSolids { get; set; }

        [ExcelColumn("EquipmentUnitSecchi")]
        public string EquipmentUnitSecchi { get; set; }

        [ExcelColumn("EquipmentUnitTurbidity")]
        public string EquipmentUnitTurbidity { get; set; }

        [ExcelColumn("EquipmentUnitChlorophyll")]
        public string EquipmentUnitChlorophyll { get; set; }

        [ExcelColumn("EquipmentUnitGps")]
        public string EquipmentUnitGps { get; set; }
    }
}
