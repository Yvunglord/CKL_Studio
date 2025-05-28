using CKL_Studio.Common.Interfaces;
using CKL_Studio.Common.Interfaces.CKLInterfaces;
using CKL_Studio.Common.Interfaces.Factories;
using CKL_Studio.Infrastructure.Services;
using CKL_Studio.Infrastructure.Static;
using CKL_Studio.Presentation.Commands;
using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels.Base;
using CKL_Studio.Presentation.ViewModels.Dialog;
using CKL_Studio.Presentation.Windows.Dialogs;
using CKLDrawing;
using CKLLib;
using CKLLib.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace CKL_Studio.Presentation.ViewModels
{
    public class CKLViewModel : ViewModelBase, IParameterReceiver<CKLView>
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly ISolutionExplorerDataServiceFactory _serviceFactory;
        private readonly IDataService<FileData> _fileService;
        private IDataService<CKL>? _solutionExplorerService;

        private CKLView _mainCklView;
        private CKL? _selectedSolutionItem;
        private CKLView? _selectedCklView;
        private ObservableCollection<CKLView> _openedCKLViews = new ObservableCollection<CKLView>();

        public CKLView MainCKLView
        {
            get => _mainCklView;
            set => SetField(ref _mainCklView, value);
        }

        public CKL? SelectedSolutionItem
        {
            get => _selectedSolutionItem;
            set => SetField(ref _selectedSolutionItem, value);
        }

        public ObservableCollection<CKL> SolutionItems => (_solutionExplorerService as SolutionExplorerDataService).Items ?? new ObservableCollection<CKL>();

        public CKLView? SelectedCKLView
        {
            get => _selectedCklView;
            set => SetField(ref _selectedCklView, value);
        }

        public ObservableCollection<CKLView> OpenedCKLViews
        {
            get => _openedCKLViews;
            set => SetField(ref _openedCKLViews, value);
        }

        public ICommand NavigateToEntryPointViewCommand => new RelayCommand(NavigateToEntryPointView);
        public ICommand NavigateToCKLCreationViewCommand => new RelayCommand(NavigateToCKLCreationView);
        public ICommand OpenSolutionItemCommand => new RelayCommand(OpenSelectedItem);
        public ICommand DeleteSolutionItemCommand => new RelayCommand(DeleteSolutionItem);
        public ICommand CloseTabCommand => new RelayCommand<CKLView>(CloseTab);
        public ICommand UnionCommand => new RelayCommand(() => PerformBinaryOperation(CKLMath.Union));
        public ICommand IntersectionCommand => new RelayCommand(() => PerformBinaryOperation(CKLMath.Intersection));
        public ICommand DifferenceCommand => new RelayCommand(() => PerformBinaryOperation(CKLMath.Difference));
        public ICommand CompositionCommand => new RelayCommand(() => PerformBinaryOperation(CKLMath.Composition));
        public ICommand SemanticUnionCommand => new RelayCommand(() => PerformBinaryOperation(CKLMath.SemanticUnion));
        public ICommand SemanticIntersectionCommand => new RelayCommand(() => PerformBinaryOperation(CKLMath.SemanticIntersection));
        public ICommand SemanticDifferenceCommand => new RelayCommand(() => PerformBinaryOperation(CKLMath.SemanticDifference));
        public ICommand InversionCommand => new RelayCommand(() => PerformUnaryOperation(CKLMath.Inversion));
        public ICommand TranspositionCommand => new RelayCommand(() => PerformUnaryOperation(CKLMath.Tranposition));
        public ICommand TimeTransformCommand => new RelayCommand(() => PerformTimeOperation(CKLMath.TimeTransform));
        public ICommand LeftPrecedenceCommand => new RelayCommand(() => PerformParameterizedTimeOperation(CKLMath.LeftPrecedence));
        public ICommand LeftContinuationCommand => new RelayCommand(() => PerformParameterizedTimeOperation(CKLMath.LeftContinuation));
        public ICommand RightPrecedenceCommand => new RelayCommand(() => PerformParameterizedTimeOperation(CKLMath.RightPrecedence));
        public ICommand RightContinuationCommand => new RelayCommand(() => PerformParameterizedTimeOperation(CKLMath.RightContinuation));
        public ICommand ScalePlusCommand => new RelayCommand(ScalePlus);
        public ICommand ScaleMinusCommand => new RelayCommand(ScaleMinus);

        public CKLViewModel(IServiceProvider serviceProvider, CKLView cklView) : base(serviceProvider)
        {
            _navigationService = serviceProvider.GetRequiredService<INavigationService>();  
            _dialogService = serviceProvider.GetRequiredService<IDialogService>();
            _fileService = serviceProvider.GetRequiredService<IDataService<FileData>>();
            _mainCklView  = cklView;
            _serviceFactory = serviceProvider.GetRequiredService<ISolutionExplorerDataServiceFactory>();
        }

        public void ReceiveParameter(CKLView parameter) 
        {
            _mainCklView = parameter;
            _solutionExplorerService = _serviceFactory.Create(_mainCklView.Ckl);
            _openedCKLViews.Add(_mainCklView);
            LoadSolutionItems();
        }

        private void NavigateToEntryPointView() => _navigationService.NavigateTo<EntryPointViewModel>();
        private void NavigateToCKLCreationView() => _navigationService.NavigateTo<CKLCreationViewModel>();

        private void LoadSolutionItems()
        {
            SolutionItems.Clear();

            _solutionExplorerService.Add(MainCKLView.Ckl);
            var root = Path.GetDirectoryName(MainCKLView.Ckl.FilePath);
            if (root != null)
            { 
                foreach (var path in Directory.GetFiles(root, "*.ckl"))
                {
                    if (path != MainCKLView.Ckl.FilePath)
                    {
                        try
                        {
                            var related = CKL.GetFromFile(path);
                            related.FilePath = path;
                            if (related != null && BinaryCKLOperationsValidator.CanPerformOperation(MainCKLView.Ckl, related))
                            {
                                _solutionExplorerService.Add(related);
                            }
                        }

                        catch (Exception ex)
                        {
                            _dialogService.ShowMessage($"Ошибка при добавлении файла: {ex.Message}");
                        }
                    }
                }
            }
        }

        private void OpenSelectedItem()
        { 
            if (SelectedSolutionItem != null)
            {
                var alreadyOpened = OpenedCKLViews.FirstOrDefault(v => v.Ckl.FilePath == SelectedSolutionItem.FilePath);
                if (alreadyOpened != null)
                {
                    SelectedCKLView = alreadyOpened;
                    return;
                }

                var newView = new CKLView(SelectedSolutionItem);
                OpenedCKLViews.Add(newView);
                SelectedCKLView = newView;
            }
        }

        private void DeleteSolutionItem()
        {
            _dialogService.ShowMessage("Вы собираетесь безвозвратно удалить файл! \n Хотите продолжить?");

            if (SelectedSolutionItem != null)
            {
                string path = SelectedSolutionItem.FilePath;
                File.Delete(path);              
                MainCKLView = new CKLView(new CKL() { FilePath = path});
            }

            LoadSolutionItems();
        }

        private void CloseTab(CKLView? viewToClose)
        {
            if (viewToClose != null)
            { 
                OpenedCKLViews.Remove(viewToClose);

                if (SelectedCKLView == viewToClose)
                    SelectedCKLView = OpenedCKLViews.LastOrDefault();
            }
        }

        private int _currentDelCoast = 0;
        private TimeDimentions _currentDimentions = default;
        private static readonly int[] TIME_DIMENTIONS_CONVERT = new int[] { 1000, 1000, 1000, 60, 60, 24, 7 };

        private void ScalePlus()
        {

            if (_currentDelCoast != _selectedCklView.DelCoast)
            {
                _currentDelCoast = _selectedCklView.DelCoast;
                _currentDimentions = _selectedCklView.TimeDimention;
            }

            if (_currentDelCoast * 2 >= TIME_DIMENTIONS_CONVERT[(int)_currentDimentions]
                   && !_currentDimentions.Equals(TimeDimentions.WEEKS))
            {
                _currentDelCoast = 1;
                _currentDimentions = (TimeDimentions)(int)_currentDimentions + 1;
            }
            else _currentDelCoast *= 2;


            _selectedCklView.ChangeDelCoast(_currentDimentions, _currentDelCoast);
        }

        private void ScaleMinus()
        {

            if (_currentDelCoast != _selectedCklView.DelCoast)
            {
                _currentDelCoast = _selectedCklView.DelCoast;
                _currentDimentions = _selectedCklView.TimeDimention;
            }

            if (_currentDelCoast / 2 == 0
                    && !_currentDimentions.Equals(TimeDimentions.NANOSECONDS))
            {
                _currentDimentions = (TimeDimentions)(int)_currentDimentions - 1;
                _currentDelCoast = TIME_DIMENTIONS_CONVERT[(int)_currentDimentions] / 2;
            }
            else if (_currentDelCoast >= 2) _currentDelCoast /= 2;

            _selectedCklView.ChangeDelCoast(_currentDimentions, _currentDelCoast);
        }

        private void PerformBinaryOperation(Func<CKL, CKL, CKL> operation)
        {
            if (SelectedCKLView == null)
            {
                _dialogService.ShowMessage("Сначала выберите основную CKL в рабочей области");
                return;
            }

            var currentCkl = SelectedCKLView.Ckl;
            var allCkls = SolutionItems.Concat(OpenedCKLViews.Select(v => v.Ckl)).Distinct().ToList();

            var dialogVm = new SelectCklDialogViewModel(allCkls, currentCkl.FilePath);

            dialogVm.RequestClose += result =>
            {
                if (result == true && dialogVm.SelectedCkl != null)
                {
                    try
                    {
                        var resultCkl = operation(currentCkl, dialogVm.SelectedCkl);
                        CKL.Save(resultCkl);
                        AddResultToWorkspace(resultCkl);
                    }
                    catch (Exception ex)
                    {
                        _dialogService.ShowMessage($"Ошибка выполнения бинарной операции: {ex.Message}");
                    }
                }
            };

            _dialogService.ShowDialog<SelectCklDialog>(dialog => dialog.DataContext = dialogVm);
        }

        private void PerformUnaryOperation(Func<CKL, CKL> operation)
        {
            if (SelectedCKLView == null)
            {
                _dialogService.ShowMessage("Сначала выберите основную CKL в рабочей области");
                return;
            }

            try
            {
                var resultCkl = operation(SelectedCKLView.Ckl);
                CKL.Save(resultCkl);
                AddResultToWorkspace(resultCkl);
            }
            catch (Exception ex)
            { 
                _dialogService.ShowMessage($"Ошибка выполнения унарной операции: {ex.Message}");
            }
        }

        public void PerformTimeOperation(Func<CKL, TimeInterval, CKL> operation)
        {
            var dialog = new TimeOperationDialog();
            if (dialog.ShowDialog() == true)
            {
                var stTime = double.Parse(dialog.TextBox1Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                var enTime = double.Parse(dialog.TextBox2Value, NumberStyles.Any, CultureInfo.InvariantCulture);

                try
                {
                    var resultCkl = operation(SelectedCKLView.Ckl, new TimeInterval(stTime, enTime));
                    CKL.Save(resultCkl);
                    AddResultToWorkspace(resultCkl);
                }
                catch (ArgumentException ex)
                {
                    _dialogService.ShowMessage($"Uncorrect data: {ex.Message}", "Error");
                }
            }
        }

        public void PerformParameterizedTimeOperation(Func<CKL, TimeInterval, double, CKL> operation)
        {
            var dialog = new ParameterizedTimeOperationDialog();
            if (dialog.ShowDialog() == true)
            {
                var stTime = double.Parse(dialog.TextBox1Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                var enTime = double.Parse(dialog.TextBox2Value, NumberStyles.Any, CultureInfo.InvariantCulture);
                var t = double.Parse(dialog.TextBox3Value, NumberStyles.Any, CultureInfo.InvariantCulture);

                try
                {
                    var resultCKl = operation(SelectedCKLView.Ckl, new TimeInterval(stTime, enTime), t);
                    CKL.Save(resultCKl);
                    AddResultToWorkspace(resultCKl);
                }
                catch (ArgumentException ex)
                {
                    _dialogService.ShowMessage($"Uncorrect data: {ex.Message}", "Error");
                }
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

        private void AddResultToWorkspace(CKL result)
        {
            AddFile(result.FilePath);
            var newView = new CKLView(result);
            OpenedCKLViews.Add(newView);
            SelectedCKLView = newView;

            if (_solutionExplorerService != null && !SolutionItems.Contains(result))
            {
                _solutionExplorerService.Add(result);
            }
        }
    }
}
