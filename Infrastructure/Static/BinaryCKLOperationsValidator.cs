using CKLLib;
using CKLLib.Operations;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CKL_Studio.Infrastructure.Static
{
    public static class BinaryCKLOperationsValidator
    {
        public static bool CanPerformOperation(CKL ckl1, CKL ckl2)
        {
            bool[] checks = new bool[]
            {
                CheckBinaryConditions(ckl1, ckl2),
                CheckAlgebraConditions(ckl1, ckl2),
                CheckSemanticConditions(ckl1, ckl2)
            };

            return checks.Any(check => check);
        }

        private static bool CheckBinaryConditions(CKL ckl1, CKL ckl2)
        {
            try
            {
                if (ckl1 == null) return false;
                if (ckl2 == null) return false;

                if (!ckl1.GlobalInterval.Equals(ckl2.GlobalInterval)) return false;
                if (!ckl1.Dimention.Equals(ckl2.Dimention)) return false;
                if (!ckl1.Source.SequenceEqual(ckl2.Source)) return false;     

                return true;
            }
            catch { return false; }
        }

        private static bool CheckAlgebraConditions(CKL ckl1, CKL ckl2)
        {
            try
            {
                if (ckl1 == null) return false;
                if (ckl2 == null) return false;

                if (!ckl1.GlobalInterval.Equals(ckl2.GlobalInterval)) return false;
                if (!ckl1.Dimention.Equals(ckl2.Dimention)) return false;

                foreach (Pair p1 in ckl1.Source)
                {
                    if (!ckl2.Source.Any(el => el.FirstValue.ToString().Equals(p1.SecondValue.ToString()))) return false;
                }

                return true;
            }
            catch { return false; }
        }

        private static bool CheckSemanticConditions(CKL ckl1, CKL ckl2)
        {
            try
            {
                if (ckl1 == null) return false;
                if (ckl2 == null) return false;

                if (!ckl1.GlobalInterval.Equals(ckl2.GlobalInterval)) return false;
                if (!ckl1.Dimention.Equals(ckl2.Dimention)) return false;

                foreach (Pair p in ckl1.Source)
                {
                    if (p.SecondValue != null) return false;
                }
                foreach (Pair p in ckl2.Source)
                {
                    if (p.SecondValue != null) return false;
                }

                return true;
            }
            catch { return false; }
        }
    }
}
