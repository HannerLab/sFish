using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Autofac;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Repositorys;

namespace HannerLabApp.ViewModels.PhotoViewModels
{
    public class PhotoListViewModel : ProjectAndActivitySpecificListViewModel<Photo>
    {
        public PhotoListViewModel(IPageService pageService, IReadOnlyRepository<Photo> repository) : base(pageService, repository)
        {
        }

        protected override async Task LoadData()
        {
            IsLoaded = true;

            AllItems = new ObservableCollection<IValidableViewModel<Photo>>();

            var ts = await _repository.GetItemsAsync();
            foreach (var t in ts)
            {
                var vm = App.AppContainer.Resolve<IValidableViewModel<Photo>>();
                vm.Model = t;

                try
                {
                    await (vm as PhotoViewModel).LoadFileImageThumbnailAsync();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Could not generate image thumbnail.", ex);
                }

                AllItems.Add(vm);
            }

            FilteredItems = AllItems;
        }
    }
}
