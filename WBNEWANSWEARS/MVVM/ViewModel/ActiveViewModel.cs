using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using WBNEWANSWEARS.Core;
using WBNEWANSWEARS.MVVM.Model;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class ActiveViewModel : Core.ViewModel
    {
        public API api = new();
        public delegate void UsersAnsweredEventHandler();
        public event UsersAnsweredEventHandler UsersAnswered;
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
                return startAnswer ??= new RelayCommand(async obj =>
                {
                    try
                    {
                        if (UsersSelected.Count == 0)
                        {
                            MessageBox.Show("Пользователи не выбраны!", "Ошибка",
                                MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        else
                        {
                            foreach (UsersStructure user in UsersSelected)
                            {
                                bool result = await api.ProcessUserFeedbacksAsync(user);
                                if (result)
                                {
                                    MessageBox.Show($"Обработка отзывов для пользователя {user.UserName} завершена успешно.");
                                }
                                else
                                {
                                    MessageBox.Show($"Ошибка при обработке отзывов для пользователя {user.UserName}.");
                                }
                            }
                            UsersAnswered?.Invoke();
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при обработке отзывов: {ex.Message}");
                    }
                }, obj => UsersSelected != null);
            }
        }
        public RelayCommand StartSingleAnswer
        {
            get
            {
                return startSingleAnswer ??= new RelayCommand(async obj =>
                {
                    if (obj is UsersStructure user)
                    {
                        try
                        {
                            bool result = await api.ProcessUserFeedbacksAsync(user);
                            if (result)
                            {
                                MessageBox.Show($"Отработка отзывов для пользователя {user.UserName} завершена успешно.");
                                UsersAnswered?.Invoke();
                            }
                            else
                            {
                                MessageBox.Show($"Ошибка при обработке отзывов для пользователя {user.UserName}.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Ошибка при обработке отзывов: {ex.Message}");
                        }
                    }
                }, obj => UsersSelected != null);
            }
        }

        public RelayCommand SelectAllUsersCommand
        {
            get
            {
                return selectAllUsersCommand ??= new RelayCommand( obj =>
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
