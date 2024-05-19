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
                    AnswersStructure _ = new AnswersStructure(GetLastId(SelectedUser.Answers), "Новый шаблон", 9, false, "", "", SelectedUser.Id);
                    Answers.Add(_);
                    SelectedUser.Answers = Answers.ToList();
                }, obj => SelectedUser != null);
            }
        }

        private int GetLastId(List<AnswersStructure> answ)
        {
            int res = answ.Count + 1;
            return res;
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
                return saveAnsw ??= new RelayCommand(obj =>
                {
                    (Application.Current as App).USERS = new List<UsersStructure>(Users);
                    dbRequests db = new();
                    db.DeleteAllRowsDb("Users");
                    db.DeleteAllRowsDb("Answers");
                    bool isSaved = true;
                    foreach (var user in Users)
                    {
                        db.AddDBUsers(user);
                        foreach (var answ in user.Answers)
                        {
                            db.AddDBAnsw(answ);
                        }
                    }
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
