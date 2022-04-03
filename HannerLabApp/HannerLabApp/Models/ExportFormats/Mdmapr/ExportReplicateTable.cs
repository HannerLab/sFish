// ReSharper disable InconsistentNaming

using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Mdmapr
{
    [ExcelSheet("replicate_Table")]
    public class ExportReplicateTable
    {
        [ExcelColumn("replicateID")]
        public string replicateID { get; set; }  = string.Empty;

        [ExcelColumn("stationID")]
        public string stationID { get; set; }  = string.Empty;

        [ExcelColumn("collectorName")]
        public string collectorName { get; set; }  = string.Empty;

        [ExcelColumn("replicateName")]
        public string replicateName { get; set; }  = string.Empty;

        [ExcelColumn("collectionDate")]
        public string collectionDate { get; set; }  = string.Empty;

        [ExcelColumn("collectionTime")]
        public string collectionTime { get; set; }  = string.Empty;

        [ExcelColumn("storageID")]
        public string storageID { get; set; }  = string.Empty;

        [ExcelColumn("DateOfStorage")]
        public string DateOfStorage { get; set; }  = string.Empty;

        [ExcelColumn("methodOfStorage")]
        public string methodOfStorage { get; set; }  = string.Empty;

        [ExcelColumn("minimumElevationInMeters")]
        public string minimumElevationInMeters { get; set; }  = string.Empty;

        [ExcelColumn("maximumElevationInMeters")]
        public string maximumElevationInMeters { get; set; }  = string.Empty;

        [ExcelColumn("verbatimElevation")]
        public string verbatimElevation { get; set; }  = string.Empty;

        [ExcelColumn("minimumDepthInMeters")]
        public string minimumDepthInMeters { get; set; }  = string.Empty;

        [ExcelColumn("maximumDepthInMeters")]
        public string maximumDepthInMeters { get; set; }  = string.Empty;

        [ExcelColumn("verbatimDepth")]
        public string verbatimDepth { get; set; }  = string.Empty;

        [ExcelColumn("flowRate(m/s)")]
        public string flowRatems { get; set; }  = string.Empty;

        [ExcelColumn("filterType")]
        public string filterType { get; set; }  = string.Empty;

        [ExcelColumn("filtrationDuration(mins)")]
        public string filtrationDurationmins { get; set; }  = string.Empty;

        [ExcelColumn("volumeFiltered")]
        public string volumeFiltered { get; set; }  = string.Empty;

        [ExcelColumn("processLocation")]
        public string processLocation { get; set; }  = string.Empty;

        [ExcelColumn("replicationNumber")]
        public string replicationNumber { get; set; }  = string.Empty;

        [ExcelColumn("riparianVegetationPercentageCover")]
        public string riparianVegetationPercentageCover { get; set; }  = string.Empty;

        [ExcelColumn("dissolvedOxygen(mg/L)")]
        public string dissolvedOxygenmgL { get; set; }  = string.Empty;

        [ExcelColumn("waterTemperature(C)")]
        public string waterTemperatureC { get; set; }  = string.Empty;

        [ExcelColumn("pH")]
        public string pH { get; set; }  = string.Empty;

        [ExcelColumn("TSS(mg/L)")]
        public string TSSmgL { get; set; }  = string.Empty;

        [ExcelColumn("EC(uS/cm)")]
        public string ECuScm { get; set; }  = string.Empty;

        [ExcelColumn("turbidity(NTU)")]
        public string turbidityNTU { get; set; }  = string.Empty;

        [ExcelColumn("discharge")]
        public string discharge { get; set; }  = string.Empty;

        [ExcelColumn("tide")]
        public string tide { get; set; }  = string.Empty;

        [ExcelColumn("chlorophyl")]
        public string chlorophyl { get; set; }  = string.Empty;

        [ExcelColumn("salinity(ppt)")]
        public string salinityPpt { get; set; }  = string.Empty;

        [ExcelColumn("contaminants(ng/g)")]
        public string contaminantsngg { get; set; }  = string.Empty;

        [ExcelColumn("traceMetals(mg/kg)")]
        public string traceMetalsmgkg { get; set; }  = string.Empty;

        [ExcelColumn("organicContent(%)")]
        public string organicContentpercent { get; set; }  = string.Empty;

        [ExcelColumn("microbialActivity")]
        public string microbialActivity { get; set; }  = string.Empty;

        [ExcelColumn("grainSize")]
        public string grainSize { get; set; }  = string.Empty;

        [ExcelColumn("replicateDataNotes")]
        public string replicateDataNotes { get; set; }  = string.Empty;

        [ExcelColumn("extractID")]
        public string extractID { get; set; }  = string.Empty;

        [ExcelColumn("extractName")]
        public string extractName { get; set; }  = string.Empty;

        [ExcelColumn("analyst")]
        public string analyst { get; set; }  = string.Empty;

        [ExcelColumn("extractionDate")]
        public string extractionDate { get; set; }  = string.Empty;

        [ExcelColumn("extractionTime")]
        public string extractionTime { get; set; }  = string.Empty;

        [ExcelColumn("location")]
        public string location { get; set; }  = string.Empty;

        [ExcelColumn("extractionMethod")]
        public string extractionMethod { get; set; }  = string.Empty;

        [ExcelColumn("methodCitation")]
        public string methodCitation { get; set; }  = string.Empty;

        [ExcelColumn("extractionNotes")]
        public string extractionNotes { get; set; }  = string.Empty;

        [ExcelColumn("tubePlateID")]
        public string tubePlateID { get; set; }  = string.Empty;

        [ExcelColumn("frozen")]
        public string frozen { get; set; }  = string.Empty;

        [ExcelColumn("fixed")]
        public string fixedFixed { get; set; }  = string.Empty;

        [ExcelColumn("dnaStorageLocation")]
        public string dnaStorageLocation { get; set; }  = string.Empty;

        [ExcelColumn("extractMethodOfStorage")]
        public string extractMethodOfStorage { get; set; }  = string.Empty;

        [ExcelColumn("dnaVolume")]
        public string dnaVolume { get; set; }  = string.Empty;

        [ExcelColumn("quantificationMethod")]
        public string quantificationMethod { get; set; }  = string.Empty;

        [ExcelColumn("concentration(ng/ul)")]
        public string concentrationngul { get; set; }  = string.Empty;
    }
}
