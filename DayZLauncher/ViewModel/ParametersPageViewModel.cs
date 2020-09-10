using DayZLauncher.Navigation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Microsoft.Win32;

namespace DayZLauncher.ViewModel
{
    public class ParametersPageViewModel : BaseViewModel, IPageViewModel
    {
        public ParametersPageViewModel()
        {
            GamePath = LauncherSettings.Default.GamePath;
        }

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
    }
}
