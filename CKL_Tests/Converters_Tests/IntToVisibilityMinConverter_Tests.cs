using CKL_Studio.Common.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Windows;

namespace CKL_Studio.CKL_Tests
{
    public class IntToVisibilityMinConverterTestRunner
    {
        public static TestResult RunAllIntToVisibilityMinConverterTests()
        {
            var testRunner = new IntToVisibilityMinConverterTestRunner();
            return testRunner.RunTestsAndPrintReport();
        }

        public TestResult RunTestsAndPrintReport()
        {
            Debug.WriteLine("Запуск тестов IntToVisibilityMinConverter");
            Debug.WriteLine("");

            var testFixture = new IntToVisibilityMinConverterTests();
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
            return typeof(IntToVisibilityMinConverterTests)
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
    public class IntToVisibilityMinConverterTests
    {
        private IntToVisibilityMinConverter _converter;
        private CultureInfo _culture;

        [SetUp]
        public void Setup()
        {
            _converter = new IntToVisibilityMinConverter();
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
        public void Convert_ValueGreaterThanMin_ReturnsVisible()
        {
            Debug.WriteLine("Тест: Convert_ValueGreaterThanMin_ReturnsVisible");

            var result = _converter.Convert(10, typeof(Visibility), "5", _culture);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
            Debug.WriteLine("УСПЕХ: значение 10 с параметром '5' возвращает Visible");
        }

        [Test]
        public void Convert_ValueEqualToMin_ReturnsVisible()
        {
            Debug.WriteLine("Тест: Convert_ValueEqualToMin_ReturnsVisible");

            var result = _converter.Convert(5, typeof(Visibility), "5", _culture);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
            Debug.WriteLine("УСПЕХ: значение 5 с параметром '5' возвращает Visible");
        }

        [Test]
        public void Convert_ValueLessThanMin_ReturnsCollapsed()
        {
            Debug.WriteLine("Тест: Convert_ValueLessThanMin_ReturnsCollapsed");

            var result = _converter.Convert(3, typeof(Visibility), "5", _culture);

            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
            Debug.WriteLine("УСПЕХ: значение 3 с параметром '5' возвращает Collapsed");
        }

        [Test]
        public void Convert_NegativeNumbers_WorksCorrectly()
        {
            Debug.WriteLine("Тест: Convert_NegativeNumbers_WorksCorrectly");

            var result1 = _converter.Convert(-3, typeof(Visibility), "-5", _culture);
            var result2 = _converter.Convert(-5, typeof(Visibility), "-3", _culture);

            Assert.That(result1, Is.EqualTo(Visibility.Visible));
            Assert.That(result2, Is.EqualTo(Visibility.Collapsed));
            Debug.WriteLine("УСПЕХ: отрицательные числа работают корректно");
        }

        [Test]
        public void Convert_ZeroValue_WorksCorrectly()
        {
            Debug.WriteLine("Тест: Convert_ZeroValue_WorksCorrectly");

            var result1 = _converter.Convert(0, typeof(Visibility), "0", _culture);
            var result2 = _converter.Convert(0, typeof(Visibility), "1", _culture);
            var result3 = _converter.Convert(1, typeof(Visibility), "0", _culture);

            Assert.That(result1, Is.EqualTo(Visibility.Visible));
            Assert.That(result2, Is.EqualTo(Visibility.Collapsed));
            Assert.That(result3, Is.EqualTo(Visibility.Visible));
            Debug.WriteLine("УСПЕХ: нулевое значение работает корректно");
        }

        [Test]
        public void Convert_NonIntValue_ReturnsCollapsed()
        {
            Debug.WriteLine("Тест: Convert_NonIntValue_ReturnsCollapsed");

            var result = _converter.Convert("not_int", typeof(Visibility), "5", _culture);

            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
            Debug.WriteLine("УСПЕХ: не-int значение возвращает Collapsed");
        }

        [Test]
        public void Convert_NullValue_ReturnsCollapsed()
        {
            Debug.WriteLine("Тест: Convert_NullValue_ReturnsCollapsed");

            var result = _converter.Convert(null, typeof(Visibility), "5", _culture);

            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
            Debug.WriteLine("УСПЕХ: null значение возвращает Collapsed");
        }

        [Test]
        public void Convert_NullParameter_ReturnsCollapsed()
        {
            Debug.WriteLine("Тест: Convert_NullParameter_ReturnsCollapsed");

            var result = _converter.Convert(5, typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
            Debug.WriteLine("УСПЕХ: null параметр возвращает Collapsed");
        }

        [Test]
        public void Convert_InvalidParameter_ReturnsCollapsed()
        {
            Debug.WriteLine("Тест: Convert_InvalidParameter_ReturnsCollapsed");

            var result = _converter.Convert(5, typeof(Visibility), "not_number", _culture);

            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
            Debug.WriteLine("УСПЕХ: нечисловой параметр возвращает Collapsed");
        }

        [Test]
        public void ConvertBack_ThrowsNotImplementedException()
        {
            Debug.WriteLine("Тест: ConvertBack_ThrowsNotImplementedException");

            Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack(Visibility.Visible, typeof(int), "5", _culture));

            Debug.WriteLine("УСПЕХ: ConvertBack выбрасывает NotImplementedException");
        }

        [Test]
        public void Converter_IsStateless()
        {
            Debug.WriteLine("Тест: Converter_IsStateless");

            var result1 = _converter.Convert(10, typeof(Visibility), "5", _culture);
            var result2 = _converter.Convert(10, typeof(Visibility), "5", _culture);
            var result3 = _converter.Convert(3, typeof(Visibility), "5", _culture);

            Assert.That(result1, Is.EqualTo(Visibility.Visible));
            Assert.That(result2, Is.EqualTo(Visibility.Visible));
            Assert.That(result3, Is.EqualTo(Visibility.Collapsed));
            Debug.WriteLine("УСПЕХ: конвертер работает стабильно при многократных вызовах");
        }
    }
}