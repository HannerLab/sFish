using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using HannerLabApp.Configuration;
using HannerLabApp.Extensions;
using HannerLabApp.Models;
using HannerLabApp.Models.ExportFormats.Hlug;
using HannerLabApp.Models.ExportFormats.Mdmapr;
using HannerLabApp.Services.Managers;
using HannerLabApp.Services.Media;
using HannerLabApp.Services.Repositorys;
using HannerLabApp.Utils;

namespace HannerLabApp.Services.Exporters
{
    /// <summary>
    /// The ActivityExportCreator handles creating an export package (contains all data for the activity) from an activity item.
    ///
    /// </summary>
    public class ActivityExportCreator : IActivityExportCreator
    {
        private readonly IReadOnlyRepository<Project> _projectRepo;
        private readonly IReadOnlyRepository<Site> _siteRepo;
        private readonly IReadOnlyRepository<Station> _stationRepo;
        private readonly IReadOnlyRepository<Edna> _ednaRepo;
        private readonly IReadOnlyRepository<Observation> _observationRepo;
        private readonly IReadOnlyRepository<Reading> _readingRepo;
        private readonly IReadOnlyRepository<Photo> _photoRepo;
        private readonly IReadOnlyRepository<Equipment> _equipmentRepo;

        public ActivityExportCreator()
        {
            _projectRepo = App.AppContainer.Resolve<IReadOnlyRepository<Project>>();
            _siteRepo = App.AppContainer.Resolve<IReadOnlyRepository<Site>>();
            _stationRepo = App.AppContainer.Resolve<IReadOnlyRepository<Station>>();
            _readingRepo = App.AppContainer.Resolve<IReadOnlyRepository<Reading>>();
            _ednaRepo = App.AppContainer.Resolve<IReadOnlyRepository<Edna>>();
            _observationRepo = App.AppContainer.Resolve<IReadOnlyRepository<Observation>>();
            _equipmentRepo = App.AppContainer.Resolve<IReadOnlyRepository<Equipment>>();

            // Do not actually load photos in memory 
            _photoRepo = new ReadOnlyRepository<Photo>(App.AppContainer.Resolve<IDbContext>());
        }

        /// <summary>
        /// Takes an activity (activity meta data) and generates an export activity 
        /// </summary>
        /// <param name="activity"></param>
        /// <returns></returns>
        public async Task<Export> CreateActivityExportAsync(Activity activity)
        {
            Export export = new Export();

            export.Activity = activity;

            // Add Id's to match
            export.Id = activity.Id;
            export.ProjectId = activity.ProjectId;
            export.ActivityId = activity.ActivityId;

            // Add project info
            export.Project = await _projectRepo.GetItemAsync(activity.ProjectId);

            // eDNAs - Add all between dates
            export.Ednas = (await _ednaRepo.GetItemsAsync())
                .Where(x => x.ActivityId == Guid.Empty)
                .Where(x => x.ProjectId == activity.ProjectId)
                .Where(x => x.Timestamp >= activity.TimestampStart && x.Timestamp <= activity.TimestampEnd)
                .ToList();

            // Readings - Add all between dates
            export.Readings = (await _readingRepo.GetItemsAsync())
                .Where(x => x.ActivityId == Guid.Empty)
                .Where(x => x.ProjectId == activity.ProjectId)
                .Where(x => x.Timestamp >= activity.TimestampStart && x.Timestamp <= activity.TimestampEnd)
                .ToList();

            // Observations - Add all between dates
            export.Observations = (await _observationRepo.GetItemsAsync())
                .Where(x => x.ActivityId == Guid.Empty)
                .Where(x => x.ProjectId == activity.ProjectId)
                .Where(x => x.Timestamp >= activity.TimestampStart && x.Timestamp <= activity.TimestampEnd)
                .ToList();

            // Photos - Add all between dates - DOES NOT INCLUDE DATA LOADED. ONLY ID RELATING TO FILE IN FILE STORE.
            export.Photos = (await _photoRepo.GetItemsAsync())
                .Where(x => x.ActivityId == Guid.Empty)
                .Where(x => x.ProjectId == activity.ProjectId)
                .Where(x => x.Timestamp >= activity.TimestampStart && x.Timestamp <= activity.TimestampEnd)
                .ToList();


            // Equipment - Add all Equipments
            export.Equipments = (await _equipmentRepo.GetItemsAsync())
                .Where(x => x.ProjectId == activity.ProjectId).ToList();

            // Sites - Add all sites
            export.Sites = (await _siteRepo.GetItemsAsync())
                .Where(x => x.ProjectId == activity.ProjectId).ToList();

            // Stations - Add all stations
            export.Stations = (await _stationRepo.GetItemsAsync())
                .Where(x => x.ProjectId == activity.ProjectId).ToList();


            // Infer missing data
            if (string.IsNullOrEmpty(export.Activity.LeadMember))
                export.Activity.LeadMember = export.Project.Owner;

            if (string.IsNullOrEmpty(export.Activity.Organization))
                export.Activity.Organization = export.Project.Institution;

            return export;
        }



