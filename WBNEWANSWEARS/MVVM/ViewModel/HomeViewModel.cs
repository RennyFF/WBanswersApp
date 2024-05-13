using ConsoleApp1;
using System.Collections.ObjectModel;

namespace WBNEWANSWEARS.MVVM.ViewModel
{
    class HomeViewModel : Core.ViewModel
    {
        private ObservableCollection<UsersStructure> _cards;
        public ObservableCollection<UsersStructure> Cards
        {
            set => _cards = value; 
            get => _cards; 
        }

        public HomeViewModel(List<UsersStructure> users)
        {
            Cards = new ObservableCollection<UsersStructure>(users);
        }
    }
}
