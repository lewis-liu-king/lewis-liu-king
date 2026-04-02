using System.Windows;
using TreeChat.ViewModels;

namespace TreeChat.Views
{
    /// <summary>
    /// 设置窗口，提供API配置的图形化编辑界面
    /// </summary>
    public partial class SettingsWindow : Window
    {
        /// <summary>
        /// 设置窗口的ViewModel实例
        /// </summary>
        public SettingsWindowVM ViewModel { get; }

        public SettingsWindow()
        {
            InitializeComponent();
            ViewModel = new SettingsWindowVM();
            DataContext = ViewModel;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}
