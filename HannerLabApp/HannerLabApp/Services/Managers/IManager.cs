using System.Threading.Tasks;
using HannerLabApp.Models;

namespace HannerLabApp.Services.Managers
{
    public interface IManager<T> where T : ISavable
    {
        Task DeleteItemAsync(T item);
        Task AddItemAsync(T item);
        Task UpdateItemAsync(T item);
    }
}
