using CKLLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Common.Interfaces.CKLInterfaces
{
    interface ICklComparsionService
    {
        bool Compare(CKL ckl1, CKL ckl2);
    }
}
