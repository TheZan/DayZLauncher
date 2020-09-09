using System;
using System.Collections.Generic;
using System.Management;
using System.Security.Cryptography;
using System.Text;

namespace DayZLauncher.Utility
{
    public static class ClientInfo
    {
        static ClientInfo()
        {
            HWID = GetHWID();
        }

        public static string SteamId { get; private set; }
        public static string Ip { get; private set; }
        public static string HWID { get; private set; }
        public static string Username { get; private set; }

        private static string GetHWID()
        {
            var mbs = new ManagementObjectSearcher("Select ProcessorId From Win32_processor");
            ManagementObjectCollection mbsList = mbs.Get();
            string id = "";
            foreach (ManagementObject mo in mbsList)
            {
                id = mo["ProcessorId"].ToString();
                break;
            }

            return id;
        }
    }
}
