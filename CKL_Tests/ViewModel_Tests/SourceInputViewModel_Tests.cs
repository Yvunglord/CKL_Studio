using CKL_Studio.Presentation.Commands;
using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels;
using CKL_Studio.Presentation.ViewModels.Base;
using CKLLib;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace CKL_Studio.CKL_Tests
{
    public class SourceInputViewModelTestRunner
    {
        public static TestResult RunAllSourceInputViewModelTests()
        {
            var testRunner = new SourceInputViewModelTestRunner();
            return testRunner.RunTestsAndPrintReport();
        }

        public TestResult RunTestsAndPrintReport()
        {
            Debug.WriteLine("Запуск тестов SourceInputViewModel");
            Debug.WriteLine("");

            var testFixture = new SourceInputViewModelTests();
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
            return typeof(SourceInputViewModelTests)
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
    public class SourceInputViewModelTests
    {
        private Mock<IServiceProvider> _serviceProviderMock;
        private Mock<INavigationService> _navigationServiceMock;
        private CKL _ckl;
        private SourceInputViewModel _viewModel;

        [SetUp]
        public void Setup()
        {
            _serviceProviderMock = new Mock<IServiceProvider>();
            _navigationServiceMock = new Mock<INavigationService>();

            _ckl = new CKL
            {
                FilePath = "C:\\test\\source.ckl",
                GlobalInterval = new TimeInterval(0, 100),
                Dimention = TimeDimentions.SECONDS,
                Source = new HashSet<Pair>()
            };

            // Настройка моков
            _serviceProviderMock.Setup(x => x.GetService(typeof(INavigationService)))
                .Returns(_navigationServiceMock.Object);

            _viewModel = new SourceInputViewModel(_serviceProviderMock.Object, _ckl);

            Debug.WriteLine("Setup для теста SourceInputViewModel");
        }

        [TearDown]
        public void Teardown()
        {
            Debug.WriteLine("Teardown завершен");
            Debug.WriteLine("");
        }

        [Test]
        public void Constructor_WithCKL_SetsPropertiesCorrectly()
        {
            Debug.WriteLine("Тест: Constructor_WithCKL_SetsPropertiesCorrectly");

            Assert.That(_viewModel.CKL, Is.SameAs(_ckl));
            Assert.That(_viewModel.Dim, Is.EqualTo(1));
            Assert.That(_viewModel.RowCount, Is.EqualTo(1));
            Assert.That(_viewModel.Source, Is.Not.Null);
            Assert.That(_viewModel.Source.Count, Is.EqualTo(1));
            Debug.WriteLine("УСПЕХ: свойства установлены корректно при передаче CKL");
        }

        [Test]
        public void Constructor_WithNullCKL_InitializesNewCKL()
        {
            Debug.WriteLine("Тест: Constructor_WithNullCKL_InitializesNewCKL");

            var viewModel = new SourceInputViewModel(_serviceProviderMock.Object, null);

            Assert.That(viewModel.CKL, Is.Not.Null);
            Debug.WriteLine("УСПЕХ: при null CKL создается новый объект");
        }

        [Test]
        public void ReceiveParameter_UpdatesCKLAndSourceStructure()
        {
            Debug.WriteLine("Тест: ReceiveParameter_UpdatesCKLAndSourceStructure");

            var newCkl = new CKL
            {
                FilePath = "C:\\new\\source.ckl",
                Source = new HashSet<Pair> { new Pair("A", "B") }
            };

            _viewModel.ReceiveParameter(newCkl);

            Assert.That(_viewModel.CKL, Is.SameAs(newCkl));
            Assert.That(_viewModel.Source, Is.Not.Empty);
            Debug.WriteLine("УСПЕХ: ReceiveParameter обновляет CKL и структуру источника");
        }

        [Test]
        public void Dim_PropertyChange_UpdatesSourceStructure()
        {
            Debug.WriteLine("Тест: Dim_PropertyChange_UpdatesSourceStructure");

            var initialCount = _viewModel.Source.Count;

            _viewModel.Dim = 2;

            Assert.That(_viewModel.Dim, Is.EqualTo(2));
            Assert.That(_viewModel.Source.Count, Is.EqualTo(initialCount));
            Debug.WriteLine("УСПЕХ: изменение Dim обновляет структуру источника");
        }

        [Test]
        public void RowCount_PropertyChange_UpdatesSourceStructure()
        {
            Debug.WriteLine("Тест: RowCount_PropertyChange_UpdatesSourceStructure");

            _viewModel.RowCount = 3;

            Assert.That(_viewModel.RowCount, Is.EqualTo(3));
            Assert.That(_viewModel.Source.Count, Is.EqualTo(3));
            Debug.WriteLine("УСПЕХ: изменение RowCount обновляет структуру источника");
        }

        [Test]
        public void AddRowCommand_IncreasesRowCount()
        {
            Debug.WriteLine("Тест: AddRowCommand_IncreasesRowCount");

            var initialCount = _viewModel.RowCount;

            _viewModel.AddRowCommand.Execute(null);

            Assert.That(_viewModel.RowCount, Is.EqualTo(initialCount + 1));
            Assert.That(_viewModel.Source.Count, Is.EqualTo(initialCount + 1));
            Debug.WriteLine("УСПЕХ: команда AddRow увеличивает количество строк");
        }

        [Test]
        public void RemoveRowCommand_DecreasesRowCount()
        {
            Debug.WriteLine("Тест: RemoveRowCommand_DecreasesRowCount");

            _viewModel.RowCount = 3; // Устанавливаем несколько строк
            var initialCount = _viewModel.RowCount;

            _viewModel.RemoveRowCommand.Execute(null);

            Assert.That(_viewModel.RowCount, Is.EqualTo(initialCount - 1));
            Assert.That(_viewModel.Source.Count, Is.EqualTo(initialCount - 1));
            Debug.WriteLine("УСПЕХ: команда RemoveRow уменьшает количество строк");
        }

        [Test]
        public void RemoveRowCommand_WhenNoRows_DoesNothing()
        {
            Debug.WriteLine("Тест: RemoveRowCommand_WhenNoRows_DoesNothing");

            _viewModel.RowCount = 0;
            var initialCount = _viewModel.RowCount;

            _viewModel.RemoveRowCommand.Execute(null);

            Assert.That(_viewModel.RowCount, Is.EqualTo(initialCount));
            Debug.WriteLine("УСПЕХ: команда RemoveRow не уменьшает количество строк при нулевых строках");
        }

        [Test]
        public void CanRemoveRow_WithRows_ReturnsTrue()
        {
            Debug.WriteLine("Тест: CanRemoveRow_WithRows_ReturnsTrue");

            _viewModel.RowCount = 2;

            Assert.That(_viewModel.CanRemoveRow, Is.True);
            Debug.WriteLine("УСПЕХ: CanRemoveRow возвращает true при наличии строк");
        }

        [Test]
        public void CanRemoveRow_WithoutRows_ReturnsFalse()
        {
            Debug.WriteLine("Тест: CanRemoveRow_WithoutRows_ReturnsFalse");

            _viewModel.RowCount = 0;

            Assert.That(_viewModel.CanRemoveRow, Is.False);
            Debug.WriteLine("УСПЕХ: CanRemoveRow возвращает false при отсутствии строк");
        }

        [Test]
        public void NavigateToRelationInputCommand_ExecutesSaveAndNavigation()
        {
            Debug.WriteLine("Тест: NavigateToRelationInputCommand_ExecutesSaveAndNavigation");

            _viewModel.NavigateToRelationInputCommand.Execute(null);

            _navigationServiceMock.Verify(x => x.NavigateTo<RelationInputViewModel, CKL>(_ckl), Times.Once);
            Debug.WriteLine("УСПЕХ: команда навигации вызывает сохранение и переход");
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
        public void SaveCommand_UpdatesCKLSource()
        {
            Debug.WriteLine("Тест: SaveCommand_UpdatesCKLSource");

            // Добавляем валидные данные
            _viewModel.Source.Clear();
            _viewModel.Source.Add(new Pair("A", "B"));

            _viewModel.SaveCommand.Execute(null);

            Assert.That(_ckl.Source, Is.Not.Null);
            Assert.That(_ckl.Source.Count, Is.GreaterThan(0));
            Debug.WriteLine("УСПЕХ: команда Save обновляет источник CKL");
        }

        [Test]
        public void Commands_AreInitialized()
        {
            Debug.WriteLine("Тест: Commands_AreInitialized");

            Assert.That(_viewModel.NavigateToRelationInputCommand, Is.Not.Null);
            Assert.That(_viewModel.GoBackCommand, Is.Not.Null);
            Assert.That(_viewModel.SaveCommand, Is.Not.Null);
            Assert.That(_viewModel.AddRowCommand, Is.Not.Null);
            Assert.That(_viewModel.RemoveRowCommand, Is.Not.Null);
            Debug.WriteLine("УСПЕХ: все команды инициализированы корректно");
        }

        [Test]
        public void PropertyChanged_Events_RaisedCorrectly()
        {
            Debug.WriteLine("Тест: PropertyChanged_Events_RaisedCorrectly");

            var changedProperties = new List<string>();
            _viewModel.PropertyChanged += (s, e) => changedProperties.Add(e.PropertyName);

            _viewModel.Dim = 2;
            _viewModel.RowCount = 3;
            _viewModel.SelectedPair = new Pair("Test");

            Assert.That(changedProperties, Contains.Item(nameof(SourceInputViewModel.Dim)));
            Assert.That(changedProperties, Contains.Item(nameof(SourceInputViewModel.RowCount)));
            Assert.That(changedProperties, Contains.Item(nameof(SourceInputViewModel.SelectedPair)));
            Debug.WriteLine("УСПЕХ: события PropertyChanged вызываются корректно");
        }

        [Test]
        public void Source_Collection_IsObservable()
        {
            Debug.WriteLine("Тест: Source_Collection_IsObservable");

            var collectionChanged = false;
            _viewModel.Source.CollectionChanged += (s, e) => collectionChanged = true;

            _viewModel.Source.Add(new Pair("Test"));

            Assert.That(collectionChanged, Is.True);
            Debug.WriteLine("УСПЕХ: коллекция Source поддерживает уведомления об изменениях");
        }
    }
}