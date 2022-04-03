using System;
using System.Threading.Tasks;
using HannerLabApp.Models;
using HannerLabApp.Services.Repositorys;
using HannerLabApp.Utils;
using Xamarin.Forms;

namespace HannerLabApp.Services.Managers
{
    /// <summary>
    /// The manager service wraps data base access, adds the message bus layer to alert the rest of the application of db changes without having to reload large chunks.
    /// Handles adding new, deleting, or updating items. SelectedProject Changes are handled as an update event - The last accessed time is updated.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class GenericManager<T> : IManager<T> where T : ISavable
    {

        private readonly IRepository<T> _repository;

        public GenericManager(IRepository<T> repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Adds a new item into the data store and sends a message alert that an item of that type has been added with the item object as the parameter.
        /// </summary>
        /// <param name="item">The item to add to the data store</param>
        /// <returns></returns>
        public async Task AddItemAsync(T item)
        {
            // Generate a new ID if non is given
            if (item.Id == Guid.Empty)
                item.Id = Guid.NewGuid();

            // Use the current project is no project is set
            if (item.ProjectId == Guid.Empty)
                item.ProjectId = App.AppSettings.CurrentProjectId;

            // Tag recorder
            if (string.IsNullOrEmpty(item.RecordedBy))
                item.RecordedBy = App.AppSettings.CurrentRecorder;

            // Tag time
            if (item.Timestamp == DateTime.MinValue)
                item.Timestamp = DateTime.Now;

            item.LastUpdated = DateTime.Now;

            await _repository.AddItemAsync(item);
            var msg = MsgEvents.GetModel(typeof(T), MsgEvents.Event.Addition);
            MessagingCenter.Send(this, msg, item);
        }

        /// <summary>
        /// Deletes an item from the data store and sends a message alert that an item of that type has been deleted with the item object as the parameter.
        /// </summary>
        /// <param name="item">The item to delete from the data store</param>
        /// <returns></returns>
        public async Task DeleteItemAsync(T item)
        {
            await _repository.DeleteItemAsync(item);
            var msg = MsgEvents.GetModel(typeof(T), MsgEvents.Event.Deletion);
            MessagingCenter.Send(this, msg, item);
        }

        /// <summary>
        /// Updates an item in the data store and sends a message alert that an item of that type has been updated with the item object as the parameter.
        /// </summary>
        /// <param name="item">The item to update in the data store</param>
        /// <returns></returns>
        public async Task UpdateItemAsync(T item)
        {
            item.LastUpdated = DateTime.Now;

            await _repository.UpdateItemAsync(item);
            var msg = MsgEvents.GetModel(typeof(T), MsgEvents.Event.Update);
            MessagingCenter.Send(this, msg, item);
        }
    }
}
