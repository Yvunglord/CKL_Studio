using CKL_Studio.Common.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows.Media;

namespace CKL_Studio.CKL_Tests
{
    public class DirtyToColorConverterTestRunner
    {
        public static TestResult RunAllDirtyToColorConverterTests()
        {
            var testRunner = new DirtyToColorConverterTestRunner();
            return testRunner.RunTestsAndPrintReport();
        }

        public TestResult RunTestsAndPrintReport()
        {
            Debug.WriteLine("Запуск тестов DirtyToColorConverter");
            Debug.WriteLine("");

            var testFixture = new DirtyToColorConverterTests();
            var testMethods = GetTestMethods();

            int totalTests = 0;
            int passedTests = 0;
            var failedTests = new List<TestFailure>();

            foreach (var testMethod in testMethods)
            {
                totalTests++;
                try
                {
                    testFixture.Setup();
                    testMethod.Invoke(testFixture, null);
                    passedTests++;
                    Debug.WriteLine($"ПРОЙДЕН: {testMethod.Name}");
                }
                catch (Exception ex)
                {
                    var innerException = ex.InnerException ?? ex;
                    var failure = new TestFailure
                    {
                        TestName = testMethod.Name,
                        Exception = innerException
                    };
                    failedTests.Add(failure);

                    Debug.WriteLine($"ПРОВАЛЕН: {testMethod.Name}");
                    Debug.WriteLine($"Ошибка: {innerException.Message}");
                }
            }

            PrintSummary(totalTests, passedTests, failedTests.Count);

            return new TestResult
            {
                TotalTests = totalTests,
                PassedTests = passedTests,
                FailedTests = failedTests
            };
        }

        private MethodInfo[] GetTestMethods()
        {
            return typeof(DirtyToColorConverterTests)
                .GetMethods(BindingFlags.Public | BindingFlags.Instance)
                .Where(m => m.GetCustomAttributes<TestAttribute>().Any())
                .ToArray();
        }

        private void PrintSummary(int total, int passed, int failed)
        {
            Debug.WriteLine("СВОДКА ТЕСТИРОВАНИЯ");
            Debug.WriteLine("====================");
            Debug.WriteLine($"Всего тестов: {total}");
            Debug.WriteLine($"Пройдено: {passed}");
            Debug.WriteLine($"Провалено: {failed}");
            Debug.WriteLine($"Успешность: {((double)passed / total * 100):F1}%");

            if (failed == 0)
            {
                Debug.WriteLine("ВСЕ ТЕСТЫ ПРОЙДЕНЫ УСПЕШНО");
            }
            else
            {
                Debug.WriteLine($"ЕСТЬ ПРОБЛЕМЫ: {failed} тестов не прошли");
            }
        }
    }

    [TestFixture]
    public class DirtyToColorConverterTests
    {
        private DirtyToColorConverter _converter;
        private CultureInfo _culture;

        [SetUp]
        public void Setup()
        {
            _converter = new DirtyToColorConverter();
            _culture = CultureInfo.InvariantCulture;
            Debug.WriteLine("Setup для теста");
        }

        [TearDown]
        public void Teardown()
        {
            Debug.WriteLine("Teardown завершен");
            Debug.WriteLine("");
        }

        [Test]
        public void Convert_True_ReturnsRed()
        {
            Debug.WriteLine("Тест: Convert_True_ReturnsRed");

            var result = _converter.Convert(true, typeof(Brush), null, _culture);

            Assert.That(result, Is.EqualTo(Brushes.Red));
            Debug.WriteLine("УСПЕХ: true преобразован в Red");
        }

        [Test]
        public void Convert_False_ReturnsGreen()
        {
            Debug.WriteLine("Тест: Convert_False_ReturnsGreen");

            var result = _converter.Convert(false, typeof(Brush), null, _culture);

            Assert.That(result, Is.EqualTo(Brushes.Green));
            Debug.WriteLine("УСПЕХ: false преобразован в Green");
        }

        [Test]
        public void Convert_NonBoolValue_ReturnsGray()
        {
            Debug.WriteLine("Тест: Convert_NonBoolValue_ReturnsGray");

            var result = _converter.Convert("not a bool", typeof(Brush), null, _culture);

            Assert.That(result, Is.EqualTo(Brushes.Gray));
            Debug.WriteLine("УСПЕХ: не-bool значение возвращает Gray");
        }

        [Test]
        public void Convert_NullValue_ReturnsGray()
        {
            Debug.WriteLine("Тест: Convert_NullValue_ReturnsGray");

            var result = _converter.Convert(null, typeof(Brush), null, _culture);

            Assert.That(result, Is.EqualTo(Brushes.Gray));
            Debug.WriteLine("УСПЕХ: null возвращает Gray");
        }

        [Test]
        public void Convert_IntegerValue_ReturnsGray()
        {
            Debug.WriteLine("Тест: Convert_IntegerValue_ReturnsGray");

            var result = _converter.Convert(123, typeof(Brush), null, _culture);

            Assert.That(result, Is.EqualTo(Brushes.Gray));
            Debug.WriteLine("УСПЕХ: integer значение возвращает Gray");
        }

