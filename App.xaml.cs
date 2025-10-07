using CKL_Studio.Common.Interfaces;
using CKL_Studio.Infrastructure.Services;
using CKL_Studio.Presentation.Services.Navigation;
using CKL_Studio.Presentation.ViewModels;
using CKL_Studio.Presentation.ViewModels.Base;
using CKL_Studio.Presentation.Views;
using CKL_Studio.Presentation.Windows;
using CKLLib;
using CKLDrawing;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;
using System.Data;
using System.Windows;
using CKL_Studio.Common.Interfaces.CKLInterfaces;
using CKL_Studio.Common.Interfaces.Factories;
using CKL_Studio.Infrastructure.Services.Factories;
using CKL_Studio.Presentation.ViewModels.Dialog;
using CKL_Studio.CKL_Tests;

namespace CKL_Studio
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; set; }

        public App()
        {
            var services = new ServiceCollection();
            ConfigureServices(services);
            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var navigationService = ServiceProvider.GetRequiredService<INavigationService>();
            navigationService.NavigateTo<EntryPointViewModel>();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<INavigationService>(provider =>
                new NavigationService(
                    type => (ViewModelBase)provider.GetRequiredService(type),
                    viewModel => CreateWindowForViewModel(viewModel),
                    viewModel => GetWindowTypeForViewModel(viewModel)
                ));

            services.AddTransient<CKL, CKL>();
            services.AddSingleton<CKLView, CKLView>();
            services.AddSingleton<INamingService, NamingService>();
            services.AddSingleton<IJsonToCklСonversion, CklConversionService>();
            services.AddSingleton<IDataService<CKL>, SolutionExplorerDataService>();
            services.AddSingleton<ISolutionExplorerDataServiceFactory, SolutionExplorerDataServiceFactory>();
            services.AddTransient<ICklOperationService, CklOperationService>();

            //Сервисы EntryPointViewModel
            services.AddSingleton<IDataService<FileData>, FileDataService>();
            services.AddSingleton<IDataService<string>, SearchHistoryDataService>();
            services.AddSingleton<IOpenCklService, OpenCklService>();
            services.AddSingleton<IDialogService, DialogService>();

            //ViewModels
            services.AddTransient<EntryPointViewModel>();
            services.AddTransient<CklCreationViewModel>();
            services.AddTransient<SourceInputViewModel>();
            services.AddTransient<RelationInputViewModel>();
            services.AddTransient<CklViewModel>();
            //Windows
            services.AddTransient<LoadDataWindow>();
            services.AddTransient<CKLWindow>();
        }

        private Window CreateWindowForViewModel(ViewModelBase viewModel)
        {
            return viewModel switch
            {
                CklViewModel _ => new CKLWindow { DataContext = viewModel },
                _ => new LoadDataWindow { DataContext = viewModel }
            };
        }

        private Type GetWindowTypeForViewModel(ViewModelBase viewModel)
        {
            return viewModel switch
            {
                CklViewModel _ => typeof(CKLWindow),
                _ => typeof(LoadDataWindow)
            };
        }     
    }
}
