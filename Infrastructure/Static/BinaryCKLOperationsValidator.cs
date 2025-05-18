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
        CKLMath
        public static bool CanPerformOperations(CKL ckl1, CKL ckl2)
        {
            if (ckl1 != null && ckl2 != null)
            {
                return false;
            }

            bool[] checks = new bool[]
                {

                };

            return checks.Any(c => c);
        }
        private static bool CheckBinaryConditions(CKL ckl1, CKL ckl2)
        {
            try
            {
                if (!ckl1.GlobalInterval.Equals(ckl2.GlobalInterval)) return false;
                if (!ckl1.Dimention.Equals(ckl2.Dimention)) return false;
                if (!ckl1.Source.SequenceEqual(ckl2.Source)) return false;

                return ckl1.Relation.Any(item1 =>
                    ckl2.Relation.Any(item2 => item1.Value.Equals(item2.Value)));
            }
            catch { return false; }
        }

        private static bool CheckAlgebraConditions(CKL ckl1, CKL ckl2)
        {
            try
            {
                if (!ckl1.GlobalInterval.Equals(ckl2.GlobalInterval)) return false;
                if (!ckl1.Dimention.Equals(ckl2.Dimention)) return false;

                bool sourceMatch = ckl1.Source.All(p1 =>
                    ckl2.Source.Any(p2 => p2.FirstValue.ToString().Equals(p1.SecondValue.ToString())));

                if (!sourceMatch) return false;

                return ckl1.Relation.Any(item1 =>
                    ckl2.Relation.Any(item2 =>
                        item2.Value.FirstValue.ToString().Equals(item1.Value.SecondValue.ToString())));
            }
            catch { return false; }
        }

        private static bool CheckSemanticConditions(CKL ckl1, CKL ckl2)
        {
            try
            {
                if (!ckl1.GlobalInterval.Equals(ckl2.GlobalInterval)) return false;
                if (!ckl1.Dimention.Equals(ckl2.Dimention)) return false;

                bool ckl1Valid = ckl1.Source.All(p => p.SecondValue == null);
                bool ckl2Valid = ckl2.Source.All(p => p.SecondValue == null);

                return ckl1Valid && ckl2Valid;
            }
            catch { return false; }
        }
    }
}
