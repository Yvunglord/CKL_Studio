using CKL_Studio.Common.Interfaces.CKLInterfaces;
using CKLDrawing;
using CKLLib;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Infrastructure.Services
{
    public class CKLViewManager : ICklViewManager
    {
        private readonly ObservableCollection<CKLView> _openedCklViews = new ObservableCollection<CKLView>();
        private CKLView? _selectedCklView;
        public ObservableCollection<CKLView> OpenedCklViews => _openedCklViews;

        public CKLView? SelectedCklView 
        { 
            get => _selectedCklView;
            set => _selectedCklView = value;
        }

        public void Close(CKLView view)
        {
            if (view != null)
            {
                OpenedCklViews.Remove(view);

                if (SelectedCklView == view)
                    SelectedCklView = OpenedCklViews.LastOrDefault();
            }
        }

        public void Open(CKL ckl)
        {
            var alreadyOpened = OpenedCklViews.FirstOrDefault(v => v.Ckl.FilePath == ckl.FilePath);
            if (alreadyOpened != null)
            {
                SelectedCklView = alreadyOpened;
                return;
            }

            var newView = new CKLView(ckl);
            OpenedCklViews.Add(newView);
            SelectedCklView = newView;
        }
    }
}
