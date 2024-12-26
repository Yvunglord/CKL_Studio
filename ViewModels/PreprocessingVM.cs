using CKL_Studio.EnterDynamicData;
using CKL_Studio.EnterStaticData;
using CKL_Studio.OnLoad;
using CKL_Studio.Services;
using CKL_Studio.Views;
using CKLLib;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;

namespace CKL_Studio.ViewModels
{
    public class PreprocessingVM : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private readonly CKLService _cklService;
        private UserControl _currentView;

        public PreprocessingVM(NavigationService navigationService, CKLService cklService)
        {
            _navigationService = navigationService;
            _cklService = cklService;
            CurrentView = new OnLoadView();
        }

        public UserControl CurrentView
        {
            get => _currentView;
            set
            {
                _currentView = value;
                OnPropertyChanged(nameof(CurrentView));
            }
        }

        public ICommand NavigateToOnLoadViewCommand => new RelayCommand(() => CurrentView = new OnLoadView());
        public ICommand NavigateToEnterStaticDataViewCommand => new RelayCommand(() => CurrentView = new EnterStaticDataView());
        public ICommand NavigateToEnterDynamicDataViewCommand => new RelayCommand(() => CurrentView = new EnterDynamicDataView());
        public ICommand NavigateToMakeRelationViewCommand => new RelayCommand(() => CurrentView = new MakeRelationView());
        public ICommand NavigateToMainViewCommand => new RelayCommand(() => _navigationService.NavigateTo(ViewType.MainView));
        public ICommand NavigateToEnterDynamicDataFromHerselfCommand => new RelayCommand(NavigateToEnterDynamicDataFromHerself);

        private void NavigateToEnterDynamicDataFromHerself(object parameter)
        {
            // Логика для перехода со второго множества на первое
            CurrentView = new EnterDynamicDataView();
        }

        public CKL CKLInstance => _cklService.CKLInstance;

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
