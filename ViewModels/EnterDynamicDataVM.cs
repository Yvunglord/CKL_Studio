using CKL_Studio.Models;
using CKL_Studio.Services;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Input;

namespace CKL_Studio.ViewModels
{
    public class EnterDynamicDataVM : INotifyPropertyChanged
    {
        private readonly CKLService _cklService;

        public EnterDynamicDataVM(CKLService cklService)
        {
            _cklService = cklService;
            Items = new ObservableCollection<ListBoxItemModel>
        {
            new ListBoxItemModel { Text = string.Empty },
            new ListBoxItemModel { Text = string.Empty },
            new ListBoxItemModel { Text = string.Empty },
            new ListBoxItemModel { Text = string.Empty },
            new ListBoxItemModel { Text = string.Empty },
            new ListBoxItemModel { Text = string.Empty },
            new ListBoxItemModel { Text = string.Empty },
            new ListBoxItemModel { Text = string.Empty },
            new ListBoxItemModel { Text = string.Empty },
            new ListBoxItemModel { Text = string.Empty }
        };
            AddItemCommand = new RelayCommand(AddItem);
        }

        private ObservableCollection<ListBoxItemModel> _items;
        public ObservableCollection<ListBoxItemModel> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged(nameof(Items));
            }
        }

        public ICommand AddItemCommand { get; }

        private void AddItem()
        {
            Items.Add(new ListBoxItemModel { Text = string.Empty });
        }

        public ICommand SaveDynamicDataCommand => new RelayCommand(() => SaveDynamicData());

        private void SaveDynamicData()
        {
            var source = new HashSet<object>(Items.Select(item => item.Text));
            _cklService.UpdateSource(source);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }

}