        /// <summary>
        /// Updated database and adds the export activity id to each of the required items. Equipment, sites, stations are never tagged as exported, only samples.
        /// </summary>
        /// <param name="export"></param>
        /// <returns></returns>
        public async Task TagItemsAsExportedAsync(Export export)
        {
            if (export == null) return;

            // Ednas
            {
                var manager = App.AppContainer.Resolve<IManager<Edna>>();
                foreach (var edna in export.Ednas)
                {
                    edna.ActivityId = export.Activity.Id;
                    await manager.UpdateItemAsync(edna);
                }
            }

            // Readings
            {
                var manager = App.AppContainer.Resolve<IManager<Reading>>();
                foreach (var reading in export.Readings)
                {
                    reading.ActivityId = export.Activity.Id;
                    await manager.UpdateItemAsync(reading);
                }
            }

            // Photos
            {
                var manager = App.AppContainer.Resolve<IManager<Photo>>();
                foreach (var photo in export.Photos)
                {
                    photo.ActivityId = export.Activity.Id;
                    await manager.UpdateItemAsync(photo);
                }
            }

            // Observations
            {
                var manager = App.AppContainer.Resolve<IManager<Observation>>();
                foreach (var observation in export.Observations)
                {
                    observation.ActivityId = export.Activity.Id;
                    await manager.UpdateItemAsync(observation);
                }
            }
        }


        /// <summary>
        /// Saves the exported activity export object to the database
        /// </summary>
        /// <param name="export"></param>
        /// <returns></returns>
        public async Task SaveExportAsync(Export export)
        {
            var exportManager = App.AppContainer.Resolve<IManager<Export>>();
            await exportManager.AddItemAsync(export);
        }


