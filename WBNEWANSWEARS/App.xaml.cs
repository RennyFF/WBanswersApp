using ConsoleApp1;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Navigation;
using WBNEWANSWEARS.MVVM.ViewModel;
using WBNEWANSWEARS.Services;
using WBNEWANSWEARS.ViewModel;
using NavigationService = WBNEWANSWEARS.Services.NavigationService;

namespace WBNEWANSWEARS
{
    public partial class App : Application
    {
        private readonly ServiceProvider _serviceProvider;

        private List<UsersStructure> users_list = new();
        public List<UsersStructure> USERS
        {
            get { return users_list; }
            set { users_list = value; }
        }

        private List<AnswersStructure> answers_list = new();
        public List<AnswersStructure> ANSWERS
        {
            get { return answers_list; }
            set { answers_list = value; }
        }

        private dbRequests db = new();

        public App()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddSingleton<MainWindow>(provider => new MainWindow
            {
                DataContext = provider.GetRequiredService<MainViewModel>()
            });
            services.AddSingleton<MainViewModel>();
            services.AddSingleton<HomeViewModel>(provider => new HomeViewModel(USERS)); /*??? РАБОТАЕТ ЛИ*/
            services.AddSingleton<SettingsViewModel>();
            services.AddSingleton<ActiveViewModel>();
            services.AddSingleton<CommentsViewModel>();
            services.AddSingleton<INavigationService, NavigationService>();

            services.AddSingleton<Func<Type, Core.ViewModel>>(serviceProvider =>
                viewModelType => (Core.ViewModel)serviceProvider.GetRequiredService(viewModelType));

            _serviceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            bool isSuccessCreation = false;
            isSuccessCreation = db.CreateDBUsers();
            isSuccessCreation = db.CreateDBAnsw();
            if (isSuccessCreation)
            {
                USERS = getAllUsersFromDB();
                ANSWERS = getAllAnswersFromDB();
            }
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        private List<UsersStructure> getAllUsersFromDB()
        {
            return db.GetDBUsers();
        }

        private List<AnswersStructure> getAllAnswersFromDB()
        {
            return db.GetDBAnsw();
        }

    }

}
