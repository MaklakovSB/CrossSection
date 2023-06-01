using CrossSection.Interfaces;
using System.Windows;

namespace CrossSection.Views
{
    /// <summary>
    /// Логика взаимодействия для MainView.xaml
    /// </summary>
    public partial class MainView : Window, IView
    {
        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="viewModel"></param>
        public MainView(IViewModel viewModel)
        {
            ViewModel = viewModel;
            InitializeComponent();
        }

        #region Имплементация IView

        public IViewModel ViewModel
        {
            get { return DataContext as IViewModel; }
            set { DataContext = value; }
        }

        #endregion
    }
}
