using HannerLabApp.Services;
using HannerLabApp.Services.Managers;
using HannerLabApp.Services.Repositorys;
using System.Threading.Tasks;
using HannerLabApp.Models;
using System.Collections.ObjectModel;
using System.Linq;
using Autofac;
using HannerLabApp.Utils;
using Xamarin.Forms;

namespace HannerLabApp.ViewModels
{
    /// <summary>
    /// A List view which filters its item to those only in the current project scope.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ProjectSpecificListViewModel<T> : ListViewModelBase<T> where T: ISavable
    {
        protected ProjectSpecificListViewModel(IPageService pageService, IReadOnlyRepository<T> repository) : base(pageService, repository)
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
        /// Load only ONLY from the current project.
        /// </summary>
        /// <returns></returns>
        protected override async Task LoadData()
        {
            IsLoaded = true;

            AllItems = new ObservableCollection<IValidableViewModel<T>>();

            var ts = await _repository.GetItemsAsync();
            foreach (var t in ts.ToList().Where(x => x.ProjectId == App.AppSettings.CurrentProjectId))
            {
                var vm = App.AppContainer.Resolve<IValidableViewModel<T>>();
                vm.Model = t;

                AllItems.Add(vm);
            }

            FilteredItems = AllItems;
        }


        public override Task OnAppearing()
        {
            return base.OnAppearing();
        }
    }
}
