using WBNEWANSWEARS.Core;
using WBNEWANSWEARS.MVVM.ViewModel;
using WBNEWANSWEARS.Services;

namespace WBNEWANSWEARS.ViewModel
{
    class MainViewModel : Core.ViewModel
    {
        private INavigationService _navigation;

        public INavigationService Navigation
        {
            get => _navigation;
            set
            {
                _navigation = value;
                onPropertyChanged();
            }
        }
        public RelayCommand NavigateHomeCommand {get; set; }
        public RelayCommand NavigateSettingsCommand {get; set; }
        public RelayCommand NavigateActiveCommand {get; set; }
        public RelayCommand NavigateCommentsCommand { get; set; }
        public MainViewModel(INavigationService navService)
        {
            Navigation = navService;
            NavigateHomeCommand = new RelayCommand(o => { Navigation.Navigate<HomeViewModel>(); }, o => true);
            NavigateSettingsCommand = new RelayCommand(o => { Navigation.Navigate<SettingsViewModel>(); }, o => true);
            NavigateActiveCommand = new RelayCommand(o => { Navigation.Navigate<ActiveViewModel>(); }, o => true);
            NavigateCommentsCommand = new RelayCommand(o => { Navigation.Navigate<CommentsViewModel>(); }, o => true);
        }
    }
}
