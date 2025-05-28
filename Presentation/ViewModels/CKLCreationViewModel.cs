using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKLLib;
using CKLDrawing;
using Microsoft.Extensions.DependencyInjection;
using System.Windows.Input;
using CKL_Studio.Presentation.Commands;
using CKL_Studio.Common.Interfaces;
using System.IO;
using System.ComponentModel;
using CKL_Studio.Common.Interfaces.CKLInterfaces;
using System.Windows;
using CKL_Studio.Infrastructure.Services;
namespace CKL_Studio.Presentation.ViewModels
{
    public class CKLCreationViewModel : ViewModelBase, IParameterReceiver<CKL>, IDataErrorInfo
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IJSONToCklСonversion _conversionService;
        private readonly INamingService _namingService;
        private readonly IDataService<FileData> _fileService;

        private CKL _ckl;
        private CKLView? _cklView;
        private string _name = "Project1";
        private bool _isFilePathManuallySet;

        public CKLView? CKLView
        {
            get => _cklView;
            set => SetField(ref _cklView, value);
        }

        public CKL CKL
        {
            get => _ckl;
            set
            {
                SetField(ref _ckl, value);
            }
        }

        public string Name
        {
            get => _name;
            set
            {
                var sanitizedValue = _namingService.SanitizeName(value);
                if (SetField(ref _name, value) && !_isFilePathManuallySet)
                {
                    UpdateFilePathFromName();
                }
            }
        }

        public string FilePath
        {
            get => _ckl.FilePath;
            set
            {
                if (_ckl.FilePath != value)
                {
                    _ckl.FilePath = value;
                    OnPropertyChanged(nameof(FilePath));

                    if (_isFilePathManuallySet)
                    {
                        var newName = Path.GetFileNameWithoutExtension(value);
                        if (newName != Name)
                        {
                            Name = newName;
                        }
                    }
                }
            }
        }

        public IEnumerable<TimeDimentions> TimeDimensions =>
            Enum.GetValues(typeof(TimeDimentions)).Cast<TimeDimentions>();

        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand NavigateToSourceInputCommand => new RelayCommand(NavigateToSourceInput, CanNavigateToSourceInput);
        public ICommand NavigateToCKLViewCommand => new RelayCommand(async () => await OpenCKLView());
        public ICommand SelectFilePathCommand => new RelayCommand(SelectFilePath);

        #region IDataErrorInfo
        public string Error => string.Empty;

