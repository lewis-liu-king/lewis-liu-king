using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using TreeChat.Models;
using TreeChat.ViewModels;
using TreeChat.Views;

namespace TreeChat.Views
{
    /// <summary>
    /// 主窗口，应用程序的主界面
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly MainWindowVM _vm = new();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _vm;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            _vm.ChatManagementPanelVM.SaveChats();
            base.OnClosing(e);
        }
    }
}
