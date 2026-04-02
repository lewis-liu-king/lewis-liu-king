using System.Windows;
using System.Windows.Threading;
using TreeChat.Models;
using TreeChat.Services;

namespace TreeChat
{
    /// <summary>
    /// 应用程序入口类，处理全局异常和应用程序生命周期
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            RegisterExceptionHandlers();
            base.OnStartup(e);
        }

        /// <summary>
        /// 注册全局异常处理器，捕获未处理的异常
        /// </summary>
        private void RegisterExceptionHandlers()
        {
            DispatcherUnhandledException += OnDispatcherUnhandledException;
            TaskScheduler.UnobservedTaskException += OnUnobservedTaskException;
            AppDomain.CurrentDomain.UnhandledException += OnDomainUnhandledException;
        }

        private void OnDispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            var error = ErrorInfo.FromException(e.Exception);
            NotificationService.Instance.ShowError(error);
            e.Handled = true;
        }

        private void OnUnobservedTaskException(object? sender, UnobservedTaskExceptionEventArgs e)
        {
            var error = ErrorInfo.FromException(e.Exception);
            NotificationService.Instance.ShowError(error);
            e.SetObserved();
        }

        private void OnDomainUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception ex)
            {
                var error = ErrorInfo.FromException(ex);
                NotificationService.Instance.ShowError(error);
            }
        }
    }
}
