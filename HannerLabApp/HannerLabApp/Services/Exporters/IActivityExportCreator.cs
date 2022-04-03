using System.Threading.Tasks;
using HannerLabApp.Models;

namespace HannerLabApp.Services.Exporters
{
    public interface IActivityExportCreator
    {
        Task<Export> CreateActivityExportAsync(Activity activity);
        Task SaveExportAsync(Export export);
        Task TagItemsAsExportedAsync(Export export);
    }
}