        public string this[string columnName]
        {
            get
            {
                string error = string.Empty;
                switch (columnName)
                {
                    case nameof(Name):
                        if (string.IsNullOrWhiteSpace(_name))
                            error = "Имя файла обязательно";
                        else if (_name.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                            error = "Недопустимые символы в имени файла";
                        break;

                    case nameof(_ckl.GlobalInterval.StartTime):
                        if (_ckl.GlobalInterval.StartTime < 0)
                            error = "Время не может быть отрицательным";
                        else if (_ckl.GlobalInterval.StartTime > _ckl.GlobalInterval.EndTime)
                            error = "Начальное время больше конечного";
                        break;

                    case nameof(_ckl.GlobalInterval.EndTime):
                        if (_ckl.GlobalInterval.EndTime < 0)
                            error = "Время не может быть отрицательным";
                        else if (_ckl.GlobalInterval.EndTime < _ckl.GlobalInterval.StartTime)
                            error = "Конечное время меньше начального";
                        break;

                    case nameof(FilePath):
                        {
                            if (string.IsNullOrWhiteSpace(_ckl.FilePath))
                            {
                                error = "Путь к файлу обязателен";
                                break;
                            }

                            try
                            {
                                var directory = Path.GetDirectoryName(_ckl.FilePath);
                                var fileName = Path.GetFileName(_ckl.FilePath);

                                if (fileName.IndexOfAny(Path.GetInvalidFileNameChars()) >= 0)
                                {
                                    error = "Недопустимые символы в имени файла";
                                    break;
                                }

                                if (string.IsNullOrEmpty(directory) || !Directory.Exists(directory))
                                {
                                    error = "Указанная директория не существует";
                                    break;
                                }

                                if (File.Exists(_ckl.FilePath))
                                {
                                    error = "Файл с таким именем уже существует";
                                }
                            }
                            catch (Exception ex) when (ex is ArgumentException || ex is NotSupportedException)
                            {
                                error = "Недопустимый формат пути";
                            }
                            catch (PathTooLongException)
                            {
                                error = "Слишком длинный путь к файлу";
                            }
                            break;
                        }
                }
                return error;
            }
        }
        #endregion

        public bool HasErrors => !string.IsNullOrEmpty(AllErrors);

        public string AllErrors
        {
            get
            {
                var errors = new[]
                {
                this[nameof(Name)],
                this[nameof(CKL.GlobalInterval.StartTime)],
                this[nameof(CKL.GlobalInterval.EndTime)],
                this[nameof(FilePath)]
            };
                return string.Join(Environment.NewLine, errors.Where(e => !string.IsNullOrEmpty(e)));
            }
        }

        public CKLCreationViewModel(IServiceProvider serviceProvider, CKL ckl) : base(serviceProvider)
        {
            _navigationService = serviceProvider.GetRequiredService<INavigationService>();
            _dialogService = serviceProvider.GetRequiredService<IDialogService>();
            _namingService = serviceProvider.GetRequiredService<INamingService>();
            _conversionService = serviceProvider.GetRequiredService<IJSONToCklСonversion>();
            _fileService = serviceProvider.GetRequiredService<IDataService<FileData>>();

            _ckl = ckl ?? new CKL();

            InitializeDefaults();

            this.PropertyChanged += (s, e) =>
            {
                CommandManager.InvalidateRequerySuggested();

                if (e.PropertyName is nameof(Name)
                    or nameof(CKL.GlobalInterval.StartTime)
                    or nameof(CKL.GlobalInterval.EndTime)
                    or nameof(FilePath))
                {
                    OnPropertyChanged(nameof(AllErrors));
                    OnPropertyChanged(nameof(HasErrors));
                }
            };
        }

        private void InitializeDefaults()
        {
            if (string.IsNullOrEmpty(_ckl.FilePath))
            {
                var defaultPath = _namingService.GeneratePath("Project", Constants.DEFAULT_CKL_FILE_PATH);
                _name = Path.GetFileNameWithoutExtension(defaultPath);
                FilePath = defaultPath;
            }
            else 
            {
                _name = Path.GetFileNameWithoutExtension(_ckl.FilePath);
                _isFilePathManuallySet = true;
            }
        }

        public void ReceiveParameter(CKL parameter)
        {
            _ckl = parameter ?? new CKL();
        }

        private void NavigateToCKLView() =>
            _navigationService.NavigateTo<CKLViewModel, CKLView>(CKLView ?? throw new InvalidOperationException("CKLView is null"));

        //private bool CanNavigateToCKLView() => CKLView != null;

        private void NavigateToSourceInput() =>
            _navigationService.NavigateTo<SourceInputViewModel, CKL>(_ckl);

        private bool CanNavigateToSourceInput()
        {
            return string.IsNullOrEmpty(this[nameof(Name)]) &&
                   string.IsNullOrEmpty(this[nameof(CKL.GlobalInterval.StartTime)]) &&
                   string.IsNullOrEmpty(this[nameof(CKL.GlobalInterval.EndTime)]) &&
                   string.IsNullOrEmpty(this[nameof(FilePath)]);
        }

        private void GoBack() => _navigationService.GoBack();

        private void UpdateFilePathFromName()
        {
            if (string.IsNullOrEmpty(_name)) return;

            FilePath = _namingService.UpdatePath(_ckl.FilePath, _name, _isFilePathManuallySet);
        }

        private void SelectFilePath()
        {
            var result = _dialogService.ShowSaveFileDialog($"{_name}.ckl", Constants.DEFAULT_CKL_FILE_PATH, Constants.CKL_FILE_DIALOG_FILTER);

            if (!string.IsNullOrEmpty(result))
            { 
                _isFilePathManuallySet = true;
                Name = Path.GetFileNameWithoutExtension(result);
                FilePath = result;
            }
        }

        private async Task<string?> GetCklPathAsync()
        {
            return await Task.Run(() => _dialogService.ShowOpenFileDialog(Constants.JSON_FILE_DIALOG_FILTER, Constants.DEFAULT_FILE_PATH));
        }

        private async Task<bool> BrowseAsync()
        { 
            var path = await GetCklPathAsync();
            if (path != null)
            {
                try
                {
                    _ckl = _conversionService.Convert(path);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            return false;
        }

        private async Task OpenCKLView()
        {
            try
            {
                bool success = await BrowseAsync();
                if (!success) return;

                CKLView = new CKLView(_ckl);
                if (CKLView != null)
                {
                    AddFile(_ckl.FilePath);
                    CKL.Save(_ckl);
                    NavigateToCKLView();
                }              
            }
            catch (Exception ex)
            {
                _dialogService.ShowMessage($"Ошибка открытия файла: {ex.Message}");
            }
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

        private void Save()
        {
            var fileService = _fileService as FileDataService;
            fileService?.Save();
        }
    }
}
