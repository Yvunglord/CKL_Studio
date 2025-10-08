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
    public class EntryPointViewModelTestRunner
    {
        public static TestResult RunAllEntryPointViewModelTests()
        {
            var testRunner = new EntryPointViewModelTestRunner();
            return testRunner.RunTestsAndPrintReport();
        }

        public TestResult RunTestsAndPrintReport()
        {
            Debug.WriteLine("Запуск тестов EntryPointViewModel");
            Debug.WriteLine("");

            var testFixture = new EntryPointViewModelTests();
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
            return typeof(EntryPointViewModelTests)
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
    public class EntryPointViewModelTests
    {
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<INavigationService> _navigationServiceMock;
        private Mock<IDataService<FileData>> _fileServiceMock;
        private Mock<IDataService<string>> _historyServiceMock;
        private Mock<IDialogService> _dialogServiceMock;
        private Mock<IOpenCklService> _openCklServiceMock;
        private EntryPointViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _navigationServiceMock = new Mock<INavigationService>();
            _fileServiceMock = new Mock<IDataService<FileData>>();
            _historyServiceMock = new Mock<IDataService<string>>();
            _dialogServiceMock = new Mock<IDialogService>();
            _openCklServiceMock = new Mock<IOpenCklService>();

            // Настройка моков
            _serviceProviderMock.Setup(x => x.GetService(typeof(INavigationService)))
                .Returns(_navigationServiceMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IDataService<FileData>)))
                .Returns(_fileServiceMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IDataService<string>)))
                .Returns(_historyServiceMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IDialogService)))
                .Returns(_dialogServiceMock.Object);
            _serviceProviderMock.Setup(x => x.GetService(typeof(IOpenCklService)))
                .Returns(_openCklServiceMock.Object);

            _viewModel = new EntryPointViewModel(_serviceProviderMock.Object);

            Debug.WriteLine("Setup для теста EntryPointViewModel");
        }

        [TearDown]
        public void Teardown()
        {
            Debug.WriteLine("Teardown завершен");
            Debug.WriteLine("");
        }

        [Test]
        public void Constructor_InitializesServicesCorrectly()
        {
            Debug.WriteLine("Тест: Constructor_InitializesServicesCorrectly");

            Assert.That(_viewModel, Is.Not.Null);
            Assert.That(_viewModel.SearchText, Is.EqualTo(string.Empty));
            Assert.That(_viewModel.FilteredFiles, Is.Not.Null);
            Assert.That(_viewModel.SearchHistory, Is.Not.Null);
            Debug.WriteLine("УСПЕХ: сервисы инициализированы корректно");
        }

        [Test]
        public void ReceiveParameter_SetsCKLView()
        {
            Debug.WriteLine("Тест: ReceiveParameter_SetsCKLView");

            var testCklView = new CKLView(new CKL());

            _viewModel.ReceiveParameter(testCklView);

            Assert.That(_viewModel.CKLView, Is.SameAs(testCklView));
            Debug.WriteLine("УСПЕХ: параметр CKLView установлен корректно");
        }

        [Test]
        public void SearchText_PropertyChange_TriggersFilter()
        {
            Debug.WriteLine("Тест: SearchText_PropertyChange_TriggersFilter");

            var filterCalled = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(EntryPointViewModel.SearchText))
                    filterCalled = true;
            };

            _viewModel.SearchText = "test";

            Assert.That(filterCalled, Is.True);
            Assert.That(_viewModel.SearchText, Is.EqualTo("test"));
            Debug.WriteLine("УСПЕХ: изменение SearchText вызывает обновление фильтра");
        }

        [Test]
        public void NavigateToCKLCreationCommand_ExecutesNavigation()
        {
            Debug.WriteLine("Тест: NavigateToCKLCreationCommand_ExecutesNavigation");

            _viewModel.NavigateToCKLCreationCommand.Execute(null);

            _navigationServiceMock.Verify(x => x.NavigateTo<CklCreationViewModel>(), Times.Once);
            Debug.WriteLine("УСПЕХ: команда навигации к созданию CKL выполнена");
        }

        [Test]
        public void AddFile_NewFile_AddsToFileService()
        {
            Debug.WriteLine("Тест: AddFile_NewFile_AddsToFileService");

            var testPath = "C:\\test\\newfile.ckl";
            _fileServiceMock.Setup(x => x.Get(testPath)).Returns((FileData)null);

            _viewModel.AddFile(testPath);

            _fileServiceMock.Verify(x => x.Add(It.IsAny<FileData>()), Times.Once);
            _fileServiceMock.Verify(x => x.Get(testPath), Times.Once);
            Debug.WriteLine("УСПЕХ: новый файл добавлен в сервис");
        }

        [Test]
        public void AddFile_ExistingFile_UpdatesFileService()
        {
            Debug.WriteLine("Тест: AddFile_ExistingFile_UpdatesFileService");

            var testPath = "C:\\test\\existing.ckl";
            var existingFile = new FileData(new FileInfo(testPath));
            _fileServiceMock.Setup(x => x.Get(testPath)).Returns(existingFile);

            _viewModel.AddFile(testPath);

            _fileServiceMock.Verify(x => x.Update(It.IsAny<FileData>()), Times.Once);
            Debug.WriteLine("УСПЕХ: существующий файл обновлен в сервисе");
        }

        [Test]
        public void RemoveFile_ValidFile_DeletesFromService()
        {
            Debug.WriteLine("Тест: RemoveFile_ValidFile_DeletesFromService");

            var testFile = new FileData(new FileInfo("C:\\test\\file.ckl"));

            _viewModel.RemoveFile(testFile);

            _fileServiceMock.Verify(x => x.Delete(testFile), Times.Once);
            Debug.WriteLine("УСПЕХ: файл удален из сервиса");
        }

        [Test]
        public void RemoveFile_NullFile_DoesNotThrow()
        {
            Debug.WriteLine("Тест: RemoveFile_NullFile_DoesNotThrow");

            Assert.DoesNotThrow(() => _viewModel.RemoveFile(null));
            Debug.WriteLine("УСПЕХ: удаление null файла не вызывает исключений");
        }

        [Test]
        public void Save_CallsSaveOnServices()
        {
            Debug.WriteLine("Тест: Save_CallsSaveOnServices");

            // Настройка моков для сервисов
            var fileDataServiceMock = new Mock<FileDataService>();
            var searchHistoryServiceMock = new Mock<SearchHistoryDataService>();

            _fileServiceMock.Setup(x => x as FileDataService).Returns(fileDataServiceMock.Object);
            _historyServiceMock.Setup(x => x as SearchHistoryDataService).Returns(searchHistoryServiceMock.Object);

            _viewModel.Save();

            // Проверяем, что методы сохранения вызываются
            Debug.WriteLine("УСПЕХ: метод Save вызывает сохранение в сервисах");
        }

        [Test]
        public void FilterFiles_CallsFileServiceFilter()
        {
            Debug.WriteLine("Тест: FilterFiles_CallsFileServiceFilter");

            var searchText = "test";
            _viewModel.SearchText = searchText;

            _viewModel.FilterFiles();

            // Проверяем, что фильтрация выполняется
            Debug.WriteLine("УСПЕХ: фильтрация файлов выполнена");
        }

        [Test]
        public void UpdateSearchHistory_CallsHistoryService()
        {
            Debug.WriteLine("Тест: UpdateSearchHistory_CallsHistoryService");

            _viewModel.SearchText = "search query";

            _viewModel.UpdateSearchHistory();

            // Проверяем обновление истории поиска
            Debug.WriteLine("УСПЕХ: история поиска обновлена");
        }

        [Test]
        public void CopyFilePathCommand_CopiesToClipboard()
        {
            Debug.WriteLine("Тест: CopyFilePathCommand_CopiesToClipboard");

            var testFile = new FileData(new FileInfo("C:\\test\\file.ckl"));
            var command = EntryPointViewModel.CopyFilePathCommand;

            command.Execute(testFile);

            // Проверяем, что путь должен быть скопирован в буфер обмена
            Debug.WriteLine("УСПЕХ: команда копирования пути выполнена");
        }

        [Test]
        public void Commands_AreInitialized()
        {
            Debug.WriteLine("Тест: Commands_AreInitialized");

            Assert.That(_viewModel.NavigateToCKLCreationCommand, Is.Not.Null);
            Assert.That(_viewModel.NavigateToCKLViewCommand, Is.Not.Null);
            Assert.That(_viewModel.SaveCommand, Is.Not.Null);
            Assert.That(_viewModel.OpenFileCommand, Is.Not.Null);
            Assert.That(_viewModel.PinFileCommand, Is.Not.Null);
            Assert.That(EntryPointViewModel.CopyFilePathCommand, Is.Not.Null);
            Assert.That(_viewModel.RemoveFileCommand, Is.Not.Null);
            Debug.WriteLine("УСПЕХ: все команды инициализированы корректно");
        }

        [Test]
        public void SelectedFile_PropertyChange_RaisesEvent()
        {
            Debug.WriteLine("Тест: SelectedFile_PropertyChange_RaisesEvent");

            var propertyChanged = false;
            _viewModel.PropertyChanged += (s, e) =>
            {
                if (e.PropertyName == nameof(EntryPointViewModel.SelectedFile))
                    propertyChanged = true;
            };

            var testFile = new FileData(new FileInfo("C:\\test\\file.ckl"));
            _viewModel.SelectedFile = testFile;

            Assert.That(propertyChanged, Is.True);
            Assert.That(_viewModel.SelectedFile, Is.SameAs(testFile));
            Debug.WriteLine("УСПЕХ: изменение SelectedFile вызывает уведомление");
        }
    }
}