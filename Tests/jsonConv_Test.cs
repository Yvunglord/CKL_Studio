using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using CKL_Studio.Infrastructure.Static;
using CKLLib;

namespace CKL_Studio.Tests.Infrastructure.Static
{
    [TestFixture]
    public class JsonToCklConverterTests
    {
        private string _tempFilePath = null!;

        [SetUp]
        public void Setup()
        {
            _tempFilePath = Path.GetTempFileName();
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists(_tempFilePath))
                File.Delete(_tempFilePath);
        }

        [Test]
        public void ConvertFromJson_ValidSingleEntry_CreatesCklCorrectly()
        {
            var json = @"[
                {
                    ""begin"": 10,
                    ""end"": 20,
                    ""A"": ""1""
                }
            ]";
            File.WriteAllText(_tempFilePath, json);

            var ckl = JsonToCklConverter.ConvertFromJson(_tempFilePath);

            Assert.That(ckl, Is.Not.Null);
            Assert.That(ckl.GlobalInterval.StartTime, Is.EqualTo(10));
            Assert.That(ckl.GlobalInterval.EndTime, Is.EqualTo(20));
            Assert.That(ckl.Source.Count, Is.EqualTo(1));
            Assert.That(ckl.Relation.Count, Is.EqualTo(1));

            var expectedPair = new Pair("A1");
            Assert.That(ckl.Source.Contains(expectedPair));

            var relationItem = ckl.Relation.FirstOrDefault(r => r.Value.Equals(expectedPair));
            Assert.That(relationItem, Is.Not.Null);

            Assert.That(relationItem!.Intervals.Count, Is.EqualTo(1));
            Assert.That(relationItem.Intervals[0].StartTime, Is.EqualTo(10));
            Assert.That(relationItem.Intervals[0].EndTime, Is.EqualTo(20));
        }

        [Test]
        public void ConvertFromJson_MultipleEntries_MergesIntervalsCorrectly()
        {
            var json = @"[
                { ""begin"": 10, ""end"": 15, ""A"": ""1"" },
                { ""begin"": 14, ""end"": 20, ""A"": ""1"" },
                { ""begin"": 25, ""end"": 30, ""A"": ""1"" }
            ]";
            File.WriteAllText(_tempFilePath, json);

            var ckl = JsonToCklConverter.ConvertFromJson(_tempFilePath);

            var expectedPair = new Pair("A1");
            Assert.That(ckl.Source.Contains(expectedPair));

            var relationItem = ckl.Relation.FirstOrDefault(r => r.Value.Equals(expectedPair));
            Assert.That(relationItem, Is.Not.Null);

            Assert.That(relationItem!.Intervals.Count, Is.EqualTo(2));
            Assert.That(relationItem.Intervals[0].StartTime, Is.EqualTo(10));
            Assert.That(relationItem.Intervals[0].EndTime, Is.EqualTo(20));
            Assert.That(relationItem.Intervals[1].StartTime, Is.EqualTo(25));
            Assert.That(relationItem.Intervals[1].EndTime, Is.EqualTo(30));
        }

        [Test]
        public void ConvertFromJson_MissingBeginOrEnd_ThrowsInvalidDataException()
        {
            var json = @"[
                { ""begin"": 10, ""A"": ""1"" }
            ]";
            File.WriteAllText(_tempFilePath, json);

            Assert.Throws<InvalidDataException>(() => JsonToCklConverter.ConvertFromJson(_tempFilePath));
        }

        [Test]
        public void ConvertFromJson_PairWithInvalidNumberOfFields_ThrowsInvalidDataException()
        {
            var json = @"[
                { ""begin"": 10, ""end"": 20, ""A"": ""1"", ""B"": ""2"", ""C"": ""3"", ""D"": ""4"" }
            ]";
            File.WriteAllText(_tempFilePath, json);

            Assert.Throws<InvalidDataException>(() => JsonToCklConverter.ConvertFromJson(_tempFilePath));
        }

        [Test]
        public void ConvertFromJson_EmptyJson_ThrowsInvalidDataException()
        {
            var json = "[]";
            File.WriteAllText(_tempFilePath, json);

            var ex = Assert.Throws<InvalidDataException>(() => JsonToCklConverter.ConvertFromJson(_tempFilePath));
            Assert.That(ex.Message, Does.Contain("Invalid JSON"));
        }
    }
}
