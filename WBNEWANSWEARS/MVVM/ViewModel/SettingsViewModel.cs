using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WBNEWANSWEARS.Core;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class SettingsViewModel : Core.ViewModel
    {
        public delegate void UsersUpdatedEventHandler(List<UsersStructure> updatedUsers);
        public event UsersUpdatedEventHandler UsersUpdated;
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
            set
            {
                _users = value;
                onPropertyChanged(nameof(Users));
            }
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
        private RelayCommand addUser;
        private RelayCommand removeUser;
        private RelayCommand saveUser;

        public RelayCommand AddUser
        {
            get
            {
                return addUser ??= new RelayCommand(obj =>
                {
                    UsersStructure _ = new UsersStructure(GetLastId(Users.ToList()),"Новый пользователь", "", "", "", [] );
                    Users.Add(_);
                }, obj => Users != null);
            }
        }

        private int GetLastId(List<UsersStructure> users)
        {
            int res = users.Count + 1;
            return res;
        }

        public RelayCommand RemoveUser
        {
            get
            {
                return removeUser ??= new RelayCommand(obj =>
                {
                    if (Users.Contains(SelectedUser))
                    {
                        Users.Remove(SelectedUser);
                    }
                }, obj => SelectedUser != null);
            }
        }
        public RelayCommand SaveUser
        {
            get
            {
                return saveUser ??= new RelayCommand(async obj =>
                {
                    foreach (var user in Users)
                    {
                        user.Preset = setPreset(user);
                    }
                    UsersUpdated?.Invoke(Users.ToList());
                    dbRequests db = new();
                    await Task.WhenAll(
                        db.DeleteAllRowsDb("Users"),
                        db.DeleteAllRowsDb("Answers")
                    );

                    bool isSaved = true;
                    foreach (var user in Users)
                    {
                        await db.AddDBUsersAsync(user);

                        foreach (var answ in user.Answers)
                        {
                            await db.AddDBAnswAsync(answ);
                        }
                    }

                    if (isSaved)
                    {
                        MessageBox.Show("Пользователи сохранены!");
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка сохранения!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, obj => Users != null);
            }
        }

        private string setPreset(UsersStructure user)
        {
            string result = string.Join("/", user.Answers
                .Where(answer => answer.IsUsed)
                .OrderBy(answer => answer.Priority)
                .ThenBy(answer => answer.Id)
                .Select(answer => answer.Id));
            return result;
        }

        public SettingsViewModel(List<UsersStructure> users)
        {
            Users = new ObservableCollection<UsersStructure>(users);
        }
    }
}
