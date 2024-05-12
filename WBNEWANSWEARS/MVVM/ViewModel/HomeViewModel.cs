using ConsoleApp1;
using System.Collections.ObjectModel;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class HomeViewModel : Core.ViewModel
    {
        private readonly ObservableCollection<UsersStructure> _cards;
        public ObservableCollection<UsersStructure> Cards
        {
            get { return _cards; }
        }

        public HomeViewModel(List<UsersStructure> users)
        {
            _cards = new ObservableCollection<UsersStructure>(users);
        }
    }
}
