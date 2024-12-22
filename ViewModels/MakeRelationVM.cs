using CKL_Studio.Services;
using CKLLib;
using System.ComponentModel;
using System.Windows.Input;

namespace CKL_Studio.ViewModels
{
    public class MakeRelationVM : INotifyPropertyChanged
    {
        private readonly CKLService _cklService;

        public MakeRelationVM(CKLService cklService)
        {
            _cklService = cklService;
        }

        private HashSet<RelationItem> _relation;
        public HashSet<RelationItem> Relation
        {
            get => _relation;
            set
            {
                _relation = value;
                OnPropertyChanged(nameof(Relation));
            }
        }

        public ICommand SaveRelationCommand => new RelayCommand(() => SaveRelation());

        private void SaveRelation()
        {
            _cklService.UpdateRelation(Relation);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
