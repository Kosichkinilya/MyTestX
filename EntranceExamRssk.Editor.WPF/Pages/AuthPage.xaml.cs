using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace EntranceExamRssk.Editor.WPF.Pages
{
    /// <summary>
    /// Класс представления страницы входа в программу редактор тестов
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
                MessageBox.Show("Добро пожаловать!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
                NavigationService.Navigate(new MainMenuPage());
            }
            else
            {
                MessageBox.Show("Неверный пароль!", "Сообщение", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

    }
}
