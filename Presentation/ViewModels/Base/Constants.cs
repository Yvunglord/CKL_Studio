using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CKL_Studio.Presentation.ViewModels.Base
{
    public static class Constants
    {
        public static readonly string CKL_FILE_DIALOG_FILTER = "CKL Files (*.ckl)|*.ckl";
        public static readonly string JSON_FILE_DIALOG_FILTER = "JSON Files (*.json)|*.json";
        public static readonly string DEFAULT_FILE_PATH = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
        public static readonly string DEFAULT_CKL_FILE_PATH = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "CKL_Files");
    }
}
