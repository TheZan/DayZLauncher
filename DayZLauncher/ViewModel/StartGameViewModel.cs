using System;
using System.Collections.Generic;
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
                TaskStatus = message;
                IsIndeterminate = progress;
            };

            checker.DownloadStatus += (current, maximum, progress) =>
            {
                IsIndeterminate = progress;
                ProgressBarMaximum = maximum;
                ProgressBarValue = current;
            };

            Start();
        }

        private async void Start()
        {
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
    }
}
