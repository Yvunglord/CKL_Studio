using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.MVVM
{
    public class NavigationService : INavigationService
    {
        private readonly Func<Type, object> _viewModelFactory;
        private readonly Action<object> _navigateAction;

        public NavigationService(Func<Type, object> viewModelFactory, Action<object> navigateAction)
        { 
            _viewModelFactory = viewModelFactory;
            _navigateAction = navigateAction;
        }

        public void NavigateTo<TViewModel>() where TViewModel : class
        {
            var viewModel = _viewModelFactory(typeof(TViewModel));
            _navigateAction(viewModel);
        }
    }
}
