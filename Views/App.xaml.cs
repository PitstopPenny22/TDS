using Shared;
using System;
using System.Configuration;
using System.Diagnostics;
using System.Threading;
using System.Windows;
using ViewModels;
using System.Windows.Threading;

namespace Views
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainViewModel _mainViewModel;
        private void App_OnStartup(object sender, StartupEventArgs e)
        {
            Current.ShutdownMode = ShutdownMode.OnMainWindowClose;
            Debug.WriteLine("Application main thread: {0}", Thread.CurrentThread.ManagedThreadId);

            InitConfigSettings();

            Current.DispatcherUnhandledException += DispatcherOnUnhandledException;

            InitViewModels();
            CreateAndShowMainWindow();
        }

        private void DispatcherOnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            LogError(e.Exception);
        }

        private void LogError(Exception e)
        {
            Debug.WriteLine($"Application Exception: {e.Message} {Environment.NewLine} {e.StackTrace}");
        }

        private void InitViewModels()
        {
            _mainViewModel = new MainViewModel();
        }

        private void InitConfigSettings()
        {
            ConfigurationManager.AppSettings[AppUtils.AppSettings.AppId.ToString()] = "TDSDS";
            ConfigurationManager.AppSettings[AppUtils.AppSettings.OpenWeatherMapApiKey.ToString()] = "8e6138afab4aa2e9d5eb58fd8d590ade";
        }
     
        private void CreateAndShowMainWindow()
        {
            MainWindow = new MainWindow(_mainViewModel);
            Current.MainWindow = MainWindow;
            MainWindow.Show();
        }
    }
}