        /// <summary>
        /// Generates an Excel file in HLUG format of an export activity
        /// </summary>
        /// <param name="export">The Excel file in base64 format</param>
        /// <returns></returns>
        public static string GenerateExcelSheetAsync(Export export)
        {
            // Single activity
            List<ExportActivity> activityTable = new List<ExportActivity>();
            {
                var row = new ExportActivity();

                row.ActivityId = export.Activity.UserSpecifiedId ?? string.Empty;
                row.ActivityComments = export.Activity.Notes ?? string.Empty;
                row.ActivityDescription = export.Activity.Description ?? string.Empty;
                row.ActivityEndTimestamp = export.Activity.TimestampEnd.ToString() ?? string.Empty;
                row.ActivityExportTimestamp = export.Activity.Timestamp.ToString(Constants.ExportDateTimeFormat) ?? string.Empty;
                row.ActivityStartTimestamp = export.Activity.TimestampStart.ToString() ?? string.Empty;
                row.ActivityFieldCrew = export.Activity.OtherMembers ?? string.Empty;
                row.ActivityLead = export.Activity.LeadMember ?? string.Empty;
                row.ActivityOrganization = export.Activity.Organization ?? string.Empty;
                row.ActivityProjectId = export.Project.UserSpecifiedId ?? string.Empty;
                row.ActivityRecordedBy = export.Activity.RecordedBy ?? string.Empty;
                row.ActivitySiteId = export.Sites.FirstOrDefault()?.UserSpecifiedId ?? string.Empty;

                activityTable.Add(row);
            }

            // Photos/attachments
            List<ExportAttachment> attachmentTable = new List<ExportAttachment>();
            foreach (var photo in export.Photos)
            {
                var row = new ExportAttachment();

                row.AttachmentComments = photo.Notes ?? string.Empty;
                row.AttachmentEventId = photo.Edna?.UserSpecifiedId ?? string.Empty;
                row.AttachmentFileName = photo.GetFileName() ?? string.Empty;
                row.AttachmentId = Path.GetFileNameWithoutExtension(photo.GetFileName()) ?? string.Empty;
                row.AttachmentObservationId = photo.Observation?.UserSpecifiedId ?? string.Empty;
                row.AttachmentProjectId = export.Project?.UserSpecifiedId ?? string.Empty;
                row.AttachmentReadingId = photo.Reading?.UserSpecifiedId ?? string.Empty;
                row.AttachmentRecordedBy = photo.RecordedBy ?? string.Empty;
                row.AttachmentSiteId = photo.Site?.UserSpecifiedId ?? string.Empty;
                row.AttachmentStationId = photo.Station?.UserSpecifiedId ?? string.Empty;
                row.AttachmentTimestamp = photo.Timestamp.ToString(Constants.ExportDateTimeFormat) ?? string.Empty;

                attachmentTable.Add(row);
            }

            // Equipments
            List<ExportEquipment> equipmentTable = new List<ExportEquipment>();
            foreach (var e in export.Equipments)
            {
                var row = new ExportEquipment()
                {
                    EquipmentCategory = e.Category.GetDescription() ?? string.Empty,
                    EquipmentDescription = e.Description ?? string.Empty,
                    EquipmentName = e.Name ?? string.Empty,
                    EquipmentId = e.UserSpecifiedId ?? string.Empty,
                    EquipmentManufacturer = e.Manufacturer ?? string.Empty,
                    EquipmentModel = e.DeviceModel ?? string.Empty,
                    EquipmentNotes = e.Notes ?? string.Empty,
                    EquipmentParameters = e.Parameters ?? string.Empty,
                    EquipmentProjectId = export.Project.UserSpecifiedId ?? string.Empty,
                    EquipmentRecordedBy = e.RecordedBy ?? string.Empty,
                    EquipmentSerialNumber = e.SerialNumber ?? string.Empty,
                    EquipmentTimestamp = e.Timestamp.ToString(Constants.ExportDateTimeFormat) ?? string.Empty,
                    EquipmentUnitTemperature = e.UnitTemperature ?? string.Empty,
                    EquipmentUnitChlorophyll = e.UnitChlorophyll ?? string.Empty,
                    EquipmentUnitConductivity = e.UnitConductivity ?? string.Empty,
                    EquipmentUnitDepth = e.UnitDepth ?? string.Empty,
                    EquipmentUnitDissolvedOxygen = e.UnitDissolvedOxygen ?? string.Empty,
                    EquipmentUnitGps = e.UnitGps ?? string.Empty,
                    EquipmentUnitOffshoreDistance = e.UnitOffshoreDistance ?? string.Empty,
                    EquipmentUnitPressure = e.UnitPressure ?? string.Empty,
                    EquipmentUnitSecchi = e.UnitSecchi ?? string.Empty,
                    EquipmentUnitSuspendedSolids = e.UnitSuspendedSolids ?? string.Empty,
                    EquipmentUnitTime = e.UnitTimeFiltering ?? string.Empty,
                    EquipmentUnitTurbidity = e.UnitTurbidity ?? string.Empty,
                    EquipmentUnitVelocity = e.UnitVelocity ?? string.Empty,
                    EquipmentUnitVolume = e.UnitVolumeFiltered ?? string.Empty,
                    EquipmentVendor = e.Vendor ?? string.Empty,
                };

                equipmentTable.Add(row);
            }

            // Events/eDNAs
            List<ExportEvent> eventTable = new List<ExportEvent>();
            foreach (var e in export.Ednas)
            {
                var row = new ExportEvent()
                {
                    EventAverageFlowRate = e.FlowRate.ToString() ?? string.Empty,
                    EventCollector = e.CollectedBy ?? string.Empty,
                    EventComments = e.Notes ?? string.Empty,
                    EventEquipmentId = e.Equipment?.UserSpecifiedId ?? string.Empty,
                    EventId = e.UserSpecifiedId ?? string.Empty,
                    EventOffshoreDistance = e.OffshoreDistance.ToString() ?? string.Empty,
                    EventProjectId = export.Project?.UserSpecifiedId ?? string.Empty,
                    EventRecordedBy = e.RecordedBy ?? string.Empty,
                    EventSamplingDepth = e.Depth.ToString() ?? string.Empty,
                    EventSamplingVolume = e.VolumeFiltered.ToString() ?? string.Empty,
                    EventStationId = e.Station?.UserSpecifiedId ?? string.Empty,
                    EventTimeFiltering = e.TimeFiltering.ToString() ?? string.Empty,
                    EventTimestamp = e.Timestamp.ToString(Constants.ExportDateTimeFormat) ?? string.Empty
                };

                eventTable.Add(row);
            }

            // Observations
            List<ExportObservation> observationTable = new List<ExportObservation>();
            foreach (var o in export.Observations)
            {
                var row = new ExportObservation()
                {
                    ObservationAnthopogenic = o.Anthropogenic ?? string.Empty,
                    ObservationCloudCover = o.CloudCover.GetDescription() ?? string.Empty,
                    ObservationCloudCoverLevel = o.CloudCoverLevel.GetDescription() ?? string.Empty,
                    ObservationComments = o.Notes ?? string.Empty,
                    ObservationId = o.UserSpecifiedId ?? string.Empty,
                    ObservationPhenology = o.Phenology ?? string.Empty,
                    ObservationPrecipitation = o.PrecipitationLevel.GetDescription() ?? string.Empty,
                    ObservationProjectId = export.Project?.UserSpecifiedId ?? string.Empty,
                    ObservationRecordedBy = o.RecordedBy ?? string.Empty,
                    ObservationSiteId = o.Site?.UserSpecifiedId ?? string.Empty,
                    ObservationStationId = o.Station?.UserSpecifiedId ?? string.Empty,
                    ObservationStormYesterday = o.StormYesterday.GetDescription() ?? string.Empty,
                    ObservationTimestamp = o.Timestamp.ToString(Constants.ExportDateTimeFormat) ?? string.Empty,
                    ObservationWeatherAirTemperature = o.Temperature.ToString() ?? string.Empty,
                    ObservationWildLife = o.Wildlife ?? string.Empty,
                    ObservationWind = o.WindLevel.GetDescription() ?? string.Empty
                };

                observationTable.Add(row);
            }

            // Project
            List<ExportProject> projectTable = new List<ExportProject>();
            projectTable.Add(new ExportProject()
            {
                ProjectContactEmail = export.Project.ContactEmail ?? string.Empty,
                ProjectDescription = export.Project.Description ?? string.Empty,
                ProjectId = export.Project.UserSpecifiedId ?? string.Empty,
                ProjectLeadOrganization = export.Project.Institution ?? string.Empty,
                ProjectLeader = export.Project.Owner ?? string.Empty,
                ProjectName = export.Project.Name ?? string.Empty,
                ProjectRecordedBy = export.Project.RecordedBy ?? string.Empty,
                ProjectTimestamp = export.Project.Timestamp.ToString(Constants.ExportDateTimeFormat) ?? string.Empty,
            });


            // Readings
            List<ExportReading> readingTable = new List<ExportReading>();
            foreach (var r in export.Readings)
            {
                var row = new ExportReading()
                {
                    ReadingChlorophyll = r.Chlorophyll.ToString() ?? string.Empty,
                    ReadingCollector = r.CollectedBy ?? string.Empty,
                    ReadingComments = r.Notes ?? string.Empty,
                    ReadingDisolvedOxygen = r.DissolvedOxygen.ToString() ?? string.Empty,
                    ReadingEquipmentId = r.Equipment?.UserSpecifiedId ?? string.Empty,
                    ReadingId = r.UserSpecifiedId ?? string.Empty,
                    ReadingProjectId = export.Project?.UserSpecifiedId ?? string.Empty,
                    ReadingRecordedBy = r.RecordedBy ?? string.Empty,
                    ReadingSecchiTube = r.Secchi.ToString() ?? string.Empty,
                    ReadingStationId = r.Station?.UserSpecifiedId ?? string.Empty,
                    ReadingSuspendedSolids = r.SuspendedSolids.ToString() ?? string.Empty,
                    ReadingTimestamp = r.Timestamp.ToString(Constants.ExportDateTimeFormat) ?? string.Empty,
                    ReadingTurbidity = r.Turbidity.ToString() ?? string.Empty,
                    ReadingWaterConductivity = r.Conductivity.ToString() ?? string.Empty,
                    ReadingWaterPh = r.Ph.ToString() ?? string.Empty,
                    ReadingWaterTemp = r.Temperature.ToString() ?? string.Empty,
                    ReadingWaterVelocity = r.Velocity.ToString() ?? string.Empty
                };

                readingTable.Add(row);
            }

            // Sites
            List<ExportSite> siteTable = new List<ExportSite>();
            foreach (var s in export.Sites)
            {
                var row = new ExportSite()
                {
                    SiteId = s.UserSpecifiedId ?? string.Empty,
                    SiteComments = s.Notes ?? string.Empty,
                    SiteCountry = s.Country ?? string.Empty,
                    SiteDescription = s.Description ?? string.Empty,
                    SiteGeology = s.Description ?? string.Empty,
                    SiteHydrology = s.Hydrology ?? string.Empty,
                    SiteLocality = s.Locality ?? string.Empty,
                    SiteName = s.Name ?? string.Empty,
                    SiteProjectId = export.Project.UserSpecifiedId ?? string.Empty,
                    SiteProvince = s.StateProvince ?? string.Empty,
                    SiteRecordedBy = s.RecordedBy ?? string.Empty,
                    SiteRegion = s.Region ?? string.Empty,
                    SiteTimestamp = s.Timestamp.ToString(Constants.ExportDateTimeFormat) ?? string.Empty,
                    SiteWaterBody = s.WaterBody ?? string.Empty
                };

                siteTable.Add(row);
            }

            // Stations
            List<ExportStation> stationTable = new List<ExportStation>();
            foreach (var s in export.Stations)
            {
                var row = new ExportStation();

                row.StationAquaticVegetation = s.VegetationAquatic ?? string.Empty;
                row.StationComments = s.Notes ?? string.Empty;
                row.StationElevation = s.Elevation.ToString() ?? string.Empty;
                row.StationFloodPlain = s.FloodPlain ?? string.Empty;
                row.StationGeology = s.Geology ?? string.Empty;
                row.StationHabitat = s.Habitat ?? string.Empty;
                row.StationHydrology = s.Hydrology ?? string.Empty;
                row.StationId = s.UserSpecifiedId ?? string.Empty;
                row.StationLatitude = s.Latitude.ToString() ?? string.Empty;
                row.StationLongitude = s.Longitude.ToString() ?? string.Empty;
                row.StationProjectId = export.Project?.UserSpecifiedId ?? string.Empty;
                row.StationRecordedBy = s.RecordedBy ?? string.Empty;
                row.StationSiteId = s.Site?.UserSpecifiedId ?? string.Empty;
                row.StationStratification = s.Stratification ?? string.Empty;
                row.StationSubstrate = s.Substrate ?? string.Empty;
                row.StationTerrestrialVegetation = s.VegetationTerrestrial ?? string.Empty;
                row.StationTimestamp = s.Timestamp.ToString(Constants.ExportDateTimeFormat) ?? string.Empty;
                row.StationWaterBody = s.WaterBody ?? string.Empty;
                row.StationWayPoint = s.WayPoint ?? string.Empty;

                stationTable.Add(row);
            }


            var tables = new List<DataTable>()
            {
                ExcelTools.ListToDataTable(projectTable),
                ExcelTools.ListToDataTable(stationTable),
                ExcelTools.ListToDataTable(activityTable),
                ExcelTools.ListToDataTable(attachmentTable),
                ExcelTools.ListToDataTable(equipmentTable),
                ExcelTools.ListToDataTable(eventTable),
                ExcelTools.ListToDataTable(observationTable),
                ExcelTools.ListToDataTable(readingTable),
                ExcelTools.ListToDataTable(siteTable),
            };

            var excelFile64 = ExcelTools.GenerateExcelFileFromDataTables(tables);

            return excelFile64;
        }


