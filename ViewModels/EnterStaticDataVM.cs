using CKL_Studio.Services;
using CKLLib;
using System.ComponentModel;
using System.Windows.Input;
using System.IO;

namespace CKL_Studio.ViewModels
{
    public class EnterStaticDataVM : INotifyPropertyChanged
    {
        private readonly CKLService _cklService;
        private string defaultDirectory;

        public EnterStaticDataVM(CKLService cklService)
        {
            _cklService = cklService;
            defaultDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            Name = "Project1";
            UpdateFilePath();
        }

        private string _name;
        public string Name
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
                UpdateFilePath();
            }
        }

        private TimeInterval _globalInterval;
        public TimeInterval GlobalInterval
        {
            get => _globalInterval;
            set
            {
                _globalInterval = value;
                OnPropertyChanged(nameof(GlobalInterval));
            }
        }

        private string _filePath;
        public string FilePath
        {
            get => _filePath;
            set
            {
                _filePath = value;
                OnPropertyChanged(nameof(FilePath));
            }
        }

        private string _selectedTimeUnit;
        public string SelectedTimeUnit
        {
            get => _selectedTimeUnit;
            set
            {
                _selectedTimeUnit = value;
                OnPropertyChanged(nameof(SelectedTimeUnit));
            }
        }

        public ICommand SaveStaticDataCommand => new RelayCommand(() => SaveStaticData());

        private void SaveStaticData()
        {
            _cklService.UpdateName(Name);
            _cklService.UpdateGlobalInterval(GlobalInterval);
        }
        private string GetUniqueFileName(string baseFileName, string extension)
        {
            string fileName = baseFileName + extension;
            string fullPath = Path.Combine(defaultDirectory, fileName);
            int counter = 1;

            while (File.Exists(fullPath))
            {
                fileName = $"{baseFileName}{counter}{extension}";
                fullPath = Path.Combine(defaultDirectory, fileName);
                counter++;
            }

            return fileName;
        }
        private void UpdateFilePath()
        {
            string baseFileName = Name;
            string extension = ".ckl"; 

            if (!baseFileName.EndsWith(extension))
            {
                baseFileName += extension;
            }

            string uniqueFileName = GetUniqueFileName(Path.GetFileNameWithoutExtension(baseFileName), extension);
            FilePath = Path.Combine(defaultDirectory, uniqueFileName);
        }


        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
