using ConsoleApp1;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    public class HomeUsersStructure : INotifyPropertyChanged
    {
        private string _userName;
        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged(nameof(UserName));
            }
        }

        private string _tokenFeedBack;
        public string TokenFeedBack
        {
            get { return _tokenFeedBack; }
            set
            {
                _tokenFeedBack = value;
                OnPropertyChanged(nameof(TokenFeedBack));
            }
        }


        private int _unansweredCount;
        public int UnansweredCount
        {
            get { return _unansweredCount; }
            set
            {
                _unansweredCount = value;
                OnPropertyChanged(nameof(UnansweredCount));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
    class HomeViewModel : Core.ViewModel
    {
        private ObservableCollection<HomeUsersStructure> _cards;
        public ObservableCollection<HomeUsersStructure> Cards
        {
            set => _cards = value; 
            get => _cards; 
        }

        public HomeViewModel(List<UsersStructure> users)
        {
            Cards = new ObservableCollection<HomeUsersStructure>(toHomeType(users));
            InitializeUnansweredCounts();
        }
        private async void InitializeUnansweredCounts()
        {
            API api = new API();
            try
            {
                foreach (var user in Cards)
                {
                    user.UnansweredCount = await api.SendGetUnanswered(user.TokenFeedBack);
                    await Task.Delay(1100);
                }
            }
            catch (InvalidOperationException ex)
            {
                await Task.Delay(5000);
                InitializeUnansweredCounts();
            }
        }
        private List<HomeUsersStructure> toHomeType(List<UsersStructure> originUsers)
        {
            var homeUsers = originUsers.Select(user => new HomeUsersStructure
            {
                UserName = user.UserName,
                TokenFeedBack = user.TokenFeedBack,
                UnansweredCount = -1
            }).ToList();

            return homeUsers;
        }
        public void UpdateUsers(List<UsersStructure> users)
        {
            Cards.Clear();
            Cards = new ObservableCollection<HomeUsersStructure>(toHomeType(users));
            InitializeUnansweredCounts();
        }

    }
}
