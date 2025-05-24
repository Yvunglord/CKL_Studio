using CKL_Studio.Presentation.Commands;
using CKL_Studio.Presentation.ViewModels.Base;
using CKLLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CKL_Studio.Presentation.ViewModels.Dialog
{
    public class SelectCklDialogViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<CKL> AvailableCkls { get; }
        private CKL? _selectedCkl;
        private readonly string _currentCklPath;

        public CKL? SelectedCkl
        {
            get => _selectedCkl;
            set
            { 
                _selectedCkl = value;
                OnPropertyChanged();
            }
        }

        private bool? _dialogResult;
        public bool? DialogResult
        {
            get => _dialogResult;
            set
            {
                _dialogResult = value;
                OnPropertyChanged();
            }
        }

        public ICommand OkCommand => new RelayCommand(() =>
        {
            RequestClose?.Invoke(true);
            DialogResult = true;
        });

        public ICommand CancelCommand => new RelayCommand(() =>
        {
            RequestClose?.Invoke(false);
            DialogResult = false;
        });

        public SelectCklDialogViewModel(IEnumerable<CKL> allCkls, string currentCklPath) 
        {
            _currentCklPath = currentCklPath;
            AvailableCkls = new ObservableCollection<CKL>(
                allCkls.Where(c => c.FilePath != _currentCklPath)
                       .GroupBy(c => c.FilePath)
                       .Select(g => g.First())
            );
        }

        public event Action<bool>? RequestClose;

        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
