using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TreeChat.Views
{
    public partial class ChatManagementPanel : UserControl
    {
        public ChatManagementPanel()
        {
            InitializeComponent();
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.ContextMenu != null)
            {
                var listBoxItem = FindParent<ListBoxItem>(button);
                if (listBoxItem != null)
                {
                    listBoxItem.IsSelected = true;
                }
                
                button.ContextMenu.PlacementTarget = button;
                button.ContextMenu.IsOpen = true;
            }
        }

        private T? FindParent<T>(DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);
            
            if (parentObject == null) return null;
            
            if (parentObject is T parent)
                return parent;
            else
                return FindParent<T>(parentObject);
        }
    }
}
