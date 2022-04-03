using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;
using HannerLabApp.Services.Repositorys;
using HannerLabApp.Utils;
using Xamarin.Forms;

namespace HannerLabApp.ViewModels
{
    /// <summary>
    /// Same as the ProjectSpecificListViewModel but will also hide previously exported items (Those with a non empty Activity ID)
    /// </summary>
    public class ProjectAndActivitySpecificListViewModel<T> : ListViewModelBase<T> where T : ISavable
    {
        protected ProjectAndActivitySpecificListViewModel(IPageService pageService, IReadOnlyRepository<T> repository) : base(pageService, repository)
        {
            // Reload all data whenever the project is changed...
            MessagingCenter.Subscribe<GenericManager<Project>, Project>
            (this, MsgEvents.GetModel(typeof(Project), MsgEvents.Event.Update),
                async (sender, arg) => await ReloadOnProjectChange());
        }

        private async Task ReloadOnProjectChange()
        {
            this.IsLoaded = false;
            await this.LoadData();
        }

        /// <summary>
        /// Load only ONLY from the current project AND those without an activity Id
        /// </summary>
        /// <returns></returns>
        protected override async Task LoadData()
        {
            IsLoaded = true;

            AllItems = new ObservableCollection<IValidableViewModel<T>>();

            var ts = (await _repository.GetItemsAsync())
                .Where(x => x.ProjectId == App.AppSettings.CurrentProjectId)
                .Where(y => y.ActivityId == Guid.Empty);

            foreach (var t in ts)
            {
                var vm = App.AppContainer.Resolve<IValidableViewModel<T>>();
                vm.Model = t;

                AllItems.Add(vm);
            }

            FilteredItems = AllItems;
        }
    }
}
