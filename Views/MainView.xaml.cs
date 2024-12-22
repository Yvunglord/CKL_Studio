using CKL_Studio.Services;
using CKL_Studio.ViewModels;
using System.Windows.Controls;
using NavigationService = CKL_Studio.Services.NavigationService;

namespace CKL_Studio
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : UserControl
    {
        NavigationService _navigationService;
        CKLService _cklService;
        public MainView()
        {
            InitializeComponent();

            this.DataContext = new MainVM(_navigationService, _cklService);
        }
    }
}
