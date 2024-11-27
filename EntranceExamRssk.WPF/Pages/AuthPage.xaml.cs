using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EntranceExamRssk.WPF.Pages
{
    /// <summary>
    /// Класс представления страницы входа в настройки программы вступительные экзамены
    /// </summary>
    public partial class AuthPage : Page
    {
        public AuthPage() => InitializeComponent();
        private void Page_Loaded(object sender, RoutedEventArgs e) => password.Focus();

        /// <summary>
        /// Обработчик клика по кнопке Войти
        /// </summary>
        private void AuthClick(object sender, RoutedEventArgs e)
        {
            if (password.Password == "rssk")
            {
                NavigationService.Navigate(new SettingsPage());
            }
            else
            {
                MessageBox.Show("Неверный пароль!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
