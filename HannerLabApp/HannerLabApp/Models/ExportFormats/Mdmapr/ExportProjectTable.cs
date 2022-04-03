using HannerLabApp.Extensions;

namespace HannerLabApp.Models.ExportFormats.Mdmapr
{
    /// <summary>
    /// Corresponds to the "project_Table" sheet in MDMAPR.
    /// </summary>
    [ExcelSheet("project_Table")]
    public class ExportProjectTable
    {
        [ExcelColumn("projectID")]
        public string ProjectId { get; set; }  = string.Empty;

        [ExcelColumn("projectCreationDate")]
        public string ProjectCreationDate { get; set; }  = string.Empty;

        [ExcelColumn("projectName")]
        public string ProjectName { get; set; }  = string.Empty;

        [ExcelColumn("projectRecordedBy")]
        public string ProjectRecordedBy { get; set; }  = string.Empty;

        [ExcelColumn("projectOwner")]
        public string ProjectOwner { get; set; }  = string.Empty;

        [ExcelColumn("projectContactEmail")]
        public string ProjectContactEmail { get; set; }  = string.Empty;

        [ExcelColumn("projectDescription")]
        public string ProjectDescription { get; set; }  = string.Empty;

        [ExcelColumn("InstitutionID")]
        public string InstitutionId { get; set; }  = string.Empty;

        [ExcelColumn("projectDataNotes")]
        public string ProjectDataNotes { get; set; }  = string.Empty;

        [ExcelColumn("geographicRegionID")]
        public string GeographicRegionId { get; set; }  = string.Empty;

        [ExcelColumn("continent")]
        public string Continent { get; set; }  = string.Empty;

        [ExcelColumn("country")]
        public string Country { get; set; }  = string.Empty;

        [ExcelColumn("stateProvince")]
        public string StateProvince { get; set; }  = string.Empty;

        [ExcelColumn("municipality")]
        public string Municipality { get; set; }  = string.Empty;

        [ExcelColumn("siteID")]
        public string SiteId { get; set; }  = string.Empty;

        [ExcelColumn("locality")]
        public string Locality { get; set; }  = string.Empty;

        [ExcelColumn("estimatedPerimeter")]
        public string EstimatedPerimeter { get; set; }  = string.Empty;

        [ExcelColumn("estimatedSurfaceArea(m2)")]
        public string EstimatedSurfaceArea { get; set; }  = string.Empty;

        [ExcelColumn("siteType")]
        public string SiteType { get; set; }  = string.Empty;

        [ExcelColumn("siteLength(m2)")]
        public string SiteLengthM2 { get; set; }  = string.Empty;

        [ExcelColumn("stationID")]
        public string StationId { get; set; }  = string.Empty;

        [ExcelColumn("stationName")]
        public string StationName { get; set; }  = string.Empty;

        [ExcelColumn("decimalLongitude")]
        public string DecimalLongitude { get; set; }  = string.Empty;

        [ExcelColumn("decimalLatitude")]
        public string DecimalLatitude { get; set; }  = string.Empty;
    }
}
