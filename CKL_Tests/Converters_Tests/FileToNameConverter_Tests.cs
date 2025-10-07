using CKL_Studio.Common.Converters;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace CKL_Studio.CKL_Tests
{
    public class FileToNameConverterTestRunner
    {
        public static TestResult RunAllFileToNameConverterTests()
        {
            var testRunner = new FileToNameConverterTestRunner();
            return testRunner.RunTestsAndPrintReport();
        }

        public TestResult RunTestsAndPrintReport()
        {
            Debug.WriteLine("Запуск тестов FileToNameConverter");
            Debug.WriteLine("");

            var testFixture = new FileToNameConverterTests();
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
            return typeof(FileToNameConverterTests)
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
    public class FileToNameConverterTests
    {
        private FileToNameConverter _converter;
        private CultureInfo _culture;

        [SetUp]
        public void Setup()
        {
            _converter = new FileToNameConverter();
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
        public void Convert_ValidFilePath_ReturnsFileName()
        {
            Debug.WriteLine("Тест: Convert_ValidFilePath_ReturnsFileName");

            var filePath = @"C:\Users\Test\Documents\file.txt";
            var result = _converter.Convert(filePath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo("file.txt"));
            Debug.WriteLine("УСПЕХ: полный путь преобразован в имя файла");
        }

        [Test]
        public void Convert_RelativeFilePath_ReturnsFileName()
        {
            Debug.WriteLine("Тест: Convert_RelativeFilePath_ReturnsFileName");

            var filePath = @"..\Documents\file.txt";
            var result = _converter.Convert(filePath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo("file.txt"));
            Debug.WriteLine("УСПЕХ: относительный путь преобразован в имя файла");
        }

        [Test]
        public void Convert_FileNameOnly_ReturnsSameFileName()
        {
            Debug.WriteLine("Тест: Convert_FileNameOnly_ReturnsSameFileName");

            var fileName = "file.txt";
            var result = _converter.Convert(fileName, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo("file.txt"));
            Debug.WriteLine("УСПЕХ: имя файла без пути возвращается как есть");
        }

        [Test]
        public void Convert_FilePathWithSpaces_ReturnsFileName()
        {
            Debug.WriteLine("Тест: Convert_FilePathWithSpaces_ReturnsFileName");

            var filePath = @"C:\My Documents\my file.txt";
            var result = _converter.Convert(filePath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo("my file.txt"));
            Debug.WriteLine("УСПЕХ: путь с пробелами корректно обработан");
        }

        [Test]
        public void Convert_FilePathWithSpecialCharacters_ReturnsFileName()
        {
            Debug.WriteLine("Тест: Convert_FilePathWithSpecialCharacters_ReturnsFileName");

            var filePath = @"C:\Test\file_123-v1.2.txt";
            var result = _converter.Convert(filePath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo("file_123-v1.2.txt"));
            Debug.WriteLine("УСПЕХ: путь со специальными символами корректно обработан");
        }

        [Test]
        public void Convert_EmptyString_ReturnsEmptyString()
        {
            Debug.WriteLine("Тест: Convert_EmptyString_ReturnsEmptyString");

            var result = _converter.Convert("", typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo(""));
            Debug.WriteLine("УСПЕХ: пустая строка возвращает пустую строку");
        }

        [Test]
        public void Convert_WhitespaceString_ReturnsEmptyString()
        {
            Debug.WriteLine("Тест: Convert_WhitespaceString_ReturnsEmptyString");

            var result = _converter.Convert("   ", typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo(""));
            Debug.WriteLine("УСПЕХ: строка с пробелами возвращает пустую строку");
        }

        [Test]
        public void Convert_NullValue_ReturnsEmptyString()
        {
            Debug.WriteLine("Тест: Convert_NullValue_ReturnsEmptyString");

            var result = _converter.Convert(null, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo(""));
            Debug.WriteLine("УСПЕХ: null возвращает пустую строку");
        }

        [Test]
        public void Convert_NonStringValue_ReturnsEmptyString()
        {
            Debug.WriteLine("Тест: Convert_NonStringValue_ReturnsEmptyString");

            var result = _converter.Convert(123, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo(""));
            Debug.WriteLine("УСПЕХ: не-string значение возвращает пустую строку");
        }

        [Test]
        public void Convert_DirectoryPath_ReturnsEmptyString()
        {
            Debug.WriteLine("Тест: Convert_DirectoryPath_ReturnsEmptyString");

            var dirPath = @"C:\Users\Test\Documents\";
            var result = _converter.Convert(dirPath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo(""));
            Debug.WriteLine("УСПЕХ: путь директории возвращает пустую строку");
        }

        [Test]
        public void Convert_RootDirectory_ReturnsEmptyString()
        {
            Debug.WriteLine("Тест: Convert_RootDirectory_ReturnsEmptyString");

            var rootPath = @"C:\";
            var result = _converter.Convert(rootPath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo(""));
            Debug.WriteLine("УСПЕХ: корневой путь возвращает пустую строку");
        }

        [Test]
        public void Convert_NetworkPath_ReturnsFileName()
        {
            Debug.WriteLine("Тест: Convert_NetworkPath_ReturnsFileName");

            var networkPath = @"\\server\share\file.txt";
            var result = _converter.Convert(networkPath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo("file.txt"));
            Debug.WriteLine("УСПЕХ: сетевой путь преобразован в имя файла");
        }

        [Test]
        public void Convert_UnixStylePath_ReturnsFileName()
        {
            Debug.WriteLine("Тест: Convert_UnixStylePath_ReturnsFileName");

            var unixPath = @"/home/user/documents/file.txt";
            var result = _converter.Convert(unixPath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo("file.txt"));
            Debug.WriteLine("УСПЕХ: Unix-стиль пути преобразован в имя файла");
        }

        [Test]
        public void Convert_FilePathWithExtension_ReturnsFileNameWithExtension()
        {
            Debug.WriteLine("Тест: Convert_FilePathWithExtension_ReturnsFileNameWithExtension");

            var filePath = @"C:\test\document.pdf";
            var result = _converter.Convert(filePath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo("document.pdf"));
            Debug.WriteLine("УСПЕХ: имя файла с расширением корректно извлечено");
        }

        [Test]
        public void Convert_FilePathWithoutExtension_ReturnsFileNameWithoutExtension()
        {
            Debug.WriteLine("Тест: Convert_FilePathWithoutExtension_ReturnsFileNameWithoutExtension");

            var filePath = @"C:\test\README";
            var result = _converter.Convert(filePath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo("README"));
            Debug.WriteLine("УСПЕХ: имя файла без расширения корректно извлечено");
        }

        [Test]
        public void Convert_WithParameter_IgnoresParameter()
        {
            Debug.WriteLine("Тест: Convert_WithParameter_IgnoresParameter");

            var filePath = @"C:\test\file.txt";
            var result1 = _converter.Convert(filePath, typeof(string), "some parameter", _culture);
            var result2 = _converter.Convert(filePath, typeof(string), null, _culture);

            Assert.That(result1, Is.EqualTo("file.txt"));
            Assert.That(result2, Is.EqualTo("file.txt"));
            Debug.WriteLine("УСПЕХ: параметр игнорируется, преобразование работает корректно");
        }

        [Test]
        public void Convert_WithDifferentCulture_ReturnsSameResult()
        {
            Debug.WriteLine("Тест: Convert_WithDifferentCulture_ReturnsSameResult");

            var cultureRu = new CultureInfo("ru-RU");
            var cultureEn = new CultureInfo("en-US");
            var filePath = @"C:\test\file.txt";

            var result1 = _converter.Convert(filePath, typeof(string), null, cultureRu);
            var result2 = _converter.Convert(filePath, typeof(string), null, cultureEn);

            Assert.That(result1, Is.EqualTo("file.txt"));
            Assert.That(result2, Is.EqualTo("file.txt"));
            Debug.WriteLine("УСПЕХ: разные культуры не влияют на результат");
        }

        [Test]
        public void Convert_TargetTypeIsNotString_ReturnsString()
        {
            Debug.WriteLine("Тест: Convert_TargetTypeIsNotString_ReturnsString");

            var filePath = @"C:\test\file.txt";
            var result = _converter.Convert(filePath, typeof(object), null, _culture);

            Assert.That(result, Is.EqualTo("file.txt"));
            Assert.That(result, Is.TypeOf<string>());
            Debug.WriteLine("УСПЕХ: результат всегда string, независимо от targetType");
        }

        [Test]
        public void ConvertBack_ThrowsNotImplementedException()
        {
            Debug.WriteLine("Тест: ConvertBack_ThrowsNotImplementedException");

            Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack("file.txt", typeof(string), null, _culture));

            Debug.WriteLine("УСПЕХ: ConvertBack выбрасывает NotImplementedException");
        }

        [Test]
        public void ConvertBack_WithDifferentParameters_ThrowsNotImplementedException()
        {
            Debug.WriteLine("Тест: ConvertBack_WithDifferentParameters_ThrowsNotImplementedException");

            Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack("file.txt", typeof(string), "param", _culture));

            Assert.Throws<NotImplementedException>(() =>
                _converter.ConvertBack("", typeof(object), null, _culture));

            Debug.WriteLine("УСПЕХ: ConvertBack всегда выбрасывает NotImplementedException");
        }

        [Test]
        public void Converter_IsStateless()
        {
            Debug.WriteLine("Тест: Converter_IsStateless");

            var filePath = @"C:\test\file.txt";
            var result1 = _converter.Convert(filePath, typeof(string), null, _culture);
            var result2 = _converter.Convert(filePath, typeof(string), null, _culture);
            var result3 = _converter.Convert("", typeof(string), null, _culture);

            Assert.That(result1, Is.EqualTo("file.txt"));
            Assert.That(result2, Is.EqualTo("file.txt"));
            Assert.That(result3, Is.EqualTo(""));
            Debug.WriteLine("УСПЕХ: конвертер работает стабильно при многократных вызовах");
        }

        [Test]
        public void Convert_VeryLongFileName_ReturnsCorrectName()
        {
            Debug.WriteLine("Тест: Convert_VeryLongFileName_ReturnsCorrectName");

            var longFileName = "very_long_file_name_with_many_characters_and_numbers_12345.txt";
            var filePath = $@"C:\test\{longFileName}";
            var result = _converter.Convert(filePath, typeof(string), null, _culture);

            Assert.That(result, Is.EqualTo(longFileName));
            Debug.WriteLine("УСПЕХ: очень длинное имя файла корректно извлечено");
        }
    }
}
