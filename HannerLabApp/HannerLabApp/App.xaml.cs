using Autofac;
using System.IO;
using System.Reflection;
using HannerLabApp.Configuration;
using HannerLabApp.Models;
using HannerLabApp.Services;
using HannerLabApp.Services.Cloud;
using HannerLabApp.Services.Cloud.DropBox;
using HannerLabApp.Services.Exporters;
using HannerLabApp.Services.Managers;
using HannerLabApp.Services.Media;
using HannerLabApp.Services.Repositorys;
using HannerLabApp.ViewModels;
using HannerLabApp.ViewModels.ActivityViewModels;
using HannerLabApp.ViewModels.EdnaViewModels;
using HannerLabApp.ViewModels.EquipmentViewModels;
using HannerLabApp.ViewModels.ObservationViewModels;
using HannerLabApp.ViewModels.PhotoViewModels;
using HannerLabApp.ViewModels.ProjectViewModels;
using HannerLabApp.ViewModels.ReadingViewModels;
using HannerLabApp.ViewModels.SiteViewModels;
using HannerLabApp.ViewModels.StationViewModels;
using HannerLabApp.Views;
using HannerLabApp.Views.ActivityViews;
using HannerLabApp.Views.EdnaViews;
using HannerLabApp.Views.EquipmentViews;
using HannerLabApp.Views.ObservationViews;
using HannerLabApp.Views.PhotoViews;
using HannerLabApp.Views.ProjectViews;
using HannerLabApp.Views.ReadingViews;
using HannerLabApp.Views.SiteViews;
using HannerLabApp.Views.StationViews;
using LiteDB;
using Newtonsoft.Json;
using TinyMvvm;
using TinyMvvm.Autofac;
using TinyMvvm.Forms;
using TinyMvvm.IoC;
using Xamarin.Forms;

namespace HannerLabApp
{
    public partial class App : Application
    {
        private static AppSettings _appSettings;
        private static IContainer _appContainer;
        private static AppConfiguration _appConfiguration;

