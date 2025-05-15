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
        private Pair? _pair;
        private string _firstValue = string.Empty;
        private string _secondValue = string.Empty;    
        private string _thirdValue = string.Empty;

        public CKL CKL
        {
            get => _ckl;
            set => SetField(ref _ckl, value);
        }

        public Pair? Pair
        {
            get => _pair;
            set => SetField(ref _pair, value);  
        }

        public string FirstValue
        {
            get => _firstValue;
            set => SetField(ref _firstValue, value);
        }

        public string SecondValue
        {
            get => _secondValue;
            set => SetField(ref _secondValue, value);
        }

        public string ThirdValue
        {
            get => _thirdValue;
            set => SetField(ref _thirdValue, value);
        }

        public ICommand NavigateToRelationInputCommand => new RelayCommand(NavigateToRelationInput);
        public ICommand GoBackCommand => new RelayCommand(GoBack);
        public ICommand AddCommand => new RelayCommand(AddPair);
        public ICommand DeleteCommand => new RelayCommand(DeletePair);

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

        private void AddPair()
        {
            if (string.IsNullOrWhiteSpace(FirstValue)) return;

            var pair = new Pair(FirstValue.Trim())
            {
                SecondValue = SecondValue?.Trim(),
                ThirdValue = ThirdValue?.Trim()
            };

            CKL.Source.Add(pair);

            OnPropertyChanged(nameof(CKL));

            FirstValue = string.Empty;
            SecondValue = string.Empty;
            ThirdValue = string.Empty;
        }

        private void DeletePair()
        {
            if (Pair != null)
            { 
                CKL.Source.Remove(Pair);
                Pair = null;
                OnPropertyChanged(nameof(CKL));
            }
        }
    }
}