        /// <summary>
        /// Generates an Excel file in MDMAPR format of an export activity
        /// </summary>
        /// <param name="export">The Excel file in base64 format</param>
        /// <returns></returns>
        public static string GenerateMdmaprExcelSheetAsync(Export export)
        {
            // project_Table sheet
            List<ExportProjectTable> projectTable = new List<ExportProjectTable>();
            foreach (var station in export.Stations)
            {
                var row = new ExportProjectTable();

                row.ProjectId = export.Project.UserSpecifiedId ?? string.Empty;
                row.ProjectCreationDate = export.Project.Timestamp.ToString(Constants.ExportDateTimeFormat) ?? string.Empty;
                row.ProjectName = export.Project.Name ?? string.Empty;
                row.ProjectRecordedBy = export.Project.RecordedBy ?? string.Empty;
                row.ProjectOwner = export.Project.Owner ?? string.Empty;
                row.ProjectDescription = export.Project.Description ?? string.Empty;
                row.InstitutionId = export.Project.Institution ?? string.Empty;
                row.ProjectDataNotes = export.Project.Notes ?? string.Empty;
                row.Country = station.Site.Country ?? string.Empty;
                row.StateProvince = station.Site.StateProvince ?? string.Empty;
                row.Locality = station.Site.Locality ?? string.Empty;
                row.SiteType = station.Site.WaterBody ?? string.Empty;
                row.SiteId = station.Site.UserSpecifiedId ?? string.Empty;
                row.StationId = station.UserSpecifiedId ?? string.Empty;
                row.StationName = station.Name ?? string.Empty;
                row.DecimalLongitude = station.Longitude.ToString() ?? string.Empty;
                row.DecimalLatitude = station.Latitude.ToString() ?? string.Empty;

                projectTable.Add(row);
            }

            // Replicate table
            List<ExportReplicateTable> replicateTable = new List<ExportReplicateTable>();
            foreach (var edna in export.Ednas)
            {
                var row = new ExportReplicateTable();

                row.replicateID = edna.UserSpecifiedId ?? string.Empty;
                row.stationID = edna.Station.UserSpecifiedId ?? string.Empty;
                row.collectorName = edna.CollectedBy ?? string.Empty;
                row.replicateName = edna.Name ?? string.Empty;
                row.collectionDate = edna.Timestamp.ToString(Constants.ExportDateFormat) ?? string.Empty;
                row.collectionTime = edna.Timestamp.ToString(Constants.ExportTimeFormat) ?? string.Empty;
                row.verbatimDepth = edna.Depth.ToString() ?? string.Empty;
                row.flowRatems = edna.FlowRate.ToString() ?? string.Empty;
                row.filtrationDurationmins = edna.TimeFiltering.ToString() ?? string.Empty;
                row.volumeFiltered = edna.VolumeFiltered.ToString() ?? string.Empty;
                row.verbatimDepth = edna.Depth.ToString() ?? string.Empty;

                row.verbatimElevation = edna.Station.Elevation.ToString() ?? string.Empty;

                replicateTable.Add(row);
            }

            foreach (var reading in export.Readings)
            {
                var row = new ExportReplicateTable();

                row.replicateID = reading.UserSpecifiedId ?? string.Empty;
                row.stationID = reading.Station.UserSpecifiedId ?? string.Empty;
                row.collectorName = reading.CollectedBy ?? string.Empty;
                row.replicateName = reading.Name ?? string.Empty;
                row.collectionDate = reading.Timestamp.ToString(Constants.ExportDateFormat) ?? string.Empty;
                row.collectionTime = reading.Timestamp.ToString(Constants.ExportTimeFormat) ?? string.Empty;
                row.verbatimDepth = reading.Depth.ToString() ?? string.Empty;
                row.dissolvedOxygenmgL = reading.DissolvedOxygen.ToString() ?? string.Empty;
                row.waterTemperatureC = reading.Temperature.ToString() ?? string.Empty;
                row.pH = reading.Ph.ToString() ?? string.Empty;
                row.TSSmgL = reading.SuspendedSolids.ToString() ?? string.Empty;
                row.ECuScm = reading.Conductivity.ToString() ?? string.Empty;
                row.turbidityNTU = reading.Turbidity.ToString() ?? string.Empty;
                row.chlorophyl = reading.Chlorophyll.ToString() ?? string.Empty;
                row.verbatimElevation = reading.Station.Elevation.ToString() ?? string.Empty;

                replicateTable.Add(row);
            }

            var projectDataTable = ExcelTools.ListToDataTable(projectTable);
            var replicateDataTable = ExcelTools.ListToDataTable(replicateTable);

            var excelFile64 = ExcelTools.GenerateExcelFileFromDataTables(new List<DataTable> { projectDataTable, replicateDataTable });

            return excelFile64;
        }


