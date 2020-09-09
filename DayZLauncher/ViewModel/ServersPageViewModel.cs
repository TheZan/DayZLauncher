using DayZLauncher.Model;
using DayZLauncher.Navigation;
using DayZLauncher.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DayZLauncher.ViewModel
{
    public class ServersPageViewModel : BaseViewModel, IPageViewModel
    {
        public ServersPageViewModel()
        {
            Servers = ServersMonitoring.GetServersInfo();
            Task.Run(RefreshMonitoring);
        }

        private async void RefreshMonitoring()
        {
            while (true)
            {
                await Task.Run(ServersMonitoring.GetServersInfo);
                await Task.Delay(15000);
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
    }
}
