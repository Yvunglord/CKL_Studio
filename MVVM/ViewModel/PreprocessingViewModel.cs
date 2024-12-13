using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.MVVM.ViewModel
{
    public class PreprocessingViewModel : INotifyPropertyChanged
    {
        private readonly INavigationService _navigationService;
        private object _currentViewModel;

        public object CurrentViewModel
        {
            get => _currentViewModel;
            set 
            {
                _currentViewModel = value;
                OnPropertyChanged(nameof(CurrentViewModel));
            }
        }

        public PreprocessingViewModel(INavigationService navigationService)
        {
            _navigationService = navigationService;
            _navigationService.NavigateTo<OnLoadViewModel>();
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
