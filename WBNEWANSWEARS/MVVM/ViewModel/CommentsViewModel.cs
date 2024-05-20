using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WBNEWANSWEARS.Core;
using static WBNEWANSWEARS.MVVM.ViewModel.ActiveViewModel;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class CommentsViewModel : Core.ViewModel
    {
        public delegate void UsersUpdatedByCommentsEventHandler();
        public event UsersUpdatedByCommentsEventHandler UsersByCommentsUpdated;
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
                Answers = new ObservableCollection<AnswersStructure>(value.Answers);
            }
            get => _selectedUser;
        }
        private ObservableCollection<AnswersStructure> _answers;
        public ObservableCollection<AnswersStructure> Answers
        {
            set
            {
                _answers = value;
                onPropertyChanged(nameof(Answers));
            }
            get => _answers;
        }
        private AnswersStructure _selectedAnswer;
        public AnswersStructure SelectedAnswer
        {
            set
            {
                _selectedAnswer = value;
                onPropertyChanged(nameof(SelectedAnswer));
            }
            get => _selectedAnswer;
        }
        private RelayCommand addAnsw;
        private RelayCommand removeAnsw;
        private RelayCommand saveAnsw;

        public RelayCommand AddAnsw
        {
            get
            {
                return addAnsw ??= new RelayCommand(obj =>
                {
                    AnswersStructure _ = new AnswersStructure("Новый шаблон", 9, false, "", "", SelectedUser.Id);
                    Answers.Add(_);
                    SelectedUser.Answers = Answers.ToList();
                }, obj => SelectedUser != null);
            }
        }
        public RelayCommand RemoveAnsw
        {
            get
            {
                return removeAnsw ??= new RelayCommand(obj =>
                {
                    if (Answers.Contains(SelectedAnswer))
                    {
                        Answers.Remove(SelectedAnswer);
                    }
                    SelectedUser.Answers = Answers.ToList();
                }, obj => SelectedUser != null && SelectedAnswer != null);
            }
        }
        public RelayCommand SaveAnsw
        {
            get
            {
                return saveAnsw ??= new RelayCommand(async obj =>
                {
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
                    UsersByCommentsUpdated?.Invoke();
                    if (isSaved)
                    {
                        MessageBox.Show("Шаблоны сохранены!");
                    }
                    else
                    {
                        MessageBox.Show("Произошла ошибка сохранения!", "Ошибка",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }, obj => Users != null);
            }
        }

        public CommentsViewModel(List<UsersStructure> users)
        {
            Users = new ObservableCollection<UsersStructure>(users);
        }
    }
}
