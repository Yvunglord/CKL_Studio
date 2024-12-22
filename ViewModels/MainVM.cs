using CKL_Studio.Services;
using CKLLib;
using System.ComponentModel;
using System.Windows.Input;

namespace CKL_Studio.ViewModels
{
    public class MainVM : INotifyPropertyChanged
    {
        private readonly NavigationService _navigationService;
        private readonly CKLService _cklService;

        public MainVM(NavigationService navigationService, CKLService cklService)
        {
            _navigationService = navigationService;
            _cklService = cklService;
        }

        // Команды для навигации между представлениями
        public ICommand NavigateToPreprocessingWindowCommand => new RelayCommand(() => _navigationService.NavigateTo(ViewType.PreprocessingWindow));

        // Свойства для управления состоянием ViewModel
        public CKL CKLInstance => _cklService.CKLInstance;

        // Реализация INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
