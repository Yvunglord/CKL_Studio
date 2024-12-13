using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CKL_Studio.MVVM.ViewModel;
using CKL_Studio.OnLoad;
using CKL_Studio.EnterDynamicData;

namespace CKL_Studio.EnterStaticData
{
    /// <summary>
    /// Логика взаимодействия для EnterStaticDataView.xaml
    /// </summary>
    public partial class EnterStaticDataView : UserControl
    {
        public EnterStaticDataView()
        {
            InitializeComponent();

            this.DataContext = new EnterStaticDataViewModel();
        }

        /*private void NavigateToOnLoad()
        {
            var viewModel = new OnLoadViewModel();

            var preprocessingWindow = Window.GetWindow(this) as PreprocessingWindow;

            if (preprocessingWindow != null)
            {
                preprocessingWindow.PreprocessingContentControl.Content = new OnLoadView();
            }
        }*/

       /* private void NavigateToDynamicData()
        {
            var viewModel = new OnLoadViewModel();

            var preprocessingWindow = Window.GetWindow(this) as PreprocessingWindow;

            if (preprocessingWindow != null)
            {
                preprocessingWindow.PreprocessingContentControl.Content = new EnterDynamicDataView();
            }
        }*/

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
          //  NavigateToOnLoad();
        }

        private void btnForward_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
