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
    /// <summary>
    /// A photo repository stores and loads photo objects like the other ones, except it removes the file64 field from the data and instead saves the files to blob storage.
    /// </summary>
    public class PhotoRepository : IRepository<Photo>
    {
        private readonly IPhotoStore _photoStore;

        private readonly ConnectionString _conString;

        public PhotoRepository(IDbContext context, IPhotoStore photoStore)
        {
            _photoStore = photoStore;

            _conString = context.GetConnectionString(false);
        }

        /// <summary>
        /// Saves a photo object to the data store/repo. The photo data in base64 string format will get converted to a file in the app data directory of the device.
        /// The resulting model saved will always contain an empty string in the file64 property.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddItemAsync(Photo item)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var ret = await _photoStore.SavePhotoAsync(item.Id, item.File);

                if (!ret)
                {
                    Console.WriteLine("Failed to saved photo");
                    return;
                }

                var col = db.GetCollection<Photo>();

                await col.InsertAsync(item);

                await col.EnsureIndexAsync(x => x.Id);
                await col.EnsureIndexAsync(y => y.ProjectId);
            }
        }

        public async Task DeleteItemAsync(Photo item)
        {
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<Photo>();

                await col.DeleteAsync(item.Id);
                await _photoStore.DeletePhotoAsync(item.Id);
            }
            
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

        public async Task UpdateItemAsync(Photo item)
        {
            // WARNING - EDITING FILES OF EXISTING MODELS SHOULD NOT BE ALLOWED. UPDATES WILL NOT OCCUR
            using (var db = new LiteDatabaseAsync(_conString))
            {
                var col = db.GetCollection<Photo>();

                await col.UpdateAsync(item);
            }
        }
    }
}
