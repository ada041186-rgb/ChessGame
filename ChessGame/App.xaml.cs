using ChessApplication;
using ChessApplication.DTO;
using ChessApplication.Interfaces.Utils;
using ChessGame.Utils;
using ChessGame.ViewModel;
using ChessGame.ViewModel.UserControlViewModels;
using ChessInfrastructure;
using ChessLibrary;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Windows;

namespace ChessGame
{
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; }

        public App()
        {
            var services = new ServiceCollection();

            services.AddChessDomain();

            services.AddChessInfrastructure();

            services.AddChessApplication();

            services.AddChessPresentation();

            services.AddLogging(builder =>
            {
                builder.ClearProviders();
                builder.AddDebug();
                builder.SetMinimumLevel(LogLevel.Debug);
            });

            services.AddSingleton<SynchronizationContext>(sp =>
                    SynchronizationContext.Current ?? new System.Windows.Threading.DispatcherSynchronizationContext());

            ServiceProvider = services.BuildServiceProvider();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var settingsService = ServiceProvider.GetRequiredService<ISettingsService>();
            var settings = settingsService.Load();

            var mainWindow = new MainWindow();
            ApplySettings(mainWindow, settings);

            mainWindow.DataContext = ServiceProvider.GetRequiredService<MainViewModel>();
            mainWindow.Show();

            ServiceProvider.GetRequiredService<INavigationService>()
                           .NavigateTo<MenuViewModel>();
        }

        private static void ApplySettings(Window window, SettingsData settings)
        {
            if (settings.IsFullScreen)
            {
                window.WindowStyle = WindowStyle.None;
                window.WindowState = WindowState.Maximized;
            }
            else
            {
                window.WindowStyle = WindowStyle.SingleBorderWindow;
                window.WindowState = WindowState.Normal;
                window.Width = settings.Width;
                window.Height = settings.Height;
                window.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
        }
    }
}