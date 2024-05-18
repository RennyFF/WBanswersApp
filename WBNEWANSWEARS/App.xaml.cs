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
            //UsersStructure us = new UsersStructure();
            //us.UserName = "1as";
            //us.TokenFeedBack = "3";
            //us.TokenContent = "3";
            //us.Preset = "3";
            //db.AddDBUsers(us);
            //us.UserName = "2as";
            //us.TokenFeedBack = "3";
            //us.TokenContent = "3";
            //us.Preset = "3";
            //db.AddDBUsers(us);
            //AnswersStructure asc = new AnswersStructure();
            //asc.Id = 1;
            //asc.IsRating = true;
            //asc.Title = "BARARA2";
            //asc.Priority = 1;
            //asc.IsUsed = true;
            //asc.Text = "FUCK2";
            //asc.UserId = 2;
            //db.AddDBAnsw(asc);
            //asc.Id = 1;
            //asc.IsRating = true;
            //asc.Title = "BARARA1";
            //asc.Priority = 1;
            //asc.IsUsed = true;
            //asc.Text = "FUCK1";
            //asc.UserId = 1;
            //db.AddDBAnsw(asc);
            if (isSuccessCreation)
            {
                List<UsersStructure> _tmpUsers = getAllUsersFromDB();
                List<AnswersStructure> _tmpAnswers = getAllAnswersFromDB();
                USERS = PopulateUsersWithAnswers(_tmpUsers, _tmpAnswers);
            }
            else
            {
                MessageBox.Show("Ошибка загрузки данных!");
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
