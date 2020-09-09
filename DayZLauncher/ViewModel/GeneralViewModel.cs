using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DayZLauncher.Model;
using DayZLauncher.Navigation;
using DayZLauncher.Utility;

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
                        case "ACCOUNT":
                            CurrentPageViewModel = PageViewModels[0];
                            break;
                        case "NEWS":
                            CurrentPageViewModel = PageViewModels[1];
                            break;
                        case "SERVERS":
                            CurrentPageViewModel = PageViewModels[2];
                            break;
                        case "MODS":
                            CurrentPageViewModel = PageViewModels[3];
                            break;
                        case "PARAMETERS":
                            CurrentPageViewModel = PageViewModels[4];
                            break;
                    }
                });
            }
        }

        public GeneralViewModel()
        {
            Menu = new List<MenuItem>()
            {
                new MenuItem(){Item = "ACCOUNT", IsEnabled = false},
                new MenuItem(){Item = "NEWS", IsEnabled = false},
                new MenuItem(){Item = "SERVERS", IsEnabled = true},
                new MenuItem(){Item = "MODS", IsEnabled = true},
                new MenuItem(){Item = "PARAMETERS", IsEnabled = true}
            };

            PageViewModels.Add(new AccountPageViewModel());
            PageViewModels.Add(new NewsPageViewModel());
            PageViewModels.Add(new ServersPageViewModel());
            PageViewModels.Add(new ModsPageViewModel());
            PageViewModels.Add(new ParametersPageViewModel());

            CurrentPageViewModel = PageViewModels[2];
        }
    }
}
