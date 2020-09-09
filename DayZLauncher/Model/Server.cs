using System.Text.Json.Serialization;

namespace DayZLauncher.Model
{
    public class Server
    {
        public bool success { get; set; }
        public string name { get; set; }
        public string ip { get; set; }
        public string port { get; set; }
        public string map { get; set; }
        public string online_state { get; set; }
        public string status { get; set; }
        public string players_cur { get; set; }
        public string players_max { get; set; }
        public string players_avg { get; set; }
        public string rating { get; set; }
        public string version { get; set; }
        public string os { get; set; }
        public string country_iso { get; set; }
        public string country_name { get; set; }
        public string country_img_16 { get; set; }
        public string country_img_24 { get; set; }
    }
}
