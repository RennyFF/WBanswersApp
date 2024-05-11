using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WBNEWANSWEARS.Core;
using WBNEWANSWEARS.MVVM.ViewModel;
using WBNEWANSWEARS.ViewModel;

namespace WBNEWANSWEARS.Services
{
    public interface INavigationService
    {
        Core.ViewModel CurrentView { get; }
        void Navigate<T>() where T : Core.ViewModel;
    }
    public class NavigationService : ObservableObject, INavigationService
    {
        private readonly Func<Type, Core.ViewModel> _viewModelFactory;
        private Core.ViewModel _currentView;

        public Core.ViewModel CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                onPropertyChanged();
            }
        }

        public NavigationService(Func<Type, Core.ViewModel> viewModelFactory)
        {
            _viewModelFactory = viewModelFactory;
            Core.ViewModel base_viewModel = _viewModelFactory.Invoke(typeof(HomeViewModel));
            CurrentView = base_viewModel;
        }

        public void Navigate<TViewModel>() where TViewModel : Core.ViewModel
        {
            Core.ViewModel viewModel = _viewModelFactory.Invoke(typeof(TViewModel));
            CurrentView = viewModel;
        }
    }
}
