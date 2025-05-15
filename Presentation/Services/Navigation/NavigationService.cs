using CKL_Studio.Presentation.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CKL_Studio.Presentation.Services.Navigation
{
    public class NavigationService : INavigationService
    {
        private readonly Func<Type, ViewModelBase> _viewModelFactory;
        private readonly Func<ViewModelBase, Window> _windowFactory;
        private readonly Func<ViewModelBase, Type> _windowTypeResolver;
        private readonly Stack<ViewModelBase> _navigationStack = new();
        private readonly Stack<Window> _windowStack = new();

        public ViewModelBase? CurrentViewModel => _navigationStack.TryPeek(out var vm) ? vm : null;

        public NavigationService(
            Func<Type, ViewModelBase> viewModelFactory,
            Func<ViewModelBase, Window> windowFactory,
            Func<ViewModelBase, Type> windowTypeResolver)
        {
            _viewModelFactory = viewModelFactory ?? throw new ArgumentNullException(nameof(viewModelFactory));
            _windowFactory = windowFactory ?? throw new ArgumentNullException(nameof(windowFactory));
            _windowTypeResolver = windowTypeResolver ?? throw new ArgumentNullException(nameof(windowTypeResolver));
        }

        public void NavigateTo<TViewModel>() where TViewModel : ViewModelBase
        {
            var viewModel = _viewModelFactory(typeof(TViewModel));
            InternalNavigate(viewModel);
        }

        public void NavigateTo<TViewModel, TParameter>(TParameter parameter)
            where TViewModel : ViewModelBase, IParameterReceiver<TParameter>
            where TParameter : class
        {
            if (parameter == null)
                throw new ArgumentNullException(nameof(parameter));

            var viewModel = (TViewModel)_viewModelFactory(typeof(TViewModel));
            viewModel.ReceiveParameter(parameter);
            InternalNavigate(viewModel);
        }

        private void OnWindowClosed(object? sender, EventArgs e)
        {
            if (sender is Window closedWindow)
            {
                closedWindow.Closed -= OnWindowClosed;

                if (_windowStack.Count > 0 && _windowStack.Peek() == closedWindow)
                {
                    _windowStack.Pop();
                    _navigationStack.TryPop(out _);
                }
            }
        }

        public bool CanGoBack => _navigationStack.Count > 1;

        public void GoBack()
        {
            if (!CanGoBack) return;

            ViewModelBase? currentViewModel = _navigationStack.Count > 0 ? _navigationStack.Pop() : null;
            if (!_navigationStack.TryPeek(out ViewModelBase? previousViewModel)) return;

            if (previousViewModel == null || _windowStack.Count == 0) return;

            Type previousWindowType = _windowTypeResolver(previousViewModel);
            Window currentWindow = _windowStack.Peek();

            if (currentWindow.GetType() == previousWindowType)
            {
                currentWindow.DataContext = previousViewModel;
            }
            else
            {
                _windowStack.Pop();
                currentWindow.Close();

                if (_windowStack.TryPeek(out Window? previousWindow))
                {
                    previousWindow?.Show();
                }
            }

            OnCurrentViewChanged();
        }

        private void InternalNavigate(ViewModelBase viewModel)
        {
            if (viewModel == null) throw new ArgumentNullException(nameof(viewModel));

            Window? newWindow = _windowFactory(viewModel)
                ?? throw new InvalidOperationException("Window factory returned null");

            newWindow.Closed += OnWindowClosed;

            if (_windowStack.Count > 0 && _windowStack.Peek() is Window existingWindow)
            {
                existingWindow.Hide();
            }

            _windowStack.Push(newWindow);
            _navigationStack.Push(viewModel);
            newWindow.Show();

            OnCurrentViewChanged();
        }

        public void CloseAll()
        {
            while (_windowStack.Count > 0)
            {
                var window = _windowStack.Pop();
                window.Closed -= OnWindowClosed;
                window.Close();
            }
            _navigationStack.Clear();
        }

        private void OnCurrentViewChanged() =>
            CurrentViewChanged?.Invoke(this, EventArgs.Empty);

        public event EventHandler? CurrentViewChanged;
    }
}
