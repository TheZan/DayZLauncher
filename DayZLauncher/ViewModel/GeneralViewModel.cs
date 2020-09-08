using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using DayZLauncher.Model;
using DayZLauncher.Navigation;

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

        private void ChangeViewModel(IPageViewModel viewModel)
        {
            if (!PageViewModels.Contains(viewModel))
            {
                PageViewModels.Add(viewModel);
            }
            else
            {
                CurrentPageViewModel = PageViewModels
                    .FirstOrDefault(vm => vm == viewModel);
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
                new MenuItem(){Item = "ACCOUNT"},
                new MenuItem(){Item = "NEWS"},
                new MenuItem(){Item = "SERVERS"},
                new MenuItem(){Item = "MODS"},
                new MenuItem(){Item = "PARAMETERS"}
            };

            PageViewModels.Add(new AccountPageViewModel());
            PageViewModels.Add(new NewsPageViewModel());
            PageViewModels.Add(new ServersPageViewModel());
            PageViewModels.Add(new ModsPageViewModel());
            PageViewModels.Add(new ParametersPageViewModel());

            CurrentPageViewModel = PageViewModels[0];
        }
    }
}
