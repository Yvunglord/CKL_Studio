using CKL_Studio.Common.Interfaces;
using CKLLib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using CKL_Studio.Presentation.ViewModels.Base;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace CKL_Studio.Infrastructure.Static
{
    public static class JSONToCKLConverter
    {
        public static CKL ConvertFromJson(string jsonFilePath, TimeDimentions dimention = TimeDimentions.SECONDS)
        {
            string json = File.ReadAllText(jsonFilePath);
            var rawData = JsonSerializer.Deserialize<List<JsonNode>>(json) ?? throw new InvalidDataException("Invalid JSON");

            var pairIntervals = new Dictionary<Pair, List<TimeInterval>>();
            var source = new HashSet<Pair>();
            double minBegin = double.MaxValue;
            double maxEnd = double.MinValue;

            foreach (var node in rawData)
            {
                var obj = node.AsObject();
                if (!obj.ContainsKey("begin") || !obj.ContainsKey("end"))
                    throw new InvalidDataException("Object missing 'begin' or 'end' field.");

                long begin = obj["begin"]!.GetValue<long>();
                long end = obj["end"]!.GetValue<long>();

                minBegin = Math.Min(minBegin, begin);
                maxEnd = Math.Max(maxEnd, end);

                var pairFields = obj.Where(kvp => kvp.Key != "begin" && kvp.Key != "end")
                     .OrderBy(kvp => kvp.Key)
                     .Select(kvp => $"{kvp.Key}{kvp.Value!.ToString()}") 
                     .ToList();

                Pair pair = new Pair(pairFields);

                source.Add(pair);

                TimeInterval interval = new(begin, end);
                if (pairIntervals.TryGetValue(pair, out var intervals))
                    intervals.Add(interval);
                else
                    pairIntervals[pair] = new List<TimeInterval> { interval };
            }

            var relation = new HashSet<RelationItem>();
            foreach (var kvp in pairIntervals)
            {
                var merged = MergeIntervals(kvp.Value);
                relation.Add(new RelationItem(kvp.Key, merged));
            }

            string cklFilePath = Path.ChangeExtension(jsonFilePath, ".ckl");

            return new CKL
            {
                FilePath = cklFilePath,
                GlobalInterval = new TimeInterval(minBegin, maxEnd),
                Dimention = dimention,
                Source = source,
                Relation = relation
            };
        }

        private static List<TimeInterval> MergeIntervals(List<TimeInterval> intervals)
        {
            if (intervals.Count == 0) return new();

            var sorted = intervals.OrderBy(i => i.StartTime).ToList();
            List<TimeInterval> merged = new() { sorted[0] };

            for (int i = 1; i < sorted.Count; i++)
            {
                var last = merged[^1];
                var current = sorted[i];

                if (current.StartTime <= last.EndTime)
                    merged[^1] = new TimeInterval(last.StartTime, Math.Max(last.EndTime, current.EndTime));
                else
                    merged.Add(current);
            }

            return merged;
        }
    }
}
