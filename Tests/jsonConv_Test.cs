using CKL_Studio.Infrastructure.Static;
using CKLLib;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

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

    public void ConvertFromJson_MultipleDifferentPairs_CreatesMultipleRelations()
        {
            var json = @"[
                { ""begin"": 5, ""end"": 10, ""A"": ""1"" },
                { ""begin"": 12, ""end"": 15, ""B"": ""2"" }
            ]";
            File.WriteAllText(_tempFilePath, json);

            var ckl = JsonToCklConverter.ConvertFromJson(_tempFilePath);

            Assert.That(ckl.Source.Count, Is.EqualTo(2));
            Assert.That(ckl.Relation.Count, Is.EqualTo(2));

            var pairA = new Pair("A1");
            var pairB = new Pair("B2");

            Assert.That(ckl.Source.Contains(pairA));
            Assert.That(ckl.Source.Contains(pairB));

            var relationA = ckl.Relation.FirstOrDefault(r => r.Value.Equals(pairA));
            var relationB = ckl.Relation.FirstOrDefault(r => r.Value.Equals(pairB));

            Assert.That(relationA, Is.Not.Null);
            Assert.That(relationB, Is.Not.Null);

            Assert.That(relationA!.Intervals[0].StartTime, Is.EqualTo(5));
            Assert.That(relationA!.Intervals[0].EndTime, Is.EqualTo(10));
            Assert.That(relationB!.Intervals[0].StartTime, Is.EqualTo(12));
            Assert.That(relationB!.Intervals[0].EndTime, Is.EqualTo(15));
        }

        // Тест: Проверка обработки JSON с интервалами, которые не пересекаются
        [Test]
        public void ConvertFromJson_NonOverlappingIntervals_CreatesSeparateIntervals()
        {
            var json = @"[
                { ""begin"": 0, ""end"": 5, ""A"": ""1"" },
                { ""begin"": 10, ""end"": 15, ""A"": ""1"" }
            ]";
            File.WriteAllText(_tempFilePath, json);

            var ckl = JsonToCklConverter.ConvertFromJson(_tempFilePath);

            var relationItem = ckl.Relation.FirstOrDefault(r => r.Value.Equals(new Pair("A1")));

            Assert.That(relationItem, Is.Not.Null);
            Assert.That(relationItem!.Intervals.Count, Is.EqualTo(2));
            Assert.Equals(0, relationItem.Intervals[0].StartTime);
            Assert.Equals(5, relationItem.Intervals[0].EndTime);
            Assert.Equals(10, relationItem.Intervals[1].StartTime);
            Assert.Equals(15, relationItem.Intervals[1].EndTime);
        }

        // Тест: Обработка некорректных данных с пропущенными ключами (например отсутствует "begin" или "end")
        [Test]
        public void ConvertFromJson_MissingEnd_ThrowsInvalidDataException()
        {
            var json = @"[
                { ""begin"": 10 }
            ]";
            File.WriteAllText(_tempFilePath, json);

            var ex = Assert.Throws<InvalidDataException>(() => JsonToCklConverter.ConvertFromJson(_tempFilePath));
            StringAssert.Contains("Invalid JSON", ex.Message);
        }

        // Тест: Обработка данных с некорректным форматом (например неправильный тип данных)
        [Test]
        public void ConvertFromJson_InvalidType_ThrowsInvalidDataException()
        {
            var json = @"[
                { ""begin"": ""not a number"", ""end"": 20, ""A"": ""1"" }
            ]";
            File.WriteAllText(_tempFilePath, json);

            Assert.Throws<InvalidDataException>(() => JsonToCklConverter.ConvertFromJson(_tempFilePath));
        }
    }
}