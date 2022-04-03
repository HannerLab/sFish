using System;

namespace HannerLabApp.Models
{
    // Things that can be saved to/from the data store/repository
    public interface ISavable
    {
        Guid Id { get; set; } // The internal ID used by the application
        Guid ProjectId { get; set; } // The ID (internal) of the project that this item is associated too
        Guid ActivityId { get; set; } // The exported activity that this item belongs to. If its empty or null then it was never exported.
        string RecordedBy { get; set; } // Who recorded this item
        DateTime Timestamp { get; set; } // The timestamp of when this item was recorded
        DateTime LastUpdated { get; set; } // The most recent time this item was changed or modified
        string UserSpecifiedId { get; set; } // An ID provided by the user. The internal ID is hidden to the user, this is the ID the user actually sees and uses when it says ID in the app.
        bool IsAdvancedShown { get; set; } // whether or not this model was saved using advance mode
    }
}
