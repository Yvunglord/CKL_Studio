using CKLDrawing;
using CKLLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.MVVM.ViewModel
{
    public class MakeRelationViewModel : INotifyPropertyChanged
    {

        private readonly CKLService _cklService;

        public MakeRelationViewModel(CKLService cklService)
        { 
            _cklService = cklService;
        }

        public HashSet<RelationItem> Relation
        { 
            get => _cklService.CKL.Relation;
            set
            {
                //_cklService.CKL.Relation = value;
                OnPropertyChanged(nameof(Relation));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
