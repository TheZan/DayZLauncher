using DayZLauncher.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net;
using System.Text;
using System.Text.Json;

namespace DayZLauncher.Utility
{
    public class ServersMonitoring
    {
        private static ObservableCollection<string> serversUrl = new ObservableCollection<string>()
        {
            @"https://dayz-servers.com/api/info/275/",
        };

        public static ObservableCollection<Server> GetServersInfo()
        {
            var servers = new ObservableCollection<Server>();

                foreach (var url in serversUrl)
                {
                    using (WebClient wc = new WebClient())
                    {
                        try
                        {
                            var json = wc.DownloadString(url);
                            var temp = JsonSerializer.Deserialize<Server>(json);
                            servers.Add(temp);
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                        }
                    }
                }

            return servers;
        }
    }
}
