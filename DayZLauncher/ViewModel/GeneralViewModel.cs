using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using DayZLauncher.Annotations;
using DayZLauncher.Model;
using DayZLauncher.Utility;

namespace DayZLauncher.ViewModel
{
    class GeneralViewModel : INotifyPropertyChanged
    {
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
                            MessageBox.Show("Нажата вкладка Account!");
                            break;
                        case "NEWS":
                            MessageBox.Show("Нажата вкладка News!");
                            break;
                        case "SERVERS":
                            MessageBox.Show("Нажата вкладка Servers!");
                            break;
                        case "MODS":
                            MessageBox.Show("Нажата вкладка Mods!");
                            break;
                        case "PARAMETERS":
                            MessageBox.Show("Нажата вкладка Parameters!");
                            break;
                    }
                });
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
