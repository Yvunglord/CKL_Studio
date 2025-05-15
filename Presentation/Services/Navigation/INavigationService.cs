using CKL_Studio.Presentation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Presentation.Services.Navigation
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>() where TViewModel : ViewModelBase;
        void NavigateTo<TViewModel, TParameter>(TParameter parameter)
            where TViewModel : ViewModelBase, IParameterReceiver<TParameter>
            where TParameter : class;
        void GoBack();
        ViewModelBase? CurrentViewModel { get; }
        event EventHandler CurrentViewChanged;
    }
}
