using CKLDrawing;
using CKLLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Common.Interfaces.CKLInterfaces
{
    public interface ICklViewManager
    {
        ObservableCollection<CKLView> OpenedCklViews { get; }
        CKLView? SelectedCklView { get; set;}
        void Open(CKL ckl);
        void Close(CKLView view);
    }
}
