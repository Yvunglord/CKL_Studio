using CKL_Studio.Common.Interfaces;
using CKLLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CKL_Studio.Infrastructure.Static
{
    public static class JSONToCKLConverter
    {
        public static CKL ConvertJSONToCKL(string path)
        {
            string jsonContent = File.ReadAllText(path);
            JArray jsonArray = JArray.Parse(jsonContent);
            HashSet<Pair> source = new HashSet<Pair>();
            Dictionary<Pair, List<TimeInterval>> relationDict = new Dictionary<Pair, List<TimeInterval>>();
            double minBegin = double.MaxValue;
            double maxEnd = double.MinValue;

            foreach (JObject item in jsonArray)
            {
                double begin = (double)item["begin"];
                double end = (double)item["end"];
                minBegin = Math.Min(minBegin, begin);
                maxEnd = Math.Max(maxEnd, end);

                var fields = item.Properties()
                    .Where(p => p.Name != "begin" && p.Name != "end")
                    .OrderBy(p => p.Name)
                    .Select(p => $"{p.Name}_{p.Value}")
                    .ToList();

                Pair pair = fields.Count switch
                {
                    1 => new Pair(fields[0]),
                    2 => new Pair(fields[0], fields[1]),
                    3 => new Pair(fields[0], fields[1], fields[2]),
                    _ => throw new NotSupportedException("Unsupported number of fields.")
                };

                if (!source.Contains(pair))
                    source.Add(pair);

                if (!relationDict.ContainsKey(pair))
                    relationDict[pair] = new List<TimeInterval>();
                relationDict[pair].Add(new TimeInterval(begin, end));
            }

            HashSet<RelationItem> relations = new HashSet<RelationItem>();
            foreach (var kvp in relationDict)
                relations.Add(new RelationItem(kvp.Key, kvp.Value));

            TimeInterval globalInterval = new TimeInterval(minBegin, maxEnd);

            return new CKL(path, globalInterval, TimeDimentions.NANOSECONDS, source, relations);
        }
    }
}
