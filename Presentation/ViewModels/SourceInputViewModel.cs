using CKL_Studio.Presentation.Commands;
using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels.Base;
using CKLLib;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;

namespace CKL_Studio.Presentation.ViewModels
{
    public class SourceInputViewModel : ViewModelBase, IParameterReceiver<CKL>
    {
        private readonly INavigationService _navigationService;

        private CKL _ckl;

        public CKL CKL
        {
            get => _ckl;
            set => SetField(ref _ckl, value);
        }

        public ICommand NavigateToRelationInputCommand => new RelayCommand(NavigateToRelationInput);
        public ICommand GoBackCommand => new RelayCommand(GoBack);

        public SourceInputViewModel(IServiceProvider serviceProvider, CKL ckl) : base(serviceProvider)
        {
            _navigationService = serviceProvider.GetRequiredService<INavigationService>();

            _ckl = ckl;
        }

        public void ReceiveParameter(CKL parameter)
        {
            _ckl = parameter;
        }

        private void NavigateToRelationInput() => _navigationService.NavigateTo<RelationInputViewModel, CKL>(CKL);
        private void GoBack() => _navigationService.GoBack();
    }
}
