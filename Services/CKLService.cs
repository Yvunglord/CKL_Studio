using CKLLib;

namespace CKL_Studio.Services
{
    public class CKLService
    {
        public CKL CKLInstance { get; set; }

        public CKLService()
        {
            CKLInstance = new CKL();
        }

        public void UpdateCKL(CKL ckl)
        {
            CKLInstance = ckl;
        }

        public void UpdateName(string name)
        {
            CKLInstance.Name = name;
        }

        public void UpdateGlobalInterval(TimeInterval interval)
        {
            // CKLInstance.GlobalInterval = interval;
        }

        public void UpdateSource(HashSet<object> source)
        {
            // CKLInstance.Source = source;
        }

        public void UpdateRelation(HashSet<RelationItem> relation)
        {
            //  CKLInstance.Relation = relation;
        }
    }
}
