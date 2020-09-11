using DayZLauncher.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace DayZLauncher.ViewModel
{
    public class ParametersPageViewModel : BaseViewModel, IPageViewModel
    {
        public ParametersPageViewModel()
        {
            GetSettings();
        }

        private List<string> settingsIni;

        private string gamePath;

        public string GamePath
        {
            get => gamePath;
            set
            {
                gamePath = value;
                OnPropertyChanged("GamePath");
            }
        }

        private string profileName;

        public string ProfileName
        {
            get => profileName;
            set
            {
                profileName = value;
                OnPropertyChanged("ProfileName");
            }
        }

        private bool high;

        public bool High
        {
            get => high;
            set
            {
                high = value;
                OnPropertyChanged("High");
            }
        }

        private bool startMemreduct;

        public bool StartMemreduct
        {
            get => startMemreduct;
            set
            {
                startMemreduct = value;
                OnPropertyChanged("StartMemreduct");
            }
        }

        private bool disableWindowsEffects;

        public bool DisableWindowsEffects
        {
            get => disableWindowsEffects;
            set
            {
                disableWindowsEffects = value;
                OnPropertyChanged("DisableWindowsEffects");
            }
        }

        private bool closeSteam;

        public bool CloseSteam
        {
            get => closeSteam;
            set
            {
                closeSteam = value;
                OnPropertyChanged("CloseSteam");
            }
        }

        private string maxMem;

        public string MaxMem
        {
            get => maxMem;
            set
            {
                maxMem = value;
                OnPropertyChanged("MaxMem");
            }
        }

        private string maxVRam;

        public string MaxVRam
        {
            get => maxVRam;
            set
            {
                maxVRam = value;
                OnPropertyChanged("MaxVRam");
            }
        }

        private string cpuCount;

        public string CpuCount
        {
            get => cpuCount;
            set
            {
                cpuCount = value;
                OnPropertyChanged("CpuCount");
            }
        }

        private string exThreads;

        public string ExThreads
        {
            get => exThreads;
            set
            {
                exThreads = value;
                OnPropertyChanged("ExThreads");
            }
        }

        private string sceneComplexity;

        public string SceneComplexity
        {
            get => sceneComplexity;
            set
            {
                sceneComplexity = value;
                OnPropertyChanged("SceneComplexity");
            }
        }

        private string shadowsDistance;

        public string ShadowsDistance
        {
            get => shadowsDistance;
            set
            {
                shadowsDistance = value;
                OnPropertyChanged("ShadowsDistance");
            }
        }

        private string viewDistance;

        public string ViewDistance
        {
            get => viewDistance;
            set
            {
                viewDistance = value;
                OnPropertyChanged("ViewDistance");
            }
        }

        private string preferredObjectViewDistance;

        public string PreferredObjectViewDistance
        {
            get => preferredObjectViewDistance;
            set
            {
                preferredObjectViewDistance = value;
                OnPropertyChanged("PreferredObjectViewDistance");
            }
        }

        private string terrainGrid;

        public string TerrainGrid
        {
            get => terrainGrid;
            set
            {
                terrainGrid = value;
                OnPropertyChanged("TerrainGrid");
            }
        }

        private RelayCommand selectGamePathCommand;
        public RelayCommand SelectGamePathCommand
        {
            get
            {
                return selectGamePathCommand ??= new RelayCommand(obj =>
                {
                    GetGamePath();
                });
            }
        }

        private RelayCommand saveSettingsCommand;
        public RelayCommand SaveSettingsCommand
        {
            get
            {
                return saveSettingsCommand ??= new RelayCommand(obj =>
                {
                    SaveSettings();
                });
            }
        }

        private void GetGamePath()
        {
            OpenFileDialog open = new OpenFileDialog() { Filter = "DayZ_x64.exe |*.exe" };
            if (open.ShowDialog() == true && open.FileName.Contains("DayZ_x64.exe"))
            {
                string path = open.FileName.TrimEnd(new char[]
                    {'D', 'a', 'y', 'Z', '_', 'x', '6', '4', '.', 'e', 'x', 'e'});
                LauncherSettings.Default.GamePath = path;
                LauncherSettings.Default.Save();
                GamePath = path;
            }
            else
            {
                MessageBox.Show("Wrong folder!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveSettings()
        {
            SetParameter("-name=", " ", ProfileName);
            SetParameter("-maxMem=", " ", MaxMem);
            SetParameter("-maxVRAM=", " ", MaxVRam);
            SetParameter("-cpuCount=", " ", CpuCount);
            SetParameter("-exThreads=", " ", ExThreads);
            SetHigh();

            LauncherSettings.Default.ProfileName = ProfileName;
            LauncherSettings.Default.MemMax = MaxMem;
            LauncherSettings.Default.MaxVRam = MaxVRam;
            LauncherSettings.Default.CpuCount = CpuCount;
            LauncherSettings.Default.ExThreads = ExThreads;
            LauncherSettings.Default.High = High;

            LauncherSettings.Default.SceneComplexity = SceneComplexity;
            LauncherSettings.Default.ShadowsDistance = ShadowsDistance;
            LauncherSettings.Default.ViewDistance = ViewDistance;
            LauncherSettings.Default.PreferredObjectViewDistance = PreferredObjectViewDistance;
            LauncherSettings.Default.TerrainGrid = TerrainGrid;
            LauncherSettings.Default.StartMemreduct = StartMemreduct;
            LauncherSettings.Default.DisableWindowsEffects = DisableWindowsEffects;
            LauncherSettings.Default.CloseSteam = CloseSteam;
            LauncherSettings.Default.Save();
        }

        private string GetParameter(string strSource, string strStart, string strEnd)
        {
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                int Start, End;
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return strSource.Substring(Start, End - Start);
            }

            return "";
        }

        private bool GetHigh()
        {
            settingsIni = new List<string>();

            using (StreamReader reader = new StreamReader($"{LauncherSettings.Default.GamePath}!StartGame.ini"))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    settingsIni.Add(line);
                }

                if (settingsIni[3].Contains("-High"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        private void SetHigh()
        {
            settingsIni = new List<string>();

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
                {
                    if (High)
                    {
                        if (!settingsIni[3].Contains("-High"))
                        {
                            string line = settingsIni[3];
                            settingsIni[3] = $"{line} -High";
                        }
                    }
                    else
                    {
                        if (settingsIni[3].Contains("-High"))
                        {
                            string line = settingsIni[3];
                            int Start;
                            Start = line.IndexOf("-High", 0);
                            settingsIni[3] = line.Remove(Start - 1, 6);
                        }
                    }

                    foreach (var line in settingsIni)
                    {
                        writer.WriteLine(line);
                    }
                }

                writer.Close();
            }
        }

        private void SetParameter(string strStart, string strEnd, string newChar)
        {
            settingsIni = new List<string>();

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
                }

                writer.Close();
            }
        }

        private void GetSettings()
        {
            if (LauncherSettings.Default.GamePath != "")
            {
                using (StreamReader reader = new StreamReader($"{LauncherSettings.Default.GamePath}!StartGame.ini"))
                {
                    string input = reader.ReadToEnd();

                    LauncherSettings.Default.ProfileName = GetParameter(input, "-name=", " ");
                    LauncherSettings.Default.MemMax = GetParameter(input, "-maxMem=", " ");
                    LauncherSettings.Default.MaxVRam = GetParameter(input, "-maxVRAM=", " ");
                    LauncherSettings.Default.CpuCount = GetParameter(input, "-cpuCount=", " ");
                    LauncherSettings.Default.ExThreads = GetParameter(input, "-exThreads=", " ");
                    LauncherSettings.Default.High = GetHigh();
                    //LauncherSettings.Default.High = High;
                    //LauncherSettings.Default.SceneComplexity = SceneComplexity;
                    //LauncherSettings.Default.ShadowsDistance = ShadowsDistance;
                    //LauncherSettings.Default.ViewDistance = ViewDistance;
                    //LauncherSettings.Default.PreferredObjectViewDistance = PreferredObjectViewDistance;
                    //LauncherSettings.Default.TerrainGrid = TerrainGrid;
                    LauncherSettings.Default.Save();
                }
            }

            GamePath = LauncherSettings.Default.GamePath;
            ProfileName = LauncherSettings.Default.ProfileName;
            MaxMem = LauncherSettings.Default.MemMax;
            MaxVRam = LauncherSettings.Default.MaxVRam;
            CpuCount = LauncherSettings.Default.CpuCount;
            ExThreads = LauncherSettings.Default.ExThreads;
            High = LauncherSettings.Default.High;
            SceneComplexity = LauncherSettings.Default.SceneComplexity;
            ShadowsDistance = LauncherSettings.Default.ShadowsDistance;
            ViewDistance = LauncherSettings.Default.ViewDistance;
            PreferredObjectViewDistance = LauncherSettings.Default.PreferredObjectViewDistance;
            TerrainGrid = LauncherSettings.Default.TerrainGrid;
            StartMemreduct = LauncherSettings.Default.StartMemreduct;
            DisableWindowsEffects = LauncherSettings.Default.DisableWindowsEffects;
            CloseSteam = LauncherSettings.Default.CloseSteam;
        }
    }
}
