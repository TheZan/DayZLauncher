using DayZLauncher.Model;
using DayZLauncher.Navigation;
using DayZLauncher.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DayZLauncher.View;
using System.IO;
using System.Linq;

namespace DayZLauncher.ViewModel
{
    public class ServersPageViewModel : BaseViewModel, IPageViewModel
    {
        public ServersPageViewModel()
        {
            SelectedServer = new Server();
            Servers = ServersMonitoring.GetServersInfo();
            Task.Run(RefreshMonitoring);
        }

        private async void RefreshMonitoring()
        {
            while (true)
            {
                Servers = await Task.Run(ServersMonitoring.GetServersInfo);
                await Task.Delay(15000);
            }
        }

        private void ConnectToServer()
        {
            SetParameter(" \"-mod=", ";\" ", LauncherSettings.Default.Mods);
            SetParameter("-connect=", " ", SelectedServer?.ip);
            SetParameter("-port=", "\n", SelectedServer?.port);

            var startGame = new StartGame
            {
                ShowInTaskbar = false,
                Owner = Application.Current.MainWindow
            };

            startGame.ShowDialog();
        }

        private void SetParameter(string strStart, string strEnd, string newChar)
        {
            ShowOrHideFiles(FileAttributes.Normal, FileAttributes.Normal);

            List<string> settingsIni = new List<string>();

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

            ShowOrHideFiles(FileAttributes.Hidden, FileAttributes.System);
        }

        private StartGame startGame;

        private Server selectedServer;

        public Server SelectedServer
        {
            get => selectedServer;
            set
            {
                selectedServer = value;
                OnPropertyChanged("SelectedServer");
            }
        }

        private ObservableCollection<Server> servers;

        public ObservableCollection<Server> Servers
        {
            get => servers;
            set
            {
                servers = value;
                OnPropertyChanged("Servers");
            }
        }

        private RelayCommand connectToServerCommand;

        public RelayCommand ConnectToServerCommand
        {
            get
            {
                return connectToServerCommand ??= new RelayCommand(obj =>
                {
                    HideSteamId();
                    ConnectToServer();
                });
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
            var roaming = $"{Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)}Roaming\\SmartSteamEmu";

            if (Directory.Exists(roaming))
            {
                var files = Directory.GetFiles(roaming).ToList();
                var hideFiles = files.Where(fileName =>
                    fileName == $"{roaming}!steam_id.ini").ToList();

                foreach (var file in hideFiles)
                {
                    File.SetAttributes(file, FileAttributes.System | FileAttributes.Hidden | FileAttributes.ReadOnly);
                }
            }
        }
    }
}
