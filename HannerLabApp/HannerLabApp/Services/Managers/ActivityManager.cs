using System;
using System.Threading.Tasks;
using HannerLabApp.Models;
using HannerLabApp.Utils;
using Xamarin.Forms;

namespace HannerLabApp.Services.Managers
{
    public class ActivityManager : IManager<Activity>
    {
        public async Task DeleteItemAsync(Activity item)
        {
            await Task.Delay(1);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Send the added, and validated activity from the details page back to the export service
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public async Task AddItemAsync(Activity item)
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

            var msg = MsgEvents.GetModel(typeof(Activity), MsgEvents.Event.Addition);
            MessagingCenter.Send(this, msg, item);
        }

        public async Task UpdateItemAsync(Activity item)
        {
            await Task.Delay(1);

            throw new NotImplementedException();
        }
    }
}
