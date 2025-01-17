﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DayZLauncher.Model;
using DayZLauncher.Navigation;
using DayZLauncher.Utility;
using DayZLauncher.View;
using Microsoft.Win32;
using AutoUpdaterDotNET;
using System.Windows.Threading;

namespace DayZLauncher.ViewModel
{
    public class GeneralViewModel : BaseViewModel
    {

        private IPageViewModel _currentPageViewModel;
        private List<IPageViewModel> _pageViewModels;

        public List<IPageViewModel> PageViewModels
        {
            get
            {
                if (_pageViewModels == null)
                    _pageViewModels = new List<IPageViewModel>();

                return _pageViewModels;
            }
        }

        public IPageViewModel CurrentPageViewModel
        {
            get => _currentPageViewModel;
            set
            {
                _currentPageViewModel = value;
                OnPropertyChanged("CurrentPageViewModel");
            }
        }

        private List<MenuItem> menu;

        public List<MenuItem> Menu
        {
            get => menu;
            set
            {
                menu = value;
                OnPropertyChanged("Menu");
            }
        }

        private MenuItem selectedMenuItem;

        public MenuItem SelectedMenuItem
        {
            get => selectedMenuItem;
            set
            {
                selectedMenuItem = value;
                OnPropertyChanged("SelectedMenuItem");
            }
        }

        private RelayCommand goToItemCommand;

        public RelayCommand GoToItemCommand
        {
            get
            {
                return goToItemCommand ??= new RelayCommand(obj =>
                {
                    switch (SelectedMenuItem?.Item)
                    {
                        case "АККАУНТ":
                            CurrentPageViewModel = PageViewModels[0];
                            break;
                        case "НОВОСТИ":
                            CurrentPageViewModel = PageViewModels[1];
                            break;
                        case "СЕРВЕРА":
                            CurrentPageViewModel = PageViewModels[2];
                            break;
                        case "ПАРАМЕТРЫ":
                            CurrentPageViewModel = PageViewModels[3];
                            break;
                    }
                });
            }
        }

        public GeneralViewModel()
        {
            Menu = new List<MenuItem>()
            {
                new MenuItem(){Item = "АККАУНТ", IsEnabled = false},
                new MenuItem(){Item = "НОВОСТИ", IsEnabled = false},
                new MenuItem(){Item = "СЕРВЕРА", IsEnabled = true},
                new MenuItem(){Item = "ПАРАМЕТРЫ", IsEnabled = true}
            };

            PageViewModels.Add(new AccountPageViewModel());
            PageViewModels.Add(new NewsPageViewModel());
            PageViewModels.Add(new ServersPageViewModel());
            PageViewModels.Add(new ParametersPageViewModel());

            CurrentPageViewModel = PageViewModels[2];

            StartLauncher();

            AutoUpdater.Start("ftp://5.188.158.148/Updater64.xml");

            DispatcherTimer timer = new DispatcherTimer { Interval = TimeSpan.FromMinutes(2) };
            timer.Tick += delegate
            {
                AutoUpdater.Start("ftp://5.188.158.148/Updater64.xml");
            };
            timer.Start();
        }

        private void StartLauncher()
        {
            ShowOrHideFiles(FileAttributes.Normal, FileAttributes.Normal);

            if (LauncherSettings.Default.GamePath == "")
            {
                StartPage selectGameFolder = new StartPage();
                if (selectGameFolder.ShowDialog() == false)
                {
                    Application.Current.Shutdown();
                }
            }

            ShowOrHideFiles(FileAttributes.Hidden, FileAttributes.System);
        }

        private void ShowOrHideFiles(FileAttributes attributes, FileAttributes fileAttributes)
        {
            if (LauncherSettings.Default.GamePath != "")
            {
                var files = Directory.GetFiles(LauncherSettings.Default.GamePath).ToList();
                var hideFiles = files.Where(fileName =>
                    fileName == $"{LauncherSettings.Default.GamePath}!start_game.bat" ||
                    fileName == $"{LauncherSettings.Default.GamePath}!StartGame.ini").ToList();
                foreach (var file in hideFiles)
                {
                    File.SetAttributes(file, fileAttributes | attributes);
                }
            }
        }
    }
}
