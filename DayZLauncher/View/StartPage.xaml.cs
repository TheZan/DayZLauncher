using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DayZLauncher.Model;
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
                LauncherSettings.Default.ProfileName = "Survivor";
                LauncherSettings.Default.Save();
                GamePathTextBox.Text = gamePath;
                CreateSettingsProfile();
                ShowOrHideFiles(FileAttributes.Normal, FileAttributes.Normal);
                SetParameter("-name=", " ", LauncherSettings.Default.ProfileName);
                ShowOrHideFiles(FileAttributes.Hidden, FileAttributes.System);
            }
            else
            {
                MessageBox.Show("Неверная папка!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Ok_OnClick(object sender, RoutedEventArgs e)
        {
            if (gamePath != "")
            {
                HideSteamId();
                DialogResult = true;
            }
        }

        private void CreateSettingsProfile()
        {
            ShowOrHideFiles(FileAttributes.Normal, FileAttributes.Normal);

            var myDocuments = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}";

            if (!Directory.Exists(@$"{myDocuments}\DayZ"))
            {
                Directory.CreateDirectory(@$"{myDocuments}\DayZ");

                using (FileStream fs = File.Create(@$"{myDocuments}\DayZ\{LauncherSettings.Default.ProfileName}_settings.DayZProfile"))
                {
                    var text = LauncherSettings.Default.DayZSettings;
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    fs.Write(array, 0, array.Length);
                }
            }
            else
            {
                using (FileStream fs = File.Create(@$"{myDocuments}\DayZ\{LauncherSettings.Default.ProfileName}_settings.DayZProfile"))
                {
                    var text = LauncherSettings.Default.DayZSettings;
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    fs.Write(array, 0, array.Length);
                }
            }

            if (!Directory.GetFiles(LauncherSettings.Default.GamePath).Where(p => p == "!StartGame.ini").Any())
            {
                using (FileStream fs = File.Create(@$"{LauncherSettings.Default.GamePath}\!StartGame.ini"))
                {
                    var text = LauncherSettings.Default.StartIni;
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    fs.Write(array, 0, array.Length);
                }
            }

            if (!Directory.GetFiles(LauncherSettings.Default.GamePath).Where(p => p == "!start_game.bat").Any())
            {
                using (FileStream fs = File.Create(@$"{LauncherSettings.Default.GamePath}\!start_game.bat"))
                {
                    var text = LauncherSettings.Default.StartGameBat;
                    byte[] array = System.Text.Encoding.Default.GetBytes(text);
                    fs.Write(array, 0, array.Length);
                }
            }

            ShowOrHideFiles(FileAttributes.Hidden, FileAttributes.System);
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

        private void SetParameter(string strStart, string strEnd, string newChar)
        {
            var settingsIni = new List<string>();

            using (StreamReader reader = new StreamReader($"{LauncherSettings.Default.GamePath}!StartGame.ini"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    settingsIni.Add(line);
                }
            }

            using (StreamWriter writer = new StreamWriter($"{LauncherSettings.Default.GamePath}!StartGame.ini"))
            {
                if (settingsIni[3].Contains(strStart) && settingsIni[3].Contains(strEnd))
                {
                    string line = settingsIni[3];
                    int Start, End;
                    Start = line.IndexOf(strStart, 0) + strStart.Length;
                    End = line.IndexOf(strEnd, Start);

                    settingsIni[3] = line.Remove(Start, End - Start).Insert(Start, newChar);
                }

                foreach (var line in settingsIni)
                {
                    writer.WriteLine(line);
                }

                writer.Close();
            }
        }

        private void ShowOrHideFiles(FileAttributes attributes, FileAttributes fileAttributes)
        {
            if (LauncherSettings.Default.GamePath != "")
            {
                var files = Directory.GetFiles(LauncherSettings.Default.GamePath).ToList();
                var hideFiles = files.Where(fileName =>
                    fileName == $"{LauncherSettings.Default.GamePath}!start_game.bat" ||
                    fileName == $"{LauncherSettings.Default.GamePath}!StartGame.ini").ToList();
                foreach (var file in hideFiles)
                {
                    File.SetAttributes(file, fileAttributes | attributes);
                }
            }
        }

        private void HideSteamId()
        {
            var roaming = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "SmartSteamEmu");
            if (Directory.Exists(roaming))
            {
                var files = Directory.GetFiles(roaming).ToList();
                var hideFile = files.FirstOrDefault(fileName =>
                    fileName == System.IO.Path.Combine(roaming, "steam_id.ini"));

                File.SetAttributes(hideFile, FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly);
            }
        }
    }
}
