using CKL_Studio.Common.Interfaces;
using CKLDrawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CKLLib;
using System.Windows.Threading;
using System.IO;

namespace CKL_Studio.Infrastructure.Services
{
    public class OpenCklService : IOpenCklService
    {
        public CKLView? Load(string path, Dispatcher dispatcher)
        {
            if (Validate(path))
            { 
#pragma warning disable CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
                CKL ckl = CKL.GetFromFile(path);
                ckl.FilePath = path;
#pragma warning restore CS8600 // Преобразование литерала, допускающего значение NULL или возможного значения NULL в тип, не допускающий значение NULL.
#pragma warning disable CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
                return dispatcher.Invoke(() => new CKLView(ckl));
#pragma warning restore CS8604 // Возможно, аргумент-ссылка, допускающий значение NULL.
            }

            return null;
        }

        public bool Validate(string path)
        {
            if (File.Exists(path) && CKL.GetFromFile(path) != null)
            { 
                return true;
            }
            
            return false;
        }
    }
}
