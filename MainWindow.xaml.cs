using CKL_Studio.Services;
using CKL_Studio.ViewModels;
using System.Windows;
using NavigationService = CKL_Studio.Services.NavigationService;

namespace CKL_Studio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        NavigationService _navigationService;
        CKLService _cklService;
        public MainWindow()
        {
            InitializeComponent();

            this.DataContext = new MainVM(_navigationService, _cklService);
        }
    }
}