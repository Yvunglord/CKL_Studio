using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels.Base;
using CKLDrawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKLLib;
using System.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CKL_Studio.Presentation.ViewModels
{
    public class RelationInputViewModel : ViewModelBase, IParameterReceiver<CKL>
    {
        private readonly INavigationService _navigationService;
        private CKL _ckl;
        private CKLView? _cklView;

        public CKL CKL
        {
            get => _ckl;
            set => SetField(ref _ckl, value);
        }

        public CKLView? CKLView
        { 
            get => _cklView;
            set => SetField(ref _cklView, value);
        }

        public RelationInputViewModel(IServiceProvider serviceProvider, CKL ckl) : base(serviceProvider)
        {
            _navigationService = serviceProvider.GetRequiredService<INavigationService>();

            _ckl = ckl;
        }

        public void ReceiveParameter(CKL parameter)
        {
            _ckl = parameter;
        }
    }
}