        /// <summary>
        /// Generates an activity export package from an export object. This will either be an excel file, or a zip file containing an excel sheet + pictures.
        /// </summary>
        /// <param name="export">The export object to package</param>
        /// <param name="includeAttachments">Whether or not to include pictures</param>
        /// <param name="useMdmaprFormat">Whether to use HLUG or MDMAPR format</param>
        /// <returns>The PATH of the export file</returns>
        public static async Task<string> GenerateExportPackageAsync(Export export, bool includeAttachments,
            bool useMdmaprFormat)
        {
            string excelFile64;
            string exportFilePath;

            if (useMdmaprFormat)
                excelFile64 = ActivityExportCreator.GenerateMdmaprExcelSheetAsync(export);
            else
                excelFile64 = ActivityExportCreator.GenerateExcelSheetAsync(export);

            // Check if successful
            if (string.IsNullOrEmpty(excelFile64))
            {
                return string.Empty;
            }

            if (includeAttachments)
            {
                // Temp dir where photos and data will be written to
                var saveDir = Path.Combine(Constants.TempDirectory, Guid.NewGuid() + "/");
                var mediaSaveDir = Path.Combine(saveDir, "media/");

                Directory.CreateDirectory(saveDir);
                Directory.CreateDirectory(mediaSaveDir);

                // Generate pictures
                {
                    // Write photos to temp dir
                    IPhotoStore photoStore = App.AppContainer.Resolve<IPhotoStore>();
                    foreach (var photo in export.Photos)
                    {
                        // Photo must be loaded again, as the photo object in the export doesn't include the actually photo data.
                        var photoFileResult = await photoStore.LoadPhotoAsync(photo.Id);

                        if (photo != null)
                        {
                            var fullSavePath = Path.Combine(mediaSaveDir, photoFileResult.FileName);

                            using (var stream = await photoFileResult.OpenReadAsync())
                            {
                                using (var newStream = File.OpenWrite(fullSavePath))
                                {
                                    await stream.CopyToAsync(newStream);
                                }
                            }
                        }
                    }
                }

                // Write excel file
                {
                    var exportSaveFileName = export.FileBaseName + ".xlsx";
                    
                    var fullSavePath = Path.Combine(saveDir, exportSaveFileName);
                    var bytes = Convert.FromBase64String(excelFile64);

                    using (var stream = new MemoryStream(bytes))
                    {
                        using (var newStream = File.OpenWrite(fullSavePath))
                        {
                            await stream.CopyToAsync(newStream);
                        }
                    }
                }

                // Zip contents of tmp dir and return that
                {
                    // Save zip to export 
                    var exportSaveFileName = export.FileBaseName + ".zip";

                    var fullSavePath = Path.Combine(Constants.ExportDirectory, exportSaveFileName);

                    var task = Task.Run(() => FileTools.ZipFolderContents(saveDir, fullSavePath));

                    var ret = await task;
                    if (!ret)
                    {
                        return string.Empty;
                    }

                    exportFilePath = fullSavePath;
                }
            }
            else
            {
                var saveDir = Path.Combine(Constants.TempDirectory, Guid.NewGuid() + "/");
                Directory.CreateDirectory(saveDir);

                var exportSaveFileName = export.FileBaseName + ".xlsx";
                
                var fullSavePath = Path.Combine(Constants.ExportDirectory, exportSaveFileName);

                // Write excel file
                var bytes = Convert.FromBase64String(excelFile64);

                using (var stream = new MemoryStream(bytes))
                {
                    using (var newStream = File.OpenWrite(fullSavePath))
                    {
                        await stream.CopyToAsync(newStream);
                    }
                }

                exportFilePath = fullSavePath;
            }


            // Check if exists, else return empty string
            if (File.Exists(exportFilePath))
                return exportFilePath;
            else
                return string.Empty;

        }
    }
}
