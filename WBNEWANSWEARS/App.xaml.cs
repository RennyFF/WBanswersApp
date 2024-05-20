using Microsoft.Extensions.DependencyInjection;
using System.Collections.ObjectModel;
using System.Windows;
using WBNEWANSWEARS.MVVM.Model;
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
            set
            {
                users_list = value;
            }
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
            services.AddSingleton<HomeViewModel>(provider => new HomeViewModel(USERS));
            services.AddSingleton<SettingsViewModel>(provider => new SettingsViewModel(USERS));
            services.AddSingleton<ActiveViewModel>(provider => new ActiveViewModel(USERS));
            services.AddSingleton<CommentsViewModel>(provider => new CommentsViewModel(USERS));
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
                USERS = getUsers();
            }
            else
            {
                MessageBox.Show("Данные не смогли загрузится", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
            var settingsViewModel = _serviceProvider.GetRequiredService<SettingsViewModel>();
            var homeViewModel = _serviceProvider.GetRequiredService<HomeViewModel>();
            var activeViewModel = _serviceProvider.GetRequiredService<ActiveViewModel>();
            var commentsViewModel = _serviceProvider.GetRequiredService<CommentsViewModel>();
            settingsViewModel.UsersUpdated += (updatedUsers) =>
            {
                USERS = updatedUsers;
                homeViewModel.UpdateUsers(updatedUsers);
                commentsViewModel.Users = new ObservableCollection<UsersStructure>(updatedUsers);
                activeViewModel.Users = new ObservableCollection<UsersStructure>(updatedUsers);

            };
            commentsViewModel.UsersByCommentsUpdated += () =>
            {
                USERS = getUsers();
                homeViewModel.UpdateUsers(USERS);
                commentsViewModel.Users = new ObservableCollection<UsersStructure>(USERS);
                activeViewModel.Users = new ObservableCollection<UsersStructure>(USERS);
            };
            activeViewModel.UsersAnswered += () =>
            {
                homeViewModel.UpdateUsers(USERS);
            };
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
            base.OnStartup(e);
        }

        private List<UsersStructure> getUsers()
        {
            List<UsersStructure> _tmpUsers = getAllUsersFromDB();
            List<AnswersStructure> _tmpAnswers = getAllAnswersFromDB();
            var Users = PopulateUsersWithAnswers(_tmpUsers, _tmpAnswers);
            return Users;
        }
        private List<UsersStructure> getAllUsersFromDB()
        {
            return db.GetDBUsers();
        }

        private List<AnswersStructure> getAllAnswersFromDB()
        {
            return db.GetDBAnsw();
        }

        private List<UsersStructure> PopulateUsersWithAnswers(List<UsersStructure> users, List<AnswersStructure> answers)
        {
            var answersGroupedByUserId = answers.GroupBy(a => a.UserId);

            foreach (var user in users)
            {
                if (answersGroupedByUserId.Any(g => g.Key == user.Id))
                {
                    user.Answers = answersGroupedByUserId.First(g => g.Key == user.Id).ToList();
                }
                else
                {
                    user.Answers = [];
                }
            }
            return users;
        }


    }

}
