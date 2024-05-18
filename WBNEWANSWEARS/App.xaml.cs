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
            UsersStructure us = new UsersStructure();
            //us.UserName = "Савицкая";
            //us.TokenFeedBack = "eyJhbGciOiJFUzI1NiIsImtpZCI6IjIwMjMxMDI1djEiLCJ0eXAiOiJKV1QifQ.eyJlbnQiOjEsImV4cCI6MTcxODg0MzU2NCwiaWQiOiI0NjU2MzkxMi0wNGQ0LTQ3NzgtYjViMS1iOTlhMWI5ZjJmMzciLCJpaWQiOjIzNDgxMjE2LCJvaWQiOjg2ODIyMiwicyI6MTI4LCJzaWQiOiI1ZjQ1NDI0ZC1kYjljLTRhNDktOTEyOC0zZmVhOWU2NTUxNTEiLCJ0IjpmYWxzZSwidWlkIjoyMzQ4MTIxNn0.avEWaBAbliSZxOI8LxwxVc08pJHuwVn21B_r6x1Xkl7v0ViZt2xt2HU3TsL02loD90X5lSc8zrUliYv_AVuGtQ";
            //us.TokenContent = "3";
            //us.Preset = "3";
            //db.AddDBUsers(us);
            //us.UserName = "Eco Best";
            //us.TokenFeedBack = "eyJhbGciOiJFUzI1NiIsImtpZCI6IjIwMjMxMDI1djEiLCJ0eXAiOiJKV1QifQ.eyJlbnQiOjEsImV4cCI6MTcxODc5MDY4NywiaWQiOiI2YTFjNTllOC01OGQyLTQwZWMtOWUwMi1lZDM5NjgxODc5NjYiLCJpaWQiOjEwMzgzMjczNywib2lkIjoxMjEyOTk3LCJzIjoxMjgsInNhbmRib3giOmZhbHNlLCJzaWQiOiJhZTA3NzhkZC1iYzg5LTRiMTQtYTM3OS04MTc1NmZiMzJjZWUiLCJ1aWQiOjEwMzgzMjczN30.aGBV6LtN4B4XjDzpAnu3tAreHDEsv6u2BDNwwvx8DBzBPR0hJGZIuhFr53LJ6QS8NChG5tO3eZmBClGSz5VGTQ";
            //us.TokenContent = "3";
            //us.Preset = "3";
            //db.AddDBUsers(us);
            //us.UserName = "Gatt";
            //us.TokenFeedBack = "eyJhbGciOiJFUzI1NiIsImtpZCI6IjIwMjMxMDI1djEiLCJ0eXAiOiJKV1QifQ.eyJlbnQiOjEsImV4cCI6MTcxODgzOTA2NiwiaWQiOiIwN2Y1NTI4Ny05ODM5LTRhNjUtOWFhMy00Zjc3YzhiZmRhZGUiLCJpaWQiOjM2OTg3NDAzLCJvaWQiOjEyODQwNDIsInMiOjEyOCwic2FuZGJveCI6ZmFsc2UsInNpZCI6IjY0NmVjN2QwLWQ5NDEtNDlmOC04NmMxLWQ5ZDhhZDlmZDRjMCIsInVpZCI6MzY5ODc0MDN9.pIfCVjs0mfioCRv33NEFf5aCbKTEm9yLhYOeZL0UO4nb_kvsuGvWGJpJtCdccnOnrwfmnYq-bdISwaEkL7Y31Q";
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
