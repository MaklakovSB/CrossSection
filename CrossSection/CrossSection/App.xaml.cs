using CrossSection.Interfaces;
using CrossSection.Views;
using CrossSection.ViewModels;
using System.Windows;

namespace CrossSection
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {

        /// <summary>
        /// Точка входа.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var mainView = new MainView(new MainViewModel());
            mainView.Show();
        }
    }
}
