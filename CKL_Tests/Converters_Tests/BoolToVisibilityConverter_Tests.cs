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
    public class TestRunner
    {
        public static TestResult RunAllBoolToVisibilityConverterTests()
        {
            var testRunner = new TestRunner();
            return testRunner.RunTestsAndPrintReport();
        }

        public TestResult RunTestsAndPrintReport()
        {
            Debug.WriteLine("Запуск тестов BoolToVisibilityConverter");
            Debug.WriteLine("");

            var testFixture = new BoolToVisibilityConverterTests();
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
            return typeof(BoolToVisibilityConverterTests)
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

    public class TestResult
    {
        public int TotalTests { get; set; }
        public int PassedTests { get; set; }
        public List<TestFailure> FailedTests { get; set; } = new List<TestFailure>();

        public bool AllTestsPassed => FailedTests.Count == 0;
    }

    public class TestFailure
    {
        public string TestName { get; set; }
        public Exception Exception { get; set; }
    }

    [TestFixture]
    public class BoolToVisibilityConverterTests
    {
        private BoolToVisibilityConverter _converter;
        private CultureInfo _culture;

        [SetUp]
        public void Setup()
        {
            _converter = new BoolToVisibilityConverter();
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
        public void Convert_True_ReturnsVisible()
        {
            Debug.WriteLine("Тест: Convert_True_ReturnsVisible");

            var result = _converter.Convert(true, typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
            Debug.WriteLine("УСПЕХ: true преобразован в Visible");
        }

        [Test]
        public void Convert_False_ReturnsCollapsed()
        {
            Debug.WriteLine("Тест: Convert_False_ReturnsCollapsed");

            var result = _converter.Convert(false, typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
            Debug.WriteLine("УСПЕХ: false преобразован в Collapsed");
        }

        [Test]
        public void Convert_FalseWithUseHidden_ReturnsHidden()
        {
            Debug.WriteLine("Тест: Convert_FalseWithUseHidden_ReturnsHidden");

            _converter.UseHidden = true;
            var result = _converter.Convert(false, typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(Visibility.Hidden));
            Debug.WriteLine("УСПЕХ: false преобразован в Hidden при UseHidden=true");
        }

        [Test]
        public void Convert_TrueWithUseHidden_ReturnsVisible()
        {
            Debug.WriteLine("Тест: Convert_TrueWithUseHidden_ReturnsVisible");

            _converter.UseHidden = true;
            var result = _converter.Convert(true, typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
            Debug.WriteLine("УСПЕХ: true преобразован в Visible при UseHidden=true");
        }

        [Test]
        public void Convert_TrueWithInvert_ReturnsCollapsed()
        {
            Debug.WriteLine("Тест: Convert_TrueWithInvert_ReturnsCollapsed");

            _converter.Invert = true;
            var result = _converter.Convert(true, typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(Visibility.Collapsed));
            Debug.WriteLine("УСПЕХ: true преобразован в Collapsed при Invert=true");
        }

        [Test]
        public void Convert_FalseWithInvert_ReturnsVisible()
        {
            Debug.WriteLine("Тест: Convert_FalseWithInvert_ReturnsVisible");

            _converter.Invert = true;
            var result = _converter.Convert(false, typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
            Debug.WriteLine("УСПЕХ: false преобразован в Visible при Invert=true");
        }

        [Test]
        public void Convert_FalseWithInvertAndUseHidden_ReturnsHidden()
        {
            Debug.WriteLine("Тест: Convert_FalseWithInvertAndUseHidden_ReturnsHidden");

            _converter.Invert = true;
            _converter.UseHidden = true;
            var result = _converter.Convert(false, typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(Visibility.Hidden));
            Debug.WriteLine("УСПЕХ: false преобразован в Hidden при Invert=true и UseHidden=true");
        }

        [Test]
        public void Convert_TrueWithInvertAndUseHidden_ReturnsVisible()
        {
            Debug.WriteLine("Тест: Convert_TrueWithInvertAndUseHidden_ReturnsVisible");

            _converter.Invert = true;
            _converter.UseHidden = true;
            var result = _converter.Convert(true, typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(Visibility.Visible));
            Debug.WriteLine("УСПЕХ: true преобразован в Visible при Invert=true и UseHidden=true");
        }

        [Test]
        public void Convert_NonBoolValue_ReturnsUnsetValue()
        {
            Debug.WriteLine("Тест: Convert_NonBoolValue_ReturnsUnsetValue");

            var result = _converter.Convert("not a bool", typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(DependencyProperty.UnsetValue));
            Debug.WriteLine("УСПЕХ: не-bool значение возвращает UnsetValue");
        }

        [Test]
        public void Convert_NullValue_ReturnsUnsetValue()
        {
            Debug.WriteLine("Тест: Convert_NullValue_ReturnsUnsetValue");

            var result = _converter.Convert(null, typeof(Visibility), null, _culture);

            Assert.That(result, Is.EqualTo(DependencyProperty.UnsetValue));
            Debug.WriteLine("УСПЕХ: null возвращает UnsetValue");
        }

        [Test]
        public void ConvertBack_Visible_ReturnsTrue()
        {
            Debug.WriteLine("Тест: ConvertBack_Visible_ReturnsTrue");

            var result = _converter.ConvertBack(Visibility.Visible, typeof(bool), null, _culture);

            Assert.That(result, Is.EqualTo(true));
            Debug.WriteLine("УСПЕХ: Visible преобразован обратно в true");
        }

        [Test]
        public void ConvertBack_Collapsed_ReturnsFalse()
        {
            Debug.WriteLine("Тест: ConvertBack_Collapsed_ReturnsFalse");

            var result = _converter.ConvertBack(Visibility.Collapsed, typeof(bool), null, _culture);

            Assert.That(result, Is.EqualTo(false));
            Debug.WriteLine("УСПЕХ: Collapsed преобразован обратно в false");
        }

        [Test]
        public void ConvertBack_Hidden_ReturnsFalse()
        {
            Debug.WriteLine("Тест: ConvertBack_Hidden_ReturnsFalse");

            var result = _converter.ConvertBack(Visibility.Hidden, typeof(bool), null, _culture);

            Assert.That(result, Is.EqualTo(false));
            Debug.WriteLine("УСПЕХ: Hidden преобразован обратно в false");
        }

        [Test]
        public void ConvertBack_VisibleWithInvert_ReturnsFalse()
        {
            Debug.WriteLine("Тест: ConvertBack_VisibleWithInvert_ReturnsFalse");

            _converter.Invert = true;
            var result = _converter.ConvertBack(Visibility.Visible, typeof(bool), null, _culture);

            Assert.That(result, Is.EqualTo(false));
            Debug.WriteLine("УСПЕХ: Visible преобразован обратно в false при Invert=true");
        }

        [Test]
        public void ConvertBack_CollapsedWithInvert_ReturnsTrue()
        {
            Debug.WriteLine("Тест: ConvertBack_CollapsedWithInvert_ReturnsTrue");

            _converter.Invert = true;
            var result = _converter.ConvertBack(Visibility.Collapsed, typeof(bool), null, _culture);

            Assert.That(result, Is.EqualTo(true));
            Debug.WriteLine("УСПЕХ: Collapsed преобразован обратно в true при Invert=true");
        }

        [Test]
        public void ConvertBack_HiddenWithInvert_ReturnsTrue()
        {
            Debug.WriteLine("Тест: ConvertBack_HiddenWithInvert_ReturnsTrue");

            _converter.Invert = true;
            var result = _converter.ConvertBack(Visibility.Hidden, typeof(bool), null, _culture);

            Assert.That(result, Is.EqualTo(true));
            Debug.WriteLine("УСПЕХ: Hidden преобразован обратно в true при Invert=true");
        }

        [Test]
        public void ConvertBack_NonVisibilityValue_ReturnsUnsetValue()
        {
            Debug.WriteLine("Тест: ConvertBack_NonVisibilityValue_ReturnsUnsetValue");

            var result = _converter.ConvertBack("not visibility", typeof(bool), null, _culture);

            Assert.That(result, Is.EqualTo(DependencyProperty.UnsetValue));
            Debug.WriteLine("УСПЕХ: не-Visibility значение возвращает UnsetValue");
        }

        [Test]
        public void ConvertBack_NullValue_ReturnsUnsetValue()
        {
            Debug.WriteLine("Тест: ConvertBack_NullValue_ReturnsUnsetValue");

            var result = _converter.ConvertBack(null, typeof(bool), null, _culture);

            Assert.That(result, Is.EqualTo(DependencyProperty.UnsetValue));
            Debug.WriteLine("УСПЕХ: null возвращает UnsetValue");
        }

        [Test]
        public void Converter_StateIsIsolatedBetweenCalls()
        {
            Debug.WriteLine("Тест: Converter_StateIsIsolatedBetweenCalls");

            _converter.Invert = true;
            _converter.UseHidden = true;

            var result1 = _converter.Convert(true, typeof(Visibility), null, _culture);
            Assert.That(result1, Is.EqualTo(Visibility.Hidden));
            Debug.WriteLine("Первый вызов: true преобразован в Hidden");

            _converter.Invert = false;
            _converter.UseHidden = false;

            var result2 = _converter.Convert(true, typeof(Visibility), null, _culture);
            Assert.That(result2, Is.EqualTo(Visibility.Visible));
            Debug.WriteLine("Второй вызов: true преобразован в Visible после изменения состояния");
        }

        [Test]
        public void Convert_InvalidInput_DoesNotAffectConverterState()
        {
            Debug.WriteLine("Тест: Convert_InvalidInput_DoesNotAffectConverterState");

            _converter.Invert = true;
            _converter.UseHidden = true;

            var invalidResult = _converter.Convert("invalid", typeof(Visibility), null, _culture);
            Debug.WriteLine("Обработан невалидный ввод");

            var validResult = _converter.Convert(true, typeof(Visibility), null, _culture);
            Assert.That(validResult, Is.EqualTo(Visibility.Hidden));
            Assert.That(invalidResult, Is.EqualTo(DependencyProperty.UnsetValue));
            Debug.WriteLine("Состояние конвертера сохранено после исключения");
        }
    }
}