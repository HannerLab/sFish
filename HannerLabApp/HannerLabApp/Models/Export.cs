using System;
using System.Collections.Generic;
using HannerLabApp.Configuration;
using LiteDB;

namespace HannerLabApp.Models
{
    public class Export : ISavable
    { 
        public Guid Id { get; set; }
        public Guid ProjectId { get; set; }
        public Guid ActivityId { get; set; }
        public string RecordedBy { get; set; }
        public DateTime Timestamp { get; set; }
        public DateTime LastUpdated { get; set; }
        public string UserSpecifiedId { get; set; }
        public bool IsAdvancedShown { get; set; }

        public Activity Activity { get; set; }
        public Project Project { get; set; }
        public IList<Site> Sites { get; set; }
        public IList<Station> Stations { get; set; }
        public IList<Edna> Ednas { get; set; }
        public IList<Reading> Readings { get; set; }
        public IList<Observation> Observations { get; set; }
        public IList<Photo> Photos { get; set; }
        public IList<Equipment> Equipments { get; set; }

        [BsonIgnoreAttribute]
        public string FileBaseName => $"sFish_export_{this.Activity?.UserSpecifiedId}_{this.Activity?.Timestamp.ToString(Constants.ExportDateFormat)}";
    }
}
