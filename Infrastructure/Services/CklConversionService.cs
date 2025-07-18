﻿using CKL_Studio.Common.Interfaces;
using CKL_Studio.Common.Interfaces.CKLInterfaces;
using CKL_Studio.Infrastructure.Static;
using CKLLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Infrastructure.Services
{
    public class CklConversionService : IJsonToCklСonversion
    { 
        public CKL Convert(string path)
        {
            if (path != null)
            {
                try
                {
                    return JsonToCklConverter.ConvertFromJson(path);            
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException(ex.Message, nameof(path));
                }
            }

            return new CKL();
        }
    }
}
