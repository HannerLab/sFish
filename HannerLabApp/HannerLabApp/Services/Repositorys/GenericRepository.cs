using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HannerLabApp.Models;
using LiteDB;
using LiteDB.Async;

namespace HannerLabApp.Services.Repositorys
{
    public class GenericRepository<T> : IRepository<T> where T : ISavable
    {

        private readonly ConnectionString _conString;

        public GenericRepository(IDbContext context)
        {
            this._conString = context.GetConnectionString(false);
        }

        public async Task AddItemAsync(T item)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<T>();

                await col.InsertAsync(item);

                await col.EnsureIndexAsync(x => x.Id);
                await col.EnsureIndexAsync(y => y.ProjectId);

                //db.Dispose();
            }
        }

        public async Task DeleteItemAsync(T item)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<T>();

                await col.DeleteAsync(item.Id);

                //db.Dispose();
            }
        }

        public async Task<T> GetItemAsync(Guid id)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<T>();

                var ret = await col.FindOneAsync(x => x.Id == id);

                //db.Dispose();

                return ret;
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<T>();

                var items = await col.FindAllAsync();

                //db.Dispose();

                return items.ToList();
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Guid projectId)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<T>();

                var items = await col.FindAllAsync();

                //db.Dispose();

                return items.ToList().Where(x => x.ProjectId == projectId);
            }
        }

        public async Task UpdateItemAsync(T item)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<T>();

                await col.UpdateAsync(item);

                //db.Dispose();
            }
            
        }
    }
}
