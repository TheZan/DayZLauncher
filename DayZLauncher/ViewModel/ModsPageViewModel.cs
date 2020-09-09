using DayZLauncher.Model;
using DayZLauncher.Navigation;
using System;
using System.Collections.Generic;
using System.Text;

namespace DayZLauncher.ViewModel
{
    public class ModsPageViewModel : BaseViewModel, IPageViewModel
    {
        public ModsPageViewModel()
        {
            Mods = new List<Mod>()
            {
                new Mod(){Name = "@COMMUNITYFRAMEWORK", IsSelectedMod = false},
                new Mod(){Name = "@COMMUNITYONLINETOOLS", IsSelectedMod = false},
                new Mod(){Name = "@DAYZEXPANSION", IsSelectedMod = true},
                new Mod(){Name = "@DAYZEXPANSIONLICENSED", IsSelectedMod = false}
            };
        }

        private List<Mod> mods;

        public List<Mod> Mods
        {
            get => mods;
            set
            {
                mods = value;
                OnPropertyChanged("Mods");
            }
        }
    }
}
