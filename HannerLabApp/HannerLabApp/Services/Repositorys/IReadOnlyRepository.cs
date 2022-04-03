using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HannerLabApp.Models;

namespace HannerLabApp.Services.Repositorys
{
    public interface IReadOnlyRepository<T> where T : ISavable
    {
        Task<T> GetItemAsync(Guid id);
        Task<IEnumerable<T>> GetItemsAsync();
        Task<IEnumerable<T>> GetItemsAsync(Guid projectId);
    }
}
