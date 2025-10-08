using CKL_Studio.Common.Interfaces;
using CKL_Studio.Common.Interfaces.CKLInterfaces;
using CKL_Studio.Infrastructure.Services;
using CKL_Studio.Presentation.Commands;
using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels;
using CKL_Studio.Presentation.ViewModels.Base;
using CKLDrawing;
using CKLLib;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Input;

namespace CKL_Studio.CKL_Tests
{
    public class CklCreationViewModelTestRunner
    {
        public static TestResult RunAllCklCreationViewModelTests()
        {
            var testRunner = new CklCreationViewModelTestRunner();
            return testRunner.RunTestsAndPrintReport();
        }

        public TestResult RunTestsAndPrintReport()
        {
            Debug.WriteLine("Запуск тестов CklCreationViewModel");
            Debug.WriteLine("");

            var testFixture = new CklCreationViewModelTests();
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
            return typeof(CklCreationViewModelTests)
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
    public class CklCreationViewModelTests
    {
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<INavigationService> _navigationServiceMock;
        private Mock<IDialogService> _dialogServiceMock;
        private Mock<IJsonToCklСonversion> _conversionServiceMock;
        private Mock<INamingService> _namingServiceMock;
        private Mock<IDataService<FileData>> _fileServiceMock;
        private CKL _ckl;
        private CklCreationViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _navigationServiceMock = new Mock<INavigationService>();
            _dialogServiceMock = new Mock<IDialogService>();
            _conversionServiceMock = new Mock<IJsonToCklСonversion>();
            _namingServiceMock = new Mock<INamingService>();
            _fileServiceMock = new Mock<IDataService<FileData>>();

            _ckl = new CKL();

            // Настройка моков
            _serviceProviderMock.Setup(x => x.GetService(typeof(INavigationService)))
                .Returns(_navigationServiceMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IDialogService)))
                .Returns(_dialogServiceMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IJsonToCklСonversion)))
                .Returns(_conversionServiceMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(INamingService)))
                .Returns(_namingServiceMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IDataService<FileData>)))
                .Returns(_fileServiceMock.Object);

            _namingServiceMock.Setup(x => x.GeneratePath(It.IsAny<string>(), It.IsAny<string>()))
                .Returns("C:\\test\\Project1.ckl");
            _namingServiceMock.Setup(x => x.UpdatePath(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>()))
                .Returns((string path, string name, bool manual) => $"C:\\test\\{name}.ckl");

            _viewModel = new CklCreationViewModel(_serviceProviderMock.Object, _ckl);

            Debug.WriteLine("Setup для теста CklCreationViewModel");
        }

        [TearDown]
        public void Teardown()
        {
            Debug.WriteLine("Teardown завершен");
            Debug.WriteLine("");
        }

        [Test]
        public void Constructor_WithNullCKL_InitializesNewCKL()
        {
            Debug.WriteLine("Тест: Constructor_WithNullCKL_InitializesNewCKL");

            var viewModel = new CklCreationViewModel(_serviceProviderMock.Object, null);

            Assert.That(viewModel.CKL, Is.Not.Null);
            Debug.WriteLine("УСПЕХ: при null CKL создается новый объект");
        }

        [Test]
        public void Constructor_WithCKL_SetsPropertiesCorrectly()
        {
            Debug.WriteLine("Тест: Constructor_WithCKL_SetsPropertiesCorrectly");

            var testCkl = new CKL { FilePath = "C:\\test\\existing.ckl" };
            var viewModel = new CklCreationViewModel(_serviceProviderMock.Object, testCkl);

            Assert.That(viewModel.CKL, Is.SameAs(testCkl));
            Assert.That(viewModel.Name, Is.EqualTo("existing"));
            Debug.WriteLine("УСПЕХ: свойства установлены корректно при передаче CKL");
        }

        [Test]
        public void Name_PropertyChange_UpdatesFilePath()
        {
            Debug.WriteLine("Тест: Name_PropertyChange_UpdatesFilePath");

            var oldName = _viewModel.Name;
            var newName = "NewProject";

            _viewModel.Name = newName;

            Assert.That(_viewModel.Name, Is.EqualTo(newName));
            Debug.WriteLine("УСПЕХ: изменение Name обновляет FilePath");
        }

        [Test]
        public void FilePath_PropertyChange_UpdatesName()
        {
            Debug.WriteLine("Тест: FilePath_PropertyChange_UpdatesName");

            _viewModel.FilePath = "C:\\test\\NewName.ckl";

            Assert.That(_viewModel.Name, Is.EqualTo("NewName"));
            Debug.WriteLine("УСПЕХ: изменение FilePath обновляет Name");
        }

        [Test]
        public void Name_Validation_EmptyName_ReturnsError()
        {
            Debug.WriteLine("Тест: Name_Validation_EmptyName_ReturnsError");

            _viewModel.Name = "";

            var error = _viewModel["Name"];

            Assert.That(error, Is.Not.Empty);
            Assert.That(error, Contains.Substring("обязательно"));
            Debug.WriteLine("УСПЕХ: пустое имя возвращает ошибку валидации");
        }

        [Test]
        public void Name_Validation_InvalidCharacters_ReturnsError()
        {
            Debug.WriteLine("Тест: Name_Validation_InvalidCharacters_ReturnsError");

            _viewModel.Name = "file<name>";

            var error = _viewModel["Name"];

            Assert.That(error, Is.Not.Empty);
            Assert.That(error, Contains.Substring("Недопустимые символы"));
            Debug.WriteLine("УСПЕХ: недопустимые символы в имени возвращают ошибку");
        }

        [Test]
        public void Name_Validation_ValidName_ReturnsEmpty()
        {
            Debug.WriteLine("Тест: Name_Validation_ValidName_ReturnsEmpty");

            _viewModel.Name = "ValidFileName";

            var error = _viewModel["Name"];

            Assert.That(error, Is.Empty);
            Debug.WriteLine("УСПЕХ: валидное имя не возвращает ошибок");
        }

        [Test]
        public void TimeInterval_Validation_NegativeStartTime_ReturnsError()
        {
            Debug.WriteLine("Тест: TimeInterval_Validation_NegativeStartTime_ReturnsError");

            _viewModel.CKL.GlobalInterval.StartTime = -1;

            var error = _viewModel["CKL.GlobalInterval.StartTime"];

            Assert.That(error, Is.Not.Empty);
            Assert.That(error, Contains.Substring("отрицательным"));
            Debug.WriteLine("УСПЕХ: отрицательное начальное время возвращает ошибку");
        }

        [Test]
        public void TimeInterval_Validation_StartTimeGreaterThanEndTime_ReturnsError()
        {
            Debug.WriteLine("Тест: TimeInterval_Validation_StartTimeGreaterThanEndTime_ReturnsError");

            _viewModel.CKL.GlobalInterval.StartTime = 10;
            _viewModel.CKL.GlobalInterval.EndTime = 5;

            var error = _viewModel["CKL.GlobalInterval.StartTime"];

            Assert.That(error, Is.Not.Empty);
            Assert.That(error, Contains.Substring("больше конечного"));
            Debug.WriteLine("УСПЕХ: начальное время больше конечного возвращает ошибку");
        }

        [Test]
        public void FilePath_Validation_EmptyPath_ReturnsError()
        {
            Debug.WriteLine("Тест: FilePath_Validation_EmptyPath_ReturnsError");

            _viewModel.FilePath = "";

            var error = _viewModel["FilePath"];

            Assert.That(error, Is.Not.Empty);
            Assert.That(error, Contains.Substring("обязателен"));
            Debug.WriteLine("УСПЕХ: пустой путь возвращает ошибку");
        }

        [Test]
        public void AllErrors_Property_ReturnsAllValidationErrors()
        {
            Debug.WriteLine("Тест: AllErrors_Property_ReturnsAllValidationErrors");

            _viewModel.Name = "";
            _viewModel.CKL.GlobalInterval.StartTime = -1;

            var allErrors = _viewModel.AllErrors;

            Assert.That(allErrors, Is.Not.Empty);
            Assert.That(allErrors, Contains.Substring("Имя файла обязательно"));
            Assert.That(allErrors, Contains.Substring("Время не может быть отрицательным"));
            Debug.WriteLine("УСПЕХ: AllErrors возвращает все ошибки валидации");
        }

        [Test]
        public void HasErrors_Property_ReturnsCorrectValue()
        {
            Debug.WriteLine("Тест: HasErrors_Property_ReturnsCorrectValue");

            _viewModel.Name = "ValidName";
            Assert.That(_viewModel.HasErrors, Is.False);

            _viewModel.Name = "";
            Assert.That(_viewModel.HasErrors, Is.True);

            Debug.WriteLine("УСПЕХ: HasErrors корректно отражает наличие ошибок");
        }

        [Test]
        public void CanNavigateToSourceInput_WithErrors_ReturnsFalse()
        {
            Debug.WriteLine("Тест: CanNavigateToSourceInput_WithErrors_ReturnsFalse");

            _viewModel.Name = "";

            var command = _viewModel.NavigateToSourceInputCommand as RelayCommand;
            var canExecute = command.CanExecute(null);

            Assert.That(canExecute, Is.False);
            Debug.WriteLine("УСПЕХ: навигация заблокирована при наличии ошибок");
        }

        [Test]
        public void CanNavigateToSourceInput_WithoutErrors_ReturnsTrue()
        {
            Debug.WriteLine("Тест: CanNavigateToSourceInput_WithoutErrors_ReturnsTrue");

            _viewModel.Name = "ValidName";
            _viewModel.CKL.GlobalInterval.StartTime = 0;
            _viewModel.CKL.GlobalInterval.EndTime = 10;
            _viewModel.FilePath = "C:\\test\\file.ckl";

            var command = _viewModel.NavigateToSourceInputCommand as RelayCommand;
            var canExecute = command.CanExecute(null);

            Assert.That(canExecute, Is.True);
            Debug.WriteLine("УСПЕХ: навигация разрешена при отсутствии ошибок");
        }

        [Test]
        public void GoBackCommand_ExecutesNavigation()
        {
            Debug.WriteLine("Тест: GoBackCommand_ExecutesNavigation");

            _viewModel.GoBackCommand.Execute(null);

            _navigationServiceMock.Verify(x => x.GoBack(), Times.Once);
            Debug.WriteLine("УСПЕХ: команда GoBack вызывает навигацию назад");
        }

        [Test]
        public void SelectFilePathCommand_OpensDialog()
        {
            Debug.WriteLine("Тест: SelectFilePathCommand_OpensDialog");

            _dialogServiceMock.Setup(x => x.ShowSaveFileDialog(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns("C:\\test\\selected.ckl");

            _viewModel.SelectFilePathCommand.Execute(null);

            _dialogServiceMock.Verify(x => x.ShowSaveFileDialog(
                It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>()), Times.Once);
            Debug.WriteLine("УСПЕХ: команда выбора пути открывает диалог");
        }

        [Test]
        public void ReceiveParameter_UpdatesCKL()
        {
            Debug.WriteLine("Тест: ReceiveParameter_UpdatesCKL");

            var newCkl = new CKL { FilePath = "C:\\new\\path.ckl" };

            _viewModel.ReceiveParameter(newCkl);

            Assert.That(_viewModel.CKL, Is.SameAs(newCkl));
            Assert.That(_viewModel.Name, Is.EqualTo("path"));
            Debug.WriteLine("УСПЕХ: ReceiveParameter обновляет CKL и связанные свойства");
        }

        [Test]
        public void TimeDimensions_Property_ReturnsAllEnumValues()
        {
            Debug.WriteLine("Тест: TimeDimensions_Property_ReturnsAllEnumValues");

            var timeDimensions = CklCreationViewModel.TimeDimensions;

            Assert.That(timeDimensions, Is.Not.Null);
            Assert.That(timeDimensions, Is.Not.Empty);
            Assert.That(timeDimensions.Count(), Is.EqualTo(Enum.GetValues(typeof(TimeDimentions)).Length));
            Debug.WriteLine("УСПЕХ: TimeDimensions возвращает все значения перечисления");
        }

        [Test]
        public void PropertyChanged_Event_RaisesForDependentProperties()
        {
            Debug.WriteLine("Тест: PropertyChanged_Event_RaisesForDependentProperties");

            var changedProperties = new List<string>();
            _viewModel.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName);

            _viewModel.Name = "NewName";

            Assert.That(changedProperties, Contains.Item(nameof(CklCreationViewModel.AllErrors)));
            Assert.That(changedProperties, Contains.Item(nameof(CklCreationViewModel.HasErrors)));
            Debug.WriteLine("УСПЕХ: изменение свойств вызывает обновление зависимых свойств");
        }

        [Test]
        public void AddFile_NewFile_AddsToFileService()
        {
            Debug.WriteLine("Тест: AddFile_NewFile_AddsToFileService");

            var filePath = "C:\\test\\file.ckl";
            _fileServiceMock.Setup(x => x.Get(filePath)).Returns((FileData)null);

            _viewModel.AddFile(filePath);

            _fileServiceMock.Verify(x => x.Add(It.IsAny<FileData>()), Times.Once);
            Debug.WriteLine("УСПЕХ: новый файл добавляется в сервис");
        }

        [Test]
        public void AddFile_ExistingFile_UpdatesFileService()
        {
            Debug.WriteLine("Тест: AddFile_ExistingFile_UpdatesFileService");

            var filePath = "C:\\test\\file.ckl";
            var existingFile = new FileData(new FileInfo(filePath));
            _fileServiceMock.Setup(x => x.Get(filePath)).Returns(existingFile);

            _viewModel.AddFile(filePath);

            _fileServiceMock.Verify(x => x.Update(It.IsAny<FileData>()), Times.Once);
            Debug.WriteLine("УСПЕХ: существующий файл обновляется в сервисе");
        }
    }
}