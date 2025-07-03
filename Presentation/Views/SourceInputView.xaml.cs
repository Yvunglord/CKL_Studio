using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace CKL_Studio.Presentation.Views
{
    /// <summary>
    /// Логика взаимодействия для SourceInputView.xaml
    /// </summary>
    public partial class SourceInputView : UserControl
    {
        public SourceInputView()
        {
            InitializeComponent();
            this.DataContextChanged += SourceInputView_DataContextChanged;
        }

        private void SourceInputView_DataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is ViewModels.SourceInputViewModel vm)
            {
                vm.PropertyChanged += (s, args) =>
                {
                    if (args.PropertyName == nameof(vm.Dim))
                    {
                        UpdateDataGridColumns();
                    }
                };
                UpdateDataGridColumns();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateDataGridColumns();
        }

        private void DataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            if (!e.Column.IsReadOnly)
            {
                e.Cancel = false;
            }
        }


        private void UpdateDataGridColumns()
        {
            SourceDataGrid.Columns.Clear();

            if (DataContext is ViewModels.SourceInputViewModel vm)
            {
                var col1 = new DataGridTextColumn
                {
                    Header = "Множество 1",
                    Binding = new Binding("Values[0]") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
                    Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                    HeaderStyle = (Style)FindResource("CustomDataGridColumnHeaderStyle"),
                    EditingElementStyle = (Style)FindResource("CustomDataGridCellEditingStyle")
                };
                SourceDataGrid.Columns.Add(col1);

                if (vm.Dim >= 2)
                {
                    var col2 = new DataGridTextColumn
                    {
                        Header = "Множество 2",
                        Binding = new Binding("Values[1]") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
                        Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                        HeaderStyle = (Style)FindResource("CustomDataGridColumnHeaderStyle"),
                        EditingElementStyle = (Style)FindResource("CustomDataGridCellEditingStyle")
                    };
                    SourceDataGrid.Columns.Add(col2);
                }

                if (vm.Dim >= 3)
                {
                    var col3 = new DataGridTextColumn
                    {
                        Header = "Множество 3",
                        Binding = new Binding("Values[2]") { UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged },
                        Width = new DataGridLength(1, DataGridLengthUnitType.Star),
                        HeaderStyle = (Style)FindResource("CustomDataGridColumnHeaderStyle"),
                        EditingElementStyle = (Style)FindResource("CustomDataGridCellEditingStyle")
                    };
                    SourceDataGrid.Columns.Add(col3);
                }
            }
        }
    
        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9.-]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                var textBox = sender as TextBox;
                var binding = textBox.GetBindingExpression(TextBox.TextProperty);
                binding.UpdateSource();

                Keyboard.ClearFocus();
                e.Handled = true;
            }
        }

        private void Grid_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            var focusedElement = Keyboard.FocusedElement as TextBox;

            if (focusedElement != null && (focusedElement.Name == "RowCountTextBox" || focusedElement.Name == "SourceDataGrid"))
            {
                var binding = focusedElement.GetBindingExpression(TextBox.TextProperty);
                binding?.UpdateSource();
                Keyboard.ClearFocus();  
            }
        }

        private void SourceDataGrid_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            var dataGrid = sender as DataGrid;

            if (dataGrid == null)
                return;

            var currentCell = dataGrid.CurrentCell;
            var currentColumn = currentCell.Column as DataGridBoundColumn;

            if (currentColumn == null)
                return;

            if (e.Key == Key.Left || e.Key == Key.Right || e.Key == Key.Up || e.Key == Key.Down)
            {
                e.Handled = true;

                int rowIndex = dataGrid.Items.IndexOf(currentCell.Item);
                int columnIndex = dataGrid.Columns.IndexOf(currentColumn);

                switch (e.Key)
                {
                    case Key.Left:
                        columnIndex--;
                        break;
                    case Key.Right:
                        columnIndex++;
                        break;
                    case Key.Up:
                        rowIndex--;
                        break;
                    case Key.Down:
                        rowIndex++;
                        break;
                }

                if (rowIndex >= 0 && rowIndex < dataGrid.Items.Count &&
                    columnIndex >= 0 && columnIndex < dataGrid.Columns.Count)
                {
                    var newCell = new DataGridCellInfo(
                        dataGrid.Items[rowIndex],
                        dataGrid.Columns[columnIndex]);

                    dataGrid.CurrentCell = newCell;
                    dataGrid.ScrollIntoView(dataGrid.Items[rowIndex], dataGrid.Columns[columnIndex]);

                    dataGrid.BeginEdit();
                }
            }
        }

        private void SourceDataGrid_Loaded(object sender, RoutedEventArgs e)
        {
            var dataGrid = sender as DataGrid;
            if (dataGrid != null)
            {
                dataGrid.PreviewKeyDown += SourceDataGrid_PreviewKeyDown;
            }
        }
    }
}
