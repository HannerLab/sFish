using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Autofac;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Managers;
using HannerLabApp.Services.Repositorys;
using HannerLabApp.Utils;
using TinyMvvm;
using Xamarin.Forms;

namespace HannerLabApp.ViewModels.ProjectViewModels
{
    /// <summary>
    /// The main project info page which displays some project statistics
    /// </summary>
    public class ProjectPageViewModel : ViewModelBase
    {
        private readonly IPageService _pageService;
        private readonly IReadOnlyRepository<Project> _repository;
        private readonly IManager<Project> _manager;

        /// <summary>
        /// The currently selected project
        /// </summary>
        private Project _selectedSelectedProject;
        public Project SelectedProject
        {
            get => _selectedSelectedProject;
            set => Set(ref _selectedSelectedProject, value);
        }

        /// <summary>
        /// The list of all the projects that are currently available.
        /// </summary>
        private ObservableCollection<Project> _projects = new ObservableCollection<Project>();
        public ObservableCollection<Project> Projects
        {
            get => _projects;
            private set => Set(ref _projects, value);
        }

        protected bool IsLoaded { get; private set; }

        public ICommand LoadProjectCommand { get; private set; }

        public ProjectInfoViewModel InfoViewModel { get; private set; }

        public ProjectPageViewModel(IPageService pageService, IReadOnlyRepository<Project> repository, IManager<Project> manager)
        {
            _pageService = pageService;
            _repository = repository;
            _manager = manager;

            InfoViewModel = App.AppContainer.Resolve<ProjectInfoViewModel>();

            LoadProjectCommand = new TinyCommand(async () => await LoadProjectAsync());

            // Subscribe to message service
            MessagingCenter.Subscribe<GenericManager<Project>, Project>
                (this, MsgEvents.GetModel(typeof(Project), MsgEvents.Event.Addition), 
                    (GenericManager<Project> source, Project parameter) => Projects.Add(parameter));

            MessagingCenter.Subscribe<GenericManager<Project>, Project>
                (this, MsgEvents.GetModel(typeof(Project), MsgEvents.Event.Deletion),
                    (GenericManager<Project> source, Project parameter) => Projects.Where(x => x.Id == parameter.Id)
                        .ToList().ForEach((Project p) => Projects.Remove(p)));
        }


        /// <summary>
        /// Initialization of this viewmodel. Async alternative to constructor.
        /// </summary>
        /// <returns></returns>
        public override async Task Initialize()
        {
            IsBusy = true;

            if (!IsLoaded)
                await LoadProjectsAsync();

            await base.Initialize();
            IsBusy = false;
        }

        /// <summary>
        /// Loads all the available projects from the data store
        /// </summary>
        /// <returns></returns>
        private async Task LoadProjectsAsync()
        {
            IsLoaded = true;

            Projects = new ObservableCollection<Project>();

            var projects = await _repository.GetItemsAsync();
            foreach (var p in projects)
            {
                Projects.Add(p);
            }

            // Set current project from settings
            var currentProjectId = App.AppSettings.CurrentProjectId;
            var current = Projects.Where(x => x.Id == currentProjectId).FirstOrDefault();


            SelectedProject = current ?? new Project();
        }

        private async Task LoadProjectAsync()
        {
            if (SelectedProject == null) return;

            // Update Settings to reflect the current project
            App.AppSettings.CurrentProjectId = SelectedProject.Id;

            // Set Project recorders to the default
            App.AppSettings.CurrentRecorder = SelectedProject.RecordedBy;

            // Update project last time access
            SelectedProject.LastAccessed = DateTime.Now;

            // Update the project in database, and in the rest of the app. 
            await _manager.UpdateItemAsync(SelectedProject);

            // Refresh project info
            await InfoViewModel.LoadProjectAsync(SelectedProject);
        }

        // Prevent infinite loop/ crashing when trying to refreshing project data..
        private bool _loadSwitch = false;

        public override async Task OnAppearing()
        {
            // Refresh project info
            if (SelectedProject != null && SelectedProject.Id != Guid.Empty && _loadSwitch == true)
            {
                await InfoViewModel.LoadProjectAsync(SelectedProject);
                _loadSwitch = false;
            }
        }

        public override Task OnDisappearing()
        {
            _loadSwitch = true;
            return base.OnDisappearing();
        }
    }
}
