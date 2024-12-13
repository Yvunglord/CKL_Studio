using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.MVVM
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>() where TViewModel : class;
    }
}
