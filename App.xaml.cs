using System.Configuration;
using System.Data;
using System.Windows;
using System.Windows.Threading;

namespace sem3laba3;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Обработка необработанных исключений
        this.DispatcherUnhandledException += App_DispatcherUnhandledException;
    }

    private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
    {
        MessageBox.Show($"Произошла ошибка: {e.Exception.Message}\n\nStack Trace:\n{e.Exception.StackTrace}",
                       "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        e.Handled = true;
    }
}