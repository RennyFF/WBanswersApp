using ConsoleApp1;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WBNEWANSWEARS.Core;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class ActiveViewModel : Core.ViewModel
    {
        private ObservableCollection<UsersStructure> _users;
        public ObservableCollection<UsersStructure> Users
        {
            set
            {
                _users = value;
                onPropertyChanged(nameof(Users));
            } 
            get => _users;
        }

        private List<UsersStructure> _usersSelected = new List<UsersStructure>();
        public List<UsersStructure> UsersSelected
        {
            set => _usersSelected = value;
            get => _usersSelected;
        }

        private RelayCommand startAnswer;
        private RelayCommand startSingleAnswer;
        private RelayCommand toggleAnswer;
        private RelayCommand selectAllUsersCommand;

        public RelayCommand ToggleAnswer
        {
            get
            {
                return toggleAnswer ??= new RelayCommand(obj =>
                {
                    if (obj is ListBox listBox)
                    {
                        UsersSelected = listBox.SelectedItems.Cast<UsersStructure>().ToList();
                    }
                }, obj => Users != null && Users.Count > 0);
            }
        }

        public RelayCommand StartAnswer
        {
            get
            {
                return startAnswer ??= new RelayCommand(obj =>
                {
                    if (UsersSelected.Count == 0)
                    {
                        MessageBox.Show("Пользователи не выбраны!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    else
                    {
                        foreach (UsersStructure item in UsersSelected)
                        {
                            MessageBox.Show($"{item.UserName} выбран!");
                        }
                    }
                }, obj => UsersSelected != null);
            }
        }
        public RelayCommand StartSingleAnswer
        {
            get
            {
                return startSingleAnswer ??= new RelayCommand(obj =>
                {
                    if (obj is UsersStructure user)
                    {
                        UsersStructure s = new UsersStructure();
                        s = obj as UsersStructure;
                        MessageBox.Show($"{s.UserName}");
                    }
                }, obj => UsersSelected != null);
            }
        }

        public RelayCommand SelectAllUsersCommand
        {
            get
            {
                return selectAllUsersCommand ??= new RelayCommand(obj =>
                {
                    UsersSelected = new List<UsersStructure>(Users);

                    if (obj is ListBox listBox)
                    {
                        listBox.SelectAll();
                        onPropertyChanged();
                    }
                }, obj => Users != null && Users.Count > 0);
            }
        }

        public ActiveViewModel(List<UsersStructure> users)
        {
            Users = new ObservableCollection<UsersStructure>(users);
        }
    }
}
