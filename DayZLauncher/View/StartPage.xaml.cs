using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace DayZLauncher.View
{
    /// <summary>
    /// Логика взаимодействия для StartPage.xaml
    /// </summary>
    public partial class StartPage : Window
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private string gamePath = "";

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog open = new OpenFileDialog(){Filter = "DayZ_x64.exe |*.exe" };
            if (open.ShowDialog() == true && open.FileName.Contains("DayZ_x64.exe"))
            {
                gamePath = open.FileName.TrimEnd(new char[]
                    {'D', 'a', 'y', 'Z', '_', 'x', '6', '4', '.', 'e', 'x', 'e'});
                LauncherSettings.Default.GamePath = gamePath;
                LauncherSettings.Default.Save();
                GamePathTextBox.Text = gamePath;
            }
            else
            {
                MessageBox.Show("Wrong folder!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            if (gamePath != "")
            {
                DialogResult = true;
            }
        }

        private void Cancel_OnClick(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void UIElement_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }

        private void ExitButton_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DialogResult = false;
        }
    }
}
