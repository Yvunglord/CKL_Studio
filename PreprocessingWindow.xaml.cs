using CKL_Studio.Services;
using CKL_Studio.ViewModels;
using System.Windows;

namespace CKL_Studio
{
    /// <summary>
    /// Логика взаимодействия для PreprocessingWindow.xaml
    /// </summary>
    public partial class PreprocessingWindow : Window
    {
        NavigationService _navigationService;
        CKLService _cklService;
        public PreprocessingWindow()
        {
            InitializeComponent();

            this.DataContext = new PreprocessingVM(_navigationService, _cklService);
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void MaximizeButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.WindowState == WindowState.Normal)
            {
                this.WindowState = WindowState.Maximized;
            }
            else
            {
                this.WindowState = WindowState.Normal;
            }
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Minimized;
        }
    }
}
