using System.Windows;
using System.Windows.Threading;
using TreeChat.Models;

namespace TreeChat.Services
{
    /// <summary>
    /// 通知类型枚举
    /// </summary>
    public enum NotificationType
    {
        Error,
        Warning,
        Success,
        Info
    }

    /// <summary>
    /// 通知服务，提供统一的消息通知机制
    /// 支持错误、警告、成功、信息四种通知类型
    /// </summary>
    public class NotificationService
    {
        private static NotificationService? _instance;
        public static NotificationService Instance => _instance ??= new();

        /// <summary>
        /// 显示错误通知
        /// </summary>
        /// <param name="error">错误信息对象</param>
        public void ShowError(ErrorInfo error)
        {
            ShowToast(error.UserMessage, NotificationType.Error);
        }

        /// <summary>
        /// 显示错误通知（简化版）
        /// </summary>
        /// <param name="message">错误消息</param>
        public void ShowError(string message)
        {
            ShowToast(message, NotificationType.Error);
        }

        /// <summary>
        /// 显示警告通知
        /// </summary>
        /// <param name="message">警告消息</param>
        public void ShowWarning(string message)
        {
            ShowToast(message, NotificationType.Warning);
        }

        /// <summary>
        /// 显示成功通知
        /// </summary>
        /// <param name="message">成功消息</param>
        public void ShowSuccess(string message)
        {
            ShowToast(message, NotificationType.Success);
        }

        /// <summary>
        /// 显示信息通知
        /// </summary>
        /// <param name="message">信息内容</param>
        public void ShowInfo(string message)
        {
            ShowToast(message, NotificationType.Info);
        }

        private void ShowToast(string message, NotificationType type)
        {
            if (Application.Current?.Dispatcher == null) return;

            Application.Current.Dispatcher.Invoke(() =>
            {
                var caption = type switch
                {
                    NotificationType.Error => "错误",
                    NotificationType.Warning => "警告",
                    NotificationType.Success => "成功",
                    _ => "提示"
                };

                var icon = type switch
                {
                    NotificationType.Error => MessageBoxImage.Error,
                    NotificationType.Warning => MessageBoxImage.Warning,
                    NotificationType.Success => MessageBoxImage.Information,
                    _ => MessageBoxImage.Information
                };

                MessageBox.Show(message, caption, MessageBoxButton.OK, icon);
            }, DispatcherPriority.Normal);
        }
    }
}
