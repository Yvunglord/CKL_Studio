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
using System.Windows.Shapes;

namespace CKL_Studio.Presentation.Windows.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для TimeOperationDialog.xaml
    /// </summary>
    public partial class TimeOperationDialog : Window
    {
        public string TextBox1Value { get; private set; } = string.Empty;
        public string TextBox2Value { get; private set; } = string.Empty;
        public TimeOperationDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            TextBox1Value = TextBox1.Text;
            TextBox2Value = TextBox2.Text;
            DialogResult = true;
            Close();
        }

        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.OriginalSource is Grid)
            {
                this.DragMove();
            }
        }
    }
}
