using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HannerLabApp.Models;
using HannerLabApp.Services.Media;
using LiteDB;
using LiteDB.Async;

namespace HannerLabApp.Services.Repositorys
{
    public class PhotoReadOnlyRepository : IReadOnlyRepository<Photo>
    {
        private readonly IPhotoStore _photoStore;

        private readonly ConnectionString _conString;

        public PhotoReadOnlyRepository(IDbContext context, IPhotoStore photoStore)
        {
            _photoStore = photoStore;

            _conString = context.GetConnectionString(true);
        }

        public async Task<Photo> GetItemAsync(Guid id)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<Photo>();

                var item = await col.FindOneAsync(x => x.Id == id);

                item.File = await _photoStore.LoadPhotoAsync(item.Id);

                return item;
            }
        }

        public async Task<IEnumerable<Photo>> GetItemsAsync()
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                List<Photo> photos = new List<Photo>();

                var col = db.GetCollection<Photo>();

                var items = await col.FindAllAsync();

                // Load actual images from filesystem
                foreach (var i in items)
                {
                    i.File = await _photoStore.LoadPhotoAsync(i.Id);
                    photos.Add(i);
                }

                return photos;
            }
        }

        public async Task<IEnumerable<Photo>> GetItemsAsync(Guid projectId)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                List<Photo> photos = new List<Photo>();

                var col = db.GetCollection<Photo>();

                var items = (await col.FindAllAsync()).ToList().Where(x => x.ProjectId == projectId);

                foreach (var i in items)
                {
                    i.File = await _photoStore.LoadPhotoAsync(i.Id);
                    photos.Add(i);
                }

                return photos;
            }
        }
    }
}
