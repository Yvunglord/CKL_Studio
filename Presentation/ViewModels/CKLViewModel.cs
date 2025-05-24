using CKL_Studio.Common.Interfaces;
using CKL_Studio.Common.Interfaces.CKLInterfaces;
using CKL_Studio.Common.Interfaces.Factories;
using CKL_Studio.Infrastructure.Services;
using CKL_Studio.Infrastructure.Static;
using CKL_Studio.Presentation.Commands;
using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels.Base;
using CKLDrawing;
using CKLLib;
using CKLLib.Operations;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ICommand CloseTabCommand => new RelayCommand<CKLView>(CloseTab);

        public CKLViewModel(IServiceProvider serviceProvider, CKLView cklView) : base(serviceProvider)
        {
            _navigationService = serviceProvider.GetRequiredService<INavigationService>();  
            _dialogService = serviceProvider.GetRequiredService<IDialogService>();
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
                            if (related != null /*&& BinaryCKLOperationsValidator.CanPerformOperation(MainCKLView.Ckl, related)*/)
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

        private void CloseTab(CKLView? viewToClose)
        {
            if (viewToClose != null)
            { 
                OpenedCKLViews.Remove(viewToClose);

                if (SelectedCKLView == viewToClose)
                    SelectedCKLView = OpenedCKLViews.LastOrDefault();
            }
        }
    }
}
