using DayZLauncher.View;
using DayZLauncher.ViewModel;
using System.Windows;

namespace DayZLauncher
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            General app = new General();
            GeneralViewModel context = new GeneralViewModel();
            app.DataContext = context;
            app.Show();
        }
    }
}
