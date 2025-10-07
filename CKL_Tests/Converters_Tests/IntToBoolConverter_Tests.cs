using CKL_Studio.Common.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace CKL_Studio.CKL_Tests
{
    public class IntToBoolConverterTestRunner
    {
        public static TestResult RunAllIntToBoolConverterTests()
        {
            var testRunner = new IntToBoolConverterTestRunner();
            return testRunner.RunTestsAndPrintReport();
        }

        public TestResult RunTestsAndPrintReport()
        {
            Debug.WriteLine("Запуск тестов IntToBoolConverter");
            Debug.WriteLine("");

            var testFixture = new IntToBoolConverterTests();
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
            return typeof(IntToBoolConverterTests)
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
    public class IntToBoolConverterTests
    {
        private IntToBoolConverter _converter;
        private CultureInfo _culture;

        [SetUp]
        public void Setup()
        {
            _converter = new IntToBoolConverter();
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
        public void Convert_IntValueMatchesParameter_ReturnsTrue()
        {
            Debug.WriteLine("Тест: Convert_IntValueMatchesParameter_ReturnsTrue");

            var result = _converter.Convert(5, typeof(bool), "5", _culture);

            Assert.That(result, Is.EqualTo(true));
            Debug.WriteLine("УСПЕХ: значение 5 с параметром '5' возвращает true");
        }

        [Test]
        public void Convert_IntValueDoesNotMatchParameter_ReturnsFalse()
        {
            Debug.WriteLine("Тест: Convert_IntValueDoesNotMatchParameter_ReturnsFalse");

            var result = _converter.Convert(5, typeof(bool), "10", _culture);

            Assert.That(result, Is.EqualTo(false));
            Debug.WriteLine("УСПЕХ: значение 5 с параметром '10' возвращает false");
        }

        [Test]
        public void Convert_NonIntValue_ReturnsFalse()
        {
            Debug.WriteLine("Тест: Convert_NonIntValue_ReturnsFalse");

            var result = _converter.Convert("not_int", typeof(bool), "5", _culture);

            Assert.That(result, Is.EqualTo(false));
            Debug.WriteLine("УСПЕХ: не-int значение возвращает false");
        }

        [Test]
        public void Convert_NullValue_ReturnsFalse()
        {
            Debug.WriteLine("Тест: Convert_NullValue_ReturnsFalse");

            var result = _converter.Convert(null, typeof(bool), "5", _culture);

            Assert.That(result, Is.EqualTo(false));
            Debug.WriteLine("УСПЕХ: null значение возвращает false");
        }

        [Test]
        public void Convert_NullParameter_ReturnsFalse()
        {
            Debug.WriteLine("Тест: Convert_NullParameter_ReturnsFalse");

            var result = _converter.Convert(5, typeof(bool), null, _culture);

            Assert.That(result, Is.EqualTo(false));
            Debug.WriteLine("УСПЕХ: null параметр возвращает false");
        }

        [Test]
        public void Convert_InvalidParameter_ReturnsFalse()
        {
            Debug.WriteLine("Тест: Convert_InvalidParameter_ReturnsFalse");

            var result = _converter.Convert(5, typeof(bool), "not_number", _culture);

            Assert.That(result, Is.EqualTo(false));
            Debug.WriteLine("УСПЕХ: нечисловой параметр возвращает false");
        }

        [Test]
        public void ConvertBack_TrueValueWithValidParameter_ReturnsParameterValue()
        {
            Debug.WriteLine("Тест: ConvertBack_TrueValueWithValidParameter_ReturnsParameterValue");

            var result = _converter.ConvertBack(true, typeof(int), "5", _culture);

            Assert.That(result, Is.EqualTo(5));
            Debug.WriteLine("УСПЕХ: true с параметром '5' возвращает 5");
        }

        [Test]
        public void ConvertBack_FalseValue_ReturnsZero()
        {
            Debug.WriteLine("Тест: ConvertBack_FalseValue_ReturnsZero");

            var result = _converter.ConvertBack(false, typeof(int), "5", _culture);

            Assert.That(result, Is.EqualTo(0));
            Debug.WriteLine("УСПЕХ: false возвращает 0");
        }

        [Test]
        public void ConvertBack_NonBoolValue_ReturnsZero()
        {
            Debug.WriteLine("Тест: ConvertBack_NonBoolValue_ReturnsZero");

            var result = _converter.ConvertBack("not_bool", typeof(int), "5", _culture);

            Assert.That(result, Is.EqualTo(0));
            Debug.WriteLine("УСПЕХ: не-bool значение возвращает 0");
        }

        [Test]
        public void ConvertBack_NullValue_ReturnsZero()
        {
            Debug.WriteLine("Тест: ConvertBack_NullValue_ReturnsZero");

            var result = _converter.ConvertBack(null, typeof(int), "5", _culture);

            Assert.That(result, Is.EqualTo(0));
            Debug.WriteLine("УСПЕХ: null значение возвращает 0");
        }

        [Test]
        public void ConvertBack_NullParameter_ReturnsZero()
        {
            Debug.WriteLine("Тест: ConvertBack_NullParameter_ReturnsZero");

            var result = _converter.ConvertBack(true, typeof(int), null, _culture);

            Assert.That(result, Is.EqualTo(0));
            Debug.WriteLine("УСПЕХ: null параметр возвращает 0");
        }

        [Test]
        public void ConvertBack_InvalidParameter_ReturnsZero()
        {
            Debug.WriteLine("Тест: ConvertBack_InvalidParameter_ReturnsZero");

            var result = _converter.ConvertBack(true, typeof(int), "not_number", _culture);

            Assert.That(result, Is.EqualTo(0));
            Debug.WriteLine("УСПЕХ: нечисловой параметр возвращает 0");
        }

        [Test]
        public void Converter_IsStateless()
        {
            Debug.WriteLine("Тест: Converter_IsStateless");

            var result1 = _converter.Convert(5, typeof(bool), "5", _culture);
            var result2 = _converter.Convert(5, typeof(bool), "5", _culture);
            var result3 = _converter.ConvertBack(true, typeof(int), "5", _culture);

            Assert.That(result1, Is.EqualTo(true));
            Assert.That(result2, Is.EqualTo(true));
            Assert.That(result3, Is.EqualTo(5));
            Debug.WriteLine("УСПЕХ: конвертер работает стабильно при многократных вызовах");
        }

        [Test]
        public void Convert_NegativeNumbers_WorksCorrectly()
        {
            Debug.WriteLine("Тест: Convert_NegativeNumbers_WorksCorrectly");

            var result1 = _converter.Convert(-5, typeof(bool), "-5", _culture);
            var result2 = _converter.Convert(-5, typeof(bool), "5", _culture);

            Assert.That(result1, Is.EqualTo(true));
            Assert.That(result2, Is.EqualTo(false));
            Debug.WriteLine("УСПЕХ: отрицательные числа работают корректно");
        }

        [Test]
        public void Convert_ZeroValue_WorksCorrectly()
        {
            Debug.WriteLine("Тест: Convert_ZeroValue_WorksCorrectly");

            var result1 = _converter.Convert(0, typeof(bool), "0", _culture);
            var result2 = _converter.Convert(0, typeof(bool), "1", _culture);

            Assert.That(result1, Is.EqualTo(true));
            Assert.That(result2, Is.EqualTo(false));
            Debug.WriteLine("УСПЕХ: нулевое значение работает корректно");
        }
    }
}