        public App()
        {
            InitializeComponent();

            // Setup Nav service
            var navigationHelper = new ShellNavigationHelper();

            var currentAssembly = Assembly.GetExecutingAssembly();
            navigationHelper.RegisterViewsInAssembly(currentAssembly);
            navigationHelper.InitViewModelNavigation(currentAssembly);

            var containerBuilder = new ContainerBuilder();

            containerBuilder.RegisterInstance<INavigationHelper>(navigationHelper);

            var appAssembly = typeof(App).GetTypeInfo().Assembly;
            containerBuilder.RegisterAssemblyTypes(appAssembly)
                   .Where(x => x.IsSubclassOf(typeof(Page)));

            containerBuilder.RegisterAssemblyTypes(appAssembly)
                   .Where(x => x.IsSubclassOf(typeof(ViewModelBase)));

            // Platform specific services - use Xamarin Dependency service resolve
            //IDeviceIdentifier
            //IMediaService

            // Services
            containerBuilder.RegisterType<PageService>().As<IPageService>();
            containerBuilder.RegisterType<PhotoCaptureService>().As<IPhotoCaptureService>();
            containerBuilder.RegisterType<BarcodeScanner>().As<IBarcodeScanner>();
            containerBuilder.RegisterType<GeoLocator>().As<IGeoLocator>();
            containerBuilder.RegisterType<DropboxLoginHelper>().As<ICloudAuthenticator>();
            containerBuilder.RegisterType<DropboxFileHandler>().As<ICloudFileHandler>();
            containerBuilder.RegisterType<PhotoStore>().As<IPhotoStore>();
            containerBuilder.RegisterType<Services.Media.FileShare>().As<IFileShare>();
            containerBuilder.RegisterType<DbContext>().As<IDbContext>();
            containerBuilder.RegisterType<ActivityExportCreator>().As<IActivityExportCreator>();

            // Repos
            containerBuilder.RegisterType<GenericRepository<Project>>().As<IRepository<Project>>();
            containerBuilder.RegisterType<GenericRepository<Site>>().As<IRepository<Site>>();
            containerBuilder.RegisterType<GenericRepository<Station>>().As<IRepository<Station>>();
            containerBuilder.RegisterType<GenericRepository<Edna>>().As<IRepository<Edna>>();
            containerBuilder.RegisterType<GenericRepository<Reading>>().As<IRepository<Reading>>();
            containerBuilder.RegisterType<PhotoRepository>().As<IRepository<Photo>>(); // Photo repo is special as it saves files outside of main tables
            containerBuilder.RegisterType<GenericRepository<Observation>>().As<IRepository<Observation>>();
            containerBuilder.RegisterType<GenericRepository<Equipment>>().As<IRepository<Equipment>>();

            containerBuilder.RegisterType<GenericRepository<Activity>>().As<IRepository<Activity>>();

            // TODO: This shouldn't depend on all that validation stuff.
            containerBuilder.RegisterType<GenericRepository<Export>>().As<IRepository<Export>>();

            // RO repos
            containerBuilder.RegisterType<ReadOnlyRepository<Project>>().As<IReadOnlyRepository<Project>>();
            containerBuilder.RegisterType<ReadOnlyRepository<Site>>().As<IReadOnlyRepository<Site>>();
            containerBuilder.RegisterType<ReadOnlyRepository<Station>>().As<IReadOnlyRepository<Station>>();
            containerBuilder.RegisterType<ReadOnlyRepository<Edna>>().As<IReadOnlyRepository<Edna>>();
            containerBuilder.RegisterType<ReadOnlyRepository<Reading>>().As<IReadOnlyRepository<Reading>>();
            containerBuilder.RegisterType<PhotoReadOnlyRepository>().As<IReadOnlyRepository<Photo>>();
            containerBuilder.RegisterType<ReadOnlyRepository<Observation>>().As<IReadOnlyRepository<Observation>>();
            containerBuilder.RegisterType<ReadOnlyRepository<Equipment>>().As<IReadOnlyRepository<Equipment>>();

            containerBuilder.RegisterType<ReadOnlyRepository<Activity>>().As<IReadOnlyRepository<Activity>>();

            containerBuilder.RegisterType<ReadOnlyRepository<Export>>().As<IReadOnlyRepository<Export>>();

            // Manager
            containerBuilder.RegisterType<GenericManager<Project>>().As<IManager<Project>>();
            containerBuilder.RegisterType<GenericManager<Site>>().As<IManager<Site>>();
            containerBuilder.RegisterType<GenericManager<Station>>().As<IManager<Station>>();
            containerBuilder.RegisterType<GenericManager<Edna>>().As<IManager<Edna>>();
            containerBuilder.RegisterType<GenericManager<Reading>>().As<IManager<Reading>>();
            containerBuilder.RegisterType<GenericManager<Photo>>().As<IManager<Photo>>();
            containerBuilder.RegisterType<GenericManager<Observation>>().As<IManager<Observation>>();
            containerBuilder.RegisterType<GenericManager<Equipment>>().As<IManager<Equipment>>();

            // Activities use a separate manager, because They aren't directly saved or deleted in db, but passed to the activity exporter service for further processing.
            containerBuilder.RegisterType<ActivityManager>().As<IManager<Activity>>();

            containerBuilder.RegisterType<GenericManager<Export>>().As<IManager<Export>>();

            // Validable view models
            containerBuilder.RegisterType<ProjectViewModel>().As<IValidableViewModel<Project>>();
            containerBuilder.RegisterType<SiteViewModel>().As<IValidableViewModel<Site>>();
            containerBuilder.RegisterType<StationViewModel>().As<IValidableViewModel<Station>>();
            containerBuilder.RegisterType<ReadingViewModel>().As<IValidableViewModel<Reading>>();
            containerBuilder.RegisterType<EdnaViewModel>().As<IValidableViewModel<Edna>>();
            containerBuilder.RegisterType<ObservationViewModel>().As<IValidableViewModel<Observation>>();
            containerBuilder.RegisterType<PhotoViewModel>().As<IValidableViewModel<Photo>>();
            containerBuilder.RegisterType<EquipmentViewModel>().As<IValidableViewModel<Equipment>>();

            // Other view models
            containerBuilder.RegisterType<ActivityViewModel>().As<IValidableViewModel<Activity>>();
            containerBuilder.RegisterType<ProjectInfoViewModel>().As<ProjectInfoViewModel>();

            // TODO: Same with this as above
            containerBuilder.RegisterType<ActivityExportViewModel>().As<IValidableViewModel<Export>>();

            // Details View Models
            containerBuilder.RegisterType<ProjectDetailsViewModel>().As<IDetailsViewModel<Project>>();
            containerBuilder.RegisterType<GenericDetailsViewModel<Site>>().As<IDetailsViewModel<Site>>();
            containerBuilder.RegisterType<StationDetailsViewModel>().As<IDetailsViewModel<Station>>(); // Depends on site
            containerBuilder.RegisterType<SampleDetailsViewModel<Edna>>().As<IDetailsViewModel<Edna>>(); // Depends on station
            containerBuilder.RegisterType<SampleDetailsViewModel<Reading>>().As<IDetailsViewModel<Reading>>(); // Depends on station
            containerBuilder.RegisterType<ObservationDetailsViewModel>().As<IDetailsViewModel<Observation>>(); // Depends on station and site
            containerBuilder.RegisterType<PhotoDetailsViewModel>().As<IDetailsViewModel<Photo>>(); // Depends on station, site, reading, and observation
            containerBuilder.RegisterType<EquipmentDetailsViewModel>().As<IDetailsViewModel<Equipment>>();

            containerBuilder.RegisterType<ActivityDetailsViewModel>().As<IDetailsViewModel<Activity>>();

            // Details views
            containerBuilder.RegisterType<ProjectDetailsView>().As<IDetailsView<Project>>();
            containerBuilder.RegisterType<SiteDetailsView>().As<IDetailsView<Site>>();
            containerBuilder.RegisterType<StationDetailsView>().As<IDetailsView<Station>>();
            containerBuilder.RegisterType<EdnaDetailsView>().As<IDetailsView<Edna>>();
            containerBuilder.RegisterType<ReadingDetailsView>().As<IDetailsView<Reading>>();
            containerBuilder.RegisterType<ObservationDetailsView>().As<IDetailsView<Observation>>();
            containerBuilder.RegisterType<PhotoDetailsView>().As<IDetailsView<Photo>>();
            containerBuilder.RegisterType<EquipmentDetailsView>().As<IDetailsView<Equipment>>();

            containerBuilder.RegisterType<ActivityDetailsView>().As<IDetailsView<Activity>>();

            // End and build
            _appContainer = containerBuilder.Build();
            Resolver.SetResolver(new AutofacResolver(_appContainer));

            // Ensure database file exists on start up.
            EnsureDataIsCreated();

            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
            // Do nothing...
        }

        protected override void OnSleep()
        {
            // Do nothing...
        }

        protected override void OnResume()
        {
            // Do nothing...
        }

        /// <summary>
        /// Creates an empty data base file if only isn't already
        /// </summary>
        private void EnsureDataIsCreated()
        {
            var conString = new DbContext().GetConnectionString(false);
            var fileFullPath = conString.Filename;

            // Create folder structure
            Directory.CreateDirectory(Constants.AppDataDirectory);
            Directory.CreateDirectory(Constants.TempDirectory);
            Directory.CreateDirectory(Constants.ExportDirectory);
            Directory.CreateDirectory(Constants.MediaDirectory);
            
            // Initialize db using non async method
            if (!File.Exists(fileFullPath))
            {
                using (var db = new LiteDatabase(conString))
                {
                    var col = db.GetCollection<Project>();
                }
            }
        }

        /// <summary>
        /// Main DI container instance to resolve dependencies
        /// </summary>
        public static IContainer AppContainer => _appContainer;

        /// <summary>
        /// The main app settings instance. App settings are managed, directly and indirectly by the user
        /// </summary>
        public static AppSettings AppSettings => _appSettings ??= new AppSettings();

        /// <summary>
        /// The main app configuration instance. App configuration is defined at build time and contains tokens etc
        /// </summary>
        public static AppConfiguration AppConfiguration
        {
            get
            {
                // Lazy load save on boot time
                if (_appConfiguration == null)
                {
                    LoadAppConfiguration();
                }

                return _appConfiguration;
            }
        }


        /// <summary>
        /// Loads up the app configuration from the proper *.json file.
        /// </summary>
        private static void LoadAppConfiguration()
        {

            // Different json file for release, and debug.
#if DEBUG
           var appSettingsResourceStream = Assembly.GetAssembly(typeof(AppConfiguration))
                .GetManifestResourceStream("HannerLabApp.appsettings.debug.json");
#else
            var appSettingsResourceStream = Assembly.GetAssembly(typeof(AppConfiguration))
                .GetManifestResourceStream("HannerLabApp.appsettings.json");
#endif

            if (appSettingsResourceStream == null)
            {
                return;
            }

            using (var streamReader = new StreamReader(appSettingsResourceStream))
            {
                var jsonString = streamReader.ReadToEnd();
                _appConfiguration = JsonConvert.DeserializeObject<AppConfiguration>(jsonString);
            }
        }
    }
}
