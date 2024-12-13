using CKL_Studio.MVVM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CKL_Studio.MVVM;

namespace CKL_Studio
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var navigationService = new NavigationService(viewModelType =>
            {
                return Activator.CreateInstance(viewModelType);
            }, viewModel =>
            {
                var preprocessingWindow = (PreprocessingWindow)Current.Windows
                    .OfType<Window>()
                    .FirstOrDefault(w => w is PreprocessingWindow);
                var preprocessingViewModel = (PreprocessingViewModel)preprocessingWindow.DataContext;
                preprocessingViewModel.CurrentViewModel = viewModel;
            });

            var preprocessingViewModel = new PreprocessingViewModel(navigationService);
            var preprocessingWindow = new PreprocessingWindow
            {
                DataContext = preprocessingViewModel
            };

            preprocessingWindow.Show();
        }
    }
}