        [Test]
        public void Convert_DecimalValue_ReturnsGray()
        {
            Debug.WriteLine("Тест: Convert_DecimalValue_ReturnsGray");

            var result = _converter.Convert(123.45m, typeof(Brush), null, _culture);

            Assert.That(result, Is.EqualTo(Brushes.Gray));
            Debug.WriteLine("УСПЕХ: decimal значение возвращает Gray");
        }

        [Test]
        public void Convert_StringValue_ReturnsGray()
        {
            Debug.WriteLine("Тест: Convert_StringValue_ReturnsGray");

            var result = _converter.Convert("some string", typeof(Brush), null, _culture);

            Assert.That(result, Is.EqualTo(Brushes.Gray));
            Debug.WriteLine("УСПЕХ: string значение возвращает Gray");
        }

        [Test]
        public void Convert_ObjectValue_ReturnsGray()
        {
            Debug.WriteLine("Тест: Convert_ObjectValue_ReturnsGray");

            var result = _converter.Convert(new object(), typeof(Brush), null, _culture);

            Assert.That(result, Is.EqualTo(Brushes.Gray));
            Debug.WriteLine("УСПЕХ: object значение возвращает Gray");
        }

        [Test]
        public void Convert_TargetTypeIsNotBrush_ReturnsCorrectColor()
        {
            Debug.WriteLine("Тест: Convert_TargetTypeIsNotBrush_ReturnsCorrectColor");

            var result = _converter.Convert(true, typeof(object), null, _culture);

            Assert.That(result, Is.EqualTo(Brushes.Red));
            Debug.WriteLine("УСПЕХ: цвет возвращается корректно даже при другом targetType");
        }

        [Test]
        public void Convert_WithParameter_IgnoresParameter()
        {
            Debug.WriteLine("Тест: Convert_WithParameter_IgnoresParameter");

            var result1 = _converter.Convert(true, typeof(Brush), "some parameter", _culture);
            var result2 = _converter.Convert(false, typeof(Brush), null, _culture);

            Assert.That(result1, Is.EqualTo(Brushes.Red));
            Assert.That(result2, Is.EqualTo(Brushes.Green));
            Debug.WriteLine("УСПЕХ: параметр игнорируется, преобразование работает корректно");
        }

        [Test]
        public void Convert_WithDifferentCulture_ReturnsSameResult()
        {
            Debug.WriteLine("Тест: Convert_WithDifferentCulture_ReturnsSameResult");

            var cultureRu = new CultureInfo("ru-RU");
            var cultureEn = new CultureInfo("en-US");

            var result1 = _converter.Convert(true, typeof(Brush), null, cultureRu);
            var result2 = _converter.Convert(true, typeof(Brush), null, cultureEn);

            Assert.That(result1, Is.EqualTo(Brushes.Red));
            Assert.That(result2, Is.EqualTo(Brushes.Red));
            Debug.WriteLine("УСПЕХ: разные культуры не влияют на результат");
        }

        [Test]
        public void ConvertBack_ThrowsNotImplementedException()
        {
            Debug.WriteLine("Тест: ConvertBack_ThrowsNotImplementedException");

            Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack(Brushes.Red, typeof(bool), null, _culture));

            Debug.WriteLine("УСПЕХ: ConvertBack выбрасывает NotImplementedException");
        }

        [Test]
        public void ConvertBack_WithDifferentParameters_ThrowsNotImplementedException()
        {
            Debug.WriteLine("Тест: ConvertBack_WithDifferentParameters_ThrowsNotImplementedException");

            Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack(Brushes.Green, typeof(bool), "param", _culture));

            Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack(Brushes.Gray, typeof(object), null, _culture));

            Debug.WriteLine("УСПЕХ: ConvertBack всегда выбрасывает NotImplementedException");
        }

        [Test]
        public void Converter_IsStateless()
        {
            Debug.WriteLine("Тест: Converter_IsStateless");

            var result1 = _converter.Convert(true, typeof(Brush), null, _culture);
            var result2 = _converter.Convert(false, typeof(Brush), null, _culture);
            var result3 = _converter.Convert(true, typeof(Brush), null, _culture);

            Assert.That(result1, Is.EqualTo(Brushes.Red));
            Assert.That(result2, Is.EqualTo(Brushes.Green));
            Assert.That(result3, Is.EqualTo(Brushes.Red));
            Debug.WriteLine("УСПЕХ: конвертер работает стабильно при многократных вызовах");
        }

        [Test]
        public void Convert_BrushObjectsAreSingletonInstances()
        {
            Debug.WriteLine("Тест: Convert_BrushObjectsAreSingletonInstances");

            var red1 = _converter.Convert(true, typeof(Brush), null, _culture);
            var red2 = _converter.Convert(true, typeof(Brush), null, _culture);
            var green1 = _converter.Convert(false, typeof(Brush), null, _culture);
            var green2 = _converter.Convert(false, typeof(Brush), null, _culture);

            Assert.That(red1, Is.SameAs(Brushes.Red));
            Assert.That(red2, Is.SameAs(Brushes.Red));
            Assert.That(green1, Is.SameAs(Brushes.Green));
            Assert.That(green2, Is.SameAs(Brushes.Green));
            Debug.WriteLine("УСПЕХ: возвращаются singleton-объекты Brushes");
        }
    }
}