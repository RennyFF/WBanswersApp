using ConsoleApp1;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class ActiveViewModel : Core.ViewModel
    {
        public class UsersStructureActiveVersion : UsersStructure
        {
            public bool IsSelected { get; set; }
        }
        private ObservableCollection<UsersStructureActiveVersion> _users;
        public ObservableCollection<UsersStructureActiveVersion> Users
        {
            set => _users = value;
            get => _users;
        }

        public ActiveViewModel(List<UsersStructure> users)
        {
            Users = AddaptToNewStructure(users);
        }

        ObservableCollection<UsersStructureActiveVersion> AddaptToNewStructure(List<UsersStructure> _usersPrime)
        {
            return new ObservableCollection<UsersStructureActiveVersion>(_usersPrime.Select(user =>
                new UsersStructureActiveVersion() { Id = user.Id, Answers = user.Answers, TokenFeedBack = user.TokenFeedBack,
                    TokenContent = user.TokenContent, UserName = user.UserName, Preset = user.Preset, IsSelected = false}
            ));
        }

    }
}