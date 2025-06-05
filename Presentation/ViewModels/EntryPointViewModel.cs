using CKL_Studio.Common.Interfaces;
using CKL_Studio.Infrastructure.Services;
using CKL_Studio.Presentation.Commands;
using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels.Base;
using CKLDrawing;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace CKL_Studio.Presentation.ViewModels
{
    public class EntryPointViewModel : ViewModelBase, IParameterReceiver<CKLView>
    {
        private readonly INavigationService _navigationService;
        private readonly IDataService<FileData> _fileService;
        private readonly IDataService<string> _historyService;
        private readonly IDialogService _dialogService;
        private readonly IOpenCklService _openCklService;

        private CKLView? _cklView;
        private string _searchText = string.Empty;
        private FileData? _selectedFile;
        
        public CKLView? CKLView
        {
            get => _cklView;
            private set => SetField(ref _cklView, value);
        }

        public FileData? SelectedFile
        {
            get => _selectedFile;
            set => SetField(ref _selectedFile, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            { 
                if (SetField(ref _searchText, value))
                    FilterFiles();
            }
        }

        public ObservableCollection<FileData> FilteredFiles =>
            (_fileService as FileDataService)?.FilteredFiles ?? new ObservableCollection<FileData>();
        public ObservableCollection<string> SearchHistory =>
            (_historyService as SearchHistoryDataService)?.Items ?? new ObservableCollection<string>();

        public ICommand NavigateToCKLCreationCommand => new RelayCommand(NavigateToCKLCreation);
        public ICommand NavigateToCKLViewCommand => new AsyncRelayCommand(BrowseAsync);
        public ICommand SaveCommand => new RelayCommand(Save);
        public ICommand OpenFileCommand => new RelayCommand(async () => await OpenSelectedFileAsync());
        public ICommand PinFileCommand => new RelayCommand<FileData>(PinFile);
        public ICommand CopyFilePathCommand => new RelayCommand<FileData>(CopyFilePath);
        public ICommand RemoveFileCommand => new RelayCommand<FileData>(RemoveFile);

        public EntryPointViewModel(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            _navigationService = GetService<INavigationService>();
            _fileService = GetService<IDataService<FileData>>();
            _historyService = GetService<IDataService<string>>(); 
            _dialogService = GetService<IDialogService>();
            _openCklService = GetService<IOpenCklService>();

            LoadData();
        }

        private void LoadData()
        {
            var fileService = _fileService as FileDataService;
            fileService?.Load();

            var historyService = _historyService as SearchHistoryDataService;
            historyService?.Load();
        }

        public void ReceiveParameter(CKLView parameter)
        {
            CKLView = parameter;
            CommandManager.InvalidateRequerySuggested();
        }

#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
        private void NavigateToCKLView() => _navigationService.NavigateTo<CKLViewModel, CKLView>(CKLView);
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.

        private void NavigateToCKLCreation() => _navigationService.NavigateTo<CKLCreationViewModel>();

        private async Task<string?> GetCklPathAsync()
        {
            return await Task.Run(() => _dialogService.ShowOpenFileDialog(Constants.CKL_FILE_DIALOG_FILTER, Constants.DEFAULT_FILE_PATH));
        }

        private async Task<CKLView?> LoadCklAsync(string path)
        {
            return await Task.Run(() => _openCklService.Load(path, Application.Current.Dispatcher));
        }

        private async Task OpenFileAsync(FileData? file)
        {
            if (file?.Path == null) return;

            try
            {
                _cklView = await LoadCklAsync(file.Path);
                NavigateToCKLView();
            }
            catch
            {
                _dialogService.ShowMessage($"Ошибка открытия файла:\n Возможно файл был перенесен или удален");
                RemoveFile(file);
                Save();
            }
        }

        private async Task OpenSelectedFileAsync()
        {
            if (SelectedFile == null) return;
            await OpenFileAsync(SelectedFile);
        }

        public async Task BrowseAsync()
        { 
            var path = await GetCklPathAsync();
            if (path != null)
            {
                _cklView = await LoadCklAsync(path);
                AddFile(path);
                NavigateToCKLView();
            }
        }

        public void FilterFiles()
        {
            var fileService = _fileService as FileDataService;
            fileService?.FilterFiles(_searchText);
        }

        public void UpdateSearchHistory()
        {
            var historyService = _historyService as SearchHistoryDataService;
            historyService?.Update(_searchText);
        }

        public void Save()
        {
            UpdateSearchHistory();

            var fileService = _fileService as FileDataService;
            fileService?.Save();

            var historyService = _historyService as SearchHistoryDataService;
            historyService?.Save();

            LoadData();
        }

        public void AddFile(string path)
        {
            var file = new FileInfo(path);
            var fileData = new FileData(file)
            {
                LastAccess = DateTime.Now
            };

            if (_fileService.Get(path) == null)
            {
                _fileService.Add(fileData);
            }
            else
            {
                _fileService.Update(fileData);
            }
            Save();
        }

        public void RemoveFile(FileData? file) 
        {         
            if (file != null)
                _fileService.Delete(file);   
            Save();
        }

        private void PinFile(FileData? file)
        {
            if (file != null)
            {
                file.IsPinned = !file.IsPinned;
                Save();
            }
        }

        private void CopyFilePath(FileData? file)
        {
            if (file != null)
            {
                Clipboard.SetText(file.Path);
                //_dialogService.ShowMessage("Путь скопирован в буфер обмена");
            }
        }
    }
}