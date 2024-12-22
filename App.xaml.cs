using CKL_Studio.Services;
using CKL_Studio.ViewModels;
using System.Windows;

namespace CKL_Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var navigationService = new NavigationService();
            var cklService = new CKLService();

            var preprocessingViewModel = new PreprocessingVM(navigationService, cklService);
            var mainViewModel = new MainVM(navigationService, cklService);

            var preprocessingWindow = new PreprocessingWindow
            {
                DataContext = preprocessingViewModel
            };

            var mainWindow = new MainWindow
            {
                DataContext = mainViewModel
            };
        }
    }
}
