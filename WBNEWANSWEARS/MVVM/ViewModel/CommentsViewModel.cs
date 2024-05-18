using ConsoleApp1;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WBNEWANSWEARS.MVVM.ViewModel.ActiveViewModel;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class CommentsViewModel : Core.ViewModel
    {
        private ObservableCollection<UsersStructure> _users;
        public ObservableCollection<UsersStructure> Users
        {
            set => _users = value;
            get => _users;
        }

        public CommentsViewModel(List<UsersStructure> users)
        {
            Users = new ObservableCollection<UsersStructure>(users);
        }
    }
}
