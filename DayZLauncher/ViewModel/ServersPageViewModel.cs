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
                    startGame = new StartGame()
                    {
                        ShowInTaskbar = false,
                        Owner = Application.Current.MainWindow
                    };
                    startGame.ShowDialog();
                });
            }
        }
    }
}
