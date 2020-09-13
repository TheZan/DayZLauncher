using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace DayZLauncher.View
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class General : Window
    {
        public General()
        {
            InitializeComponent();
        }

        private void TopPanel_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void EventSetter_OnHandler(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void EventSetter_OnHandler2(object sender, MouseButtonEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void GoToDiscord(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", "https://discord.gg/VETwBNG");
        }

        private void GoToVk(object sender, MouseButtonEventArgs e)
        {
            Process.Start("explorer.exe", "https://vk.com/dayzggames");
        }

        private void General_OnActivated(object? sender, EventArgs e)
        {
            Window window = Application.Current.Windows.OfType<General>().FirstOrDefault(p => p != this && !p.IsActive && p.OwnedWindows.Count > 0);
            if (window != null)
            {
                window.Activate();
            }
        }
    }
}
