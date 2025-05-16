using CKL_Studio.Presentation.Commands;
using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels.Base;
using CKLDrawing;
using CKLLib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace CKL_Studio.Presentation.ViewModels
{
    public class CKLViewModel : ViewModelBase, IParameterReceiver<CKLView>
    {
        private readonly INavigationService _navigationService;

        private CKLView _cklView;

        public CKLView CKLView
        {
            get => _cklView;
            set => SetField(ref _cklView, value);
        }

        public ICommand NavigateToEntryPointViewCommand => new RelayCommand(NavigateToEntryPointView);
        public ICommand NavigateToCKLCreationViewCommand => new RelayCommand(NavigateToCKLCreationView);

        public CKLViewModel(IServiceProvider serviceProvider, CKLView cklView) : base(serviceProvider)
        {
            _navigationService = serviceProvider.GetRequiredService<INavigationService>();
            _cklView  = cklView;
        }

        public void ReceiveParameter(CKLView parameter)
        {
            _cklView = parameter;
        }

        private void NavigateToEntryPointView() => _navigationService.NavigateTo<EntryPointViewModel>();
        private void NavigateToCKLCreationView() => _navigationService.NavigateTo<CKLCreationViewModel>();
    }
}
