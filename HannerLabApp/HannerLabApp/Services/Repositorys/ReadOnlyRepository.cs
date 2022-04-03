using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HannerLabApp.Models;
using LiteDB;
using LiteDB.Async;

namespace HannerLabApp.Services.Repositorys
{
    public class ReadOnlyRepository<T> : IReadOnlyRepository<T> where T: ISavable
    {
        private readonly ConnectionString _conString;

        public ReadOnlyRepository(IDbContext context)
        {
            _conString = context.GetConnectionString(true);
        }

        public async Task<T> GetItemAsync(Guid id)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<T>();

                return await col.FindOneAsync(x => x.Id == id);
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync()
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<T>();

                var items = await col.FindAllAsync();

                return items.ToList();
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Guid projectId)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<T>();

                var items = await col.FindAllAsync();

                return items.ToList().Where(x => x.ProjectId == projectId);
            }
        }
    }
}
