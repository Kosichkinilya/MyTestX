using EntranceExamRssk.WPF.Pages;
using System.Windows;

namespace EntranceExamRssk.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            pageContainer.NavigationService.Navigate(new MainMenuPage());
        }
    }
}
