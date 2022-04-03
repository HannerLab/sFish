namespace HannerLabApp.Models
{
    public interface ISample : ISavable
    {
        string Notes { get; set; } // General notes or comments on this sample
        Station Station { get; set; } // The station that this sample was taken from. Samples can ONLY be taken at specific stations
        string CollectedBy { get; set; } // Who actually collected the sample. Differs from the recorder. The collector is who is actually using the equipment.
        string Name { get; set; } // Human readable names may optionally be assigned to a sample. Defaults to ID (user specified).
        double? Depth { get; set; } // The depth at which this sample was taken from. (All samples are taken from water).
        double? OffshoreDistance { get; set; } // How far from shore was this sample taken from
        Equipment Equipment { get; set; } // Actual device object
    }
}
