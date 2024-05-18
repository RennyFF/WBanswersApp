using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class SettingsViewModel : Core.ViewModel
    {
        private ObservableCollection<int> priorityNumbers = new ObservableCollection<int>()
        {
            0,
            1,
            2,
            3,
            4,
            5,
            6,
            7,
            8,
            9
        };
        public ObservableCollection<int> PriorityNumbers
        {
            set => priorityNumbers = value;
            get => priorityNumbers;
        }
        private ObservableCollection<UsersStructure> _users;
        public ObservableCollection<UsersStructure> Users
        {
            set => _users = value;
            get => _users;
        }

        private UsersStructure _selectedUser;
        public UsersStructure SelectedUser
        {
            set
            {
                _selectedUser = value;
                onPropertyChanged(nameof(SelectedUser));
            }
            get => _selectedUser;
        }

        public SettingsViewModel(List<UsersStructure> users)
        {
            Users = new ObservableCollection<UsersStructure>(users);
        }
    }
}
