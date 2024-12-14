using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.IO;
using System.Windows.Input;
using System.Windows.Media.Animation;
using CKLLib;
using CKLLib.Operations;

namespace CKL_Studio.MVVM.ViewModel
{
    public class EnterStaticDataViewModel : INotifyPropertyChanged
    {
        private string _name;
        private string _filePath;

        private readonly CKLService _cklService;

        public EnterStaticDataViewModel(CKLService cklService)
        { 
            _cklService = cklService;
        }

        public string Name
        {
            get => _cklService.CKL.Name;
            set
            {
                _cklService.CKL.Name = value;
                OnPropertyChanged(nameof(Name));
            }
        }

        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }
        
        public TimeInterval GlobalInterval
        {
            get => _cklService.CKL.GlobalInterval;
            set
            {
                _cklService.CKL = CKLMath.TimeTransform(_cklService.CKL, value.StartTime, value.EndTime);
                OnPropertyChanged(nameof(GlobalInterval));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
