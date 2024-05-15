using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class SettingsViewModel : Core.ViewModel
    {
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
                onPropertyChanged("SelectedUser");
            }
            get => _selectedUser;
        }

        public SettingsViewModel(List<UsersStructure> users)
        {
            Users = new ObservableCollection<UsersStructure>(users);
        }
    }
}
