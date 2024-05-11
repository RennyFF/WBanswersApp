using ConsoleApp1;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class HomeViewModel : Core.ViewModel
    {
        private readonly List<UsersStructure> _users;

        public HomeViewModel(List<UsersStructure> users)
        {
            _users = users;
        }
    }
}
