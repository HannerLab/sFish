using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HannerLabApp.Models;
using HannerLabApp.Services.Repositorys;
using HannerLabApp.ViewModels.EdnaViewModels;
using HannerLabApp.ViewModels.EquipmentViewModels;
using HannerLabApp.ViewModels.ObservationViewModels;
using HannerLabApp.ViewModels.PhotoViewModels;
using HannerLabApp.ViewModels.ReadingViewModels;
using HannerLabApp.ViewModels.SiteViewModels;
using HannerLabApp.ViewModels.StationViewModels;
using Microcharts;
using SkiaSharp;
using TinyMvvm;

namespace HannerLabApp.ViewModels.ProjectViewModels
{
    public class ProjectInfoViewModel : ViewModelBase
    {
        private readonly IReadOnlyRepository<Site> _siteRepo;
        private readonly IReadOnlyRepository<Station> _stationRepo;
        private readonly IReadOnlyRepository<Edna> _ednaRepo;
        private readonly IReadOnlyRepository<Observation> _observationRepo;
        private readonly IReadOnlyRepository<Reading> _readingRepo;
        private readonly IReadOnlyRepository<Photo> _photoRepo;
        private readonly IReadOnlyRepository<Equipment> _equipmentRepo;

        private static readonly string[] Colors = new string[]
        {
            "#2c3e50",
            "#77d065",
            "#b455b6",
            "#3498db",
            "#000000",
            "#000000"
        };

        private Chart _samplesChart;
        private Chart _otherChart;

        private string _name;
        private string _description;
        private string _id;
        private string _institution;
        private string _contactEmail;
        private string _lastSynced;
        private string _defaultProjectRecorders;


        public string DefaultProjectRecorders
        {
            get => _defaultProjectRecorders;
            set => Set(ref _defaultProjectRecorders, value);
        }

        public string Institution
        {
            get => _institution;
            private set => Set(ref _institution, value);
        }

        public string ContactEmail
        {
            get => _contactEmail;
            private set => Set(ref _contactEmail, value);
        }

        public string Name
        {
            get => _name;
            private set => Set(ref _name, value);
        }

        public string Description
        {
            get => _description;
            private set => Set(ref _description, value);
        }

        public string Id
        {
            get => _id;
            private set => Set(ref _id, value);
        }

        public Chart SamplesChart
        {
            get => _samplesChart;
            private set => Set(ref _samplesChart, value);
        }

        public Chart OtherChart
        {
            get => _otherChart;
            private set => Set(ref _otherChart, value);
        }


        public ProjectInfoViewModel(IReadOnlyRepository<Site> siteRepo, IReadOnlyRepository<Station> stationRepo, IReadOnlyRepository<Edna> ednaRepo, IReadOnlyRepository<Observation> observationRepo, IReadOnlyRepository<Reading> readingRepo, IReadOnlyRepository<Photo> photoRepo, IReadOnlyRepository<Equipment> equipmentRepo)
        {
            _siteRepo = siteRepo;
            _stationRepo = stationRepo;
            _readingRepo = readingRepo;
            _ednaRepo = ednaRepo;
            _observationRepo = observationRepo;
            _photoRepo = photoRepo;
            _equipmentRepo = equipmentRepo;
        }

        public string LastSynced
        {
            get => _lastSynced;
            set => Set(ref _lastSynced, value);
        }


        /// <summary>
        /// Populates all the stats for a given project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        public async Task LoadProjectAsync(Project project)
        {
            if (project == null) return;

            // General
            {
                Name = project.Name;
                Description = project.Description;
                Id = project.UserSpecifiedId;
                ContactEmail = project.ContactEmail;
                Institution = project.Institution;
                DefaultProjectRecorders = project.RecordedBy;
                LastSynced = App.AppSettings.LastSyncTime?.ToString() ?? string.Empty;
            }
            
            // Samples plot
            {
                var entries = new List<ChartEntry>();

                // Ednas
                {
                    var count = (await _ednaRepo.GetItemsAsync(project.Id)).Count(x => x.ActivityId == Guid.Empty);
                    if (count > 0)
                    {
                        entries.Add(new ChartEntry(count)
                        {
                            Label = $"{EdnaViewModel.TitleBaseStatic}(s)",
                            ValueLabel = count.ToString(),
                            Color = SKColor.Parse(Colors[0])
                        });
                    }
                }

                // Observations
                {
                    var count = (await _observationRepo.GetItemsAsync(project.Id)).Count(x => x.ActivityId == Guid.Empty);
                    if (count > 0)
                    {
                        entries.Add(new ChartEntry(count)
                        {
                            Label = $"{ObservationViewModel.TitleBaseStatic}(s)",
                            ValueLabel = count.ToString(),
                            Color = SKColor.Parse(Colors[1])
                        });
                    }
                }

                // Readings
                {
                    var count = (await _readingRepo.GetItemsAsync(project.Id)).Count(x => x.ActivityId == Guid.Empty);
                    if (count > 0)
                    {
                        entries.Add(new ChartEntry(count)
                        {
                            Label = $"{ReadingViewModel.TitleBaseStatic}(s)",
                            ValueLabel = count.ToString(),
                            Color = SKColor.Parse(Colors[2])
                        });
                    }
                }

                // Photos
                {
                    var count = (await _photoRepo.GetItemsAsync(project.Id)).Count(x => x.ActivityId == Guid.Empty);
                    if (count > 0)
                    {
                        entries.Add(new ChartEntry(count)
                        {
                            Label = $"{PhotoViewModel.TitleBaseStatic}(s)",
                            ValueLabel = count.ToString(),
                            Color = SKColor.Parse(Colors[3])
                        });
                    }
                }


                SamplesChart = new BarChart { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal};
            }

            // Other chart
            {
                var entries = new List<ChartEntry>();

                // Sites
                {
                    var count = (await _siteRepo.GetItemsAsync(project.Id)).Count();
                    if (count > 0)
                    {
                        entries.Add(new ChartEntry(count)
                        {
                            Label = $"{SiteViewModel.TitleBaseStatic}(s)",
                            ValueLabel = count.ToString(),
                            Color = SKColor.Parse(Colors[3])
                        });
                    }
                }

                // Stations
                {
                    var count = (await _stationRepo.GetItemsAsync(project.Id)).Count();
                    if (count > 0)
                    {
                        entries.Add(new ChartEntry(count)
                        {
                            Label = $"{StationViewModel.TitleBaseStatic}(s)",
                            ValueLabel = count.ToString(),
                            Color = SKColor.Parse(Colors[4])
                        });
                    }
                }

                // Equipment
                {
                    var count = (await _equipmentRepo.GetItemsAsync(project.Id)).Count();
                    if (count > 0)
                    {
                        entries.Add(new ChartEntry(count)
                        {
                            Label = $"{EquipmentViewModel.TitleBaseStatic}(s)",
                            ValueLabel = count.ToString(),
                            Color = SKColor.Parse(Colors[5])
                        });
                    }
                }

                OtherChart = new BarChart { Entries = entries, LabelOrientation = Orientation.Horizontal, ValueLabelOrientation = Orientation.Horizontal };
            }
        }
    }
}
