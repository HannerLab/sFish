using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HannerLabApp.Models;

namespace HannerLabApp.Services.Repositorys
{
    public interface IRepository<T> where T : ISavable
    { 
        Task<T> GetItemAsync(Guid id);
        Task DeleteItemAsync(T item);
        Task AddItemAsync(T item);
        Task UpdateItemAsync(T item);
        Task<IEnumerable<T>> GetItemsAsync();
        Task<IEnumerable<T>> GetItemsAsync(Guid projectId);
    }
}
