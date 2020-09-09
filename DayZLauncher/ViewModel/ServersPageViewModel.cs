using DayZLauncher.Model;
using DayZLauncher.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DayZLauncher.ViewModel
{
    public class ServersPageViewModel : BaseViewModel, IPageViewModel
    {
        public ServersPageViewModel()
        {
            Servers = new List<Server>()
            {
                new Server(){Name = "GGAMES DAYZ 1.08 MODED", Map = "CHERNARUSPLUSGLOOM", Players = "75/75", Status = "ONLINE"},
                new Server(){Name = "GGAMES DAYZ 1.08 MODED", Map = "CHERNARUSPLUSGLOOM", Players = "75/75", Status = "ONLINE"},
                new Server(){Name = "GGAMES DAYZ 1.08 MODED", Map = "CHERNARUSPLUSGLOOM", Players = "75/75", Status = "ONLINE"},
                new Server(){Name = "GGAMES DAYZ 1.08 MODED", Map = "CHERNARUSPLUSGLOOM", Players = "75/75", Status = "ONLINE"},
                new Server(){Name = "GGAMES DAYZ 1.08 MODED", Map = "CHERNARUSPLUSGLOOM", Players = "75/75", Status = "ONLINE"},
                new Server(){Name = "GGAMES DAYZ 1.08 MODED", Map = "CHERNARUSPLUSGLOOM", Players = "75/75", Status = "ONLINE"}
            };
        }

        private List<Server> servers;

        public List<Server> Servers
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
