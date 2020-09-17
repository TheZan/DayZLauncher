using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using DayZLauncher.Navigation;
using DayZLauncher.Utility;

namespace DayZLauncher.ViewModel
{
    class StartGameViewModel : BaseViewModel, IPageViewModel
    {
        public StartGameViewModel()
        {
            checker = new MD5Checker(LauncherSettings.Default.GamePath, "ftp://5.188.158.148");

            checker.Notify += (message, progress) =>
            {
                DownloadStatus = "";
                TaskStatus = message;
                IsIndeterminate = progress;
            };

            checker.DownloadStatus += (current, maximum, progress) =>
            {
                IsIndeterminate = progress;
                ProgressBarMaximum = maximum;
                ProgressBarValue = current;
                DownloadStatus = $"{current}/{maximum} KB";
            };

            TaskStatus = "Начать подключение к серверу?";
            StartButtonVisibility = Visibility.Visible;
            ProgressBarVisibility = Visibility.Collapsed;
        }

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SystemParametersInfo(
            uint uiAction,
            uint uiParam,
            bool pvParam,
            uint fWinIni
        );

        public const int SPI_SETUIEFFECTS = 0x103F;

        public EventHandler FinishCheck;

        private void LaunchGame()
        {
            try
            {
                if (LauncherSettings.Default.CloseSteam)
                {
                    var proc = Process.GetProcesses();
                    foreach (Process process in proc)
                    {
                        if (process.ProcessName == "steam")
                        {
                            process.Kill();
                        }
                    }
                }

                if (LauncherSettings.Default.DisableWindowsEffects)
                {
                    SystemParametersInfo(
                        SPI_SETUIEFFECTS, 0, false, 0);
                }
                else
                {
                    SystemParametersInfo(
                        SPI_SETUIEFFECTS, 0, true, 0);
                }

                if (LauncherSettings.Default.StartMemreduct)
                {
                    if (Directory.Exists(@"C:\Program Files\Mem Reduct"))
                    {
                        Process.Start(@"C:\Program Files\Mem Reduct\memreduct.exe");
                    }
                    else
                    {
                        MessageBox.Show(@"Memreduct по пути C:\Program Files\Mem Reduct не найден.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                Process myProc = new Process();
                myProc.StartInfo.FileName = $"{LauncherSettings.Default.GamePath}!start_game.bat";
                myProc.StartInfo.WorkingDirectory = $"{LauncherSettings.Default.GamePath}";
                myProc.Start();

                FinishCheck?.Invoke(this, EventArgs.Empty);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void Start()
        {
            StartButtonVisibility = Visibility.Collapsed;
            ProgressBarVisibility = Visibility.Visible;

            await Task.Run(checker.StartChecking);
        }

        private MD5Checker checker;

        private bool isIndeterminate;

        public bool IsIndeterminate
        {
            get => isIndeterminate;
            set
            {
                isIndeterminate = value;
                OnPropertyChanged("IsIndeterminate");
            }
        }

        private Visibility progressBarVisibility;

        public Visibility ProgressBarVisibility
        {
            get => progressBarVisibility;
            set
            {
                progressBarVisibility = value;
                OnPropertyChanged("ProgressBarVisibility");
            }
        }

        private Visibility startButtonVisibility;

        public Visibility StartButtonVisibility
        {
            get => startButtonVisibility;
            set
            {
                startButtonVisibility = value;
                OnPropertyChanged("StartButtonVisibility");
            }
        }

        private int progressBarValue;

        public int ProgressBarValue
        {
            get => progressBarValue;
            set
            {
                progressBarValue = value;
                OnPropertyChanged("ProgressBarValue");
            }
        }

        private int progressBarMaximum;

        public int ProgressBarMaximum
        {
            get => progressBarMaximum;
            set
            {
                progressBarMaximum = value;
                OnPropertyChanged("ProgressBarMaximum");
            }
        }

        private string downloadStatus;

        public string DownloadStatus
        {
            get => downloadStatus;
            set
            {
                downloadStatus = value;
                OnPropertyChanged("DownloadStatus");
            }
        }

        private string taskStatus;

        public string TaskStatus
        {
            get => taskStatus;
            set
            {
                taskStatus = value;
                OnPropertyChanged("TaskStatus");
            }
        }

        private RelayCommand startGameCommand;

        public RelayCommand StartGameCommand
        {
            get
            {
                return startGameCommand ??= new RelayCommand(obj =>
                {
                    checker.TaskFinish += () => LaunchGame();

                    Start();
                });
            }
        }
    }
}
