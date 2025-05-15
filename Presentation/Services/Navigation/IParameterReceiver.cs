using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Presentation.Services.Navigation
{
    public interface IParameterReceiver<in T> where T : class
    {
        void ReceiveParameter(T parameter);
    }
}
