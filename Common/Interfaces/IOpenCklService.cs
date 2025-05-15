using CKLDrawing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace CKL_Studio.Common.Interfaces
{
    public interface IOpenCklService
    {
        CKLView? Load(string path, Dispatcher dispatcher);
        bool Validate(string path);
    }
}
