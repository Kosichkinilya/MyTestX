using EntranceExamRssk.Common.Helpers;
using EntranceExamRssk.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace EntranceExamRssk.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для MainMenuPage.xaml
    /// </summary>
    public partial class MainMenuPage : Page
    {
        /// <summary>
        /// Список тестов
        /// </summary>
        private ObservableCollection<Test> tests;

        public MainMenuPage()
        {
            InitializeComponent();

            var path = ConfigurationHelper.TestsPath;
            var files = Directory.GetFiles(path).ToList().Where(p => p.EndsWith(".rssk")).ToList();

            tests = new ObservableCollection<Test>();
            foreach (var file in files)
            {
                var test = StorageHelper.LoadTest(file);
                if (test == null)
                    continue;
                tests.Add(test);
            }
            testsList.ItemsSource = tests;
        }
       
        /// <summary>
        /// Сброс фокуса с контролов по нажатию на форму (для удобства).
        /// </summary>
        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
            => Keyboard.ClearFocus();

        
        private void Page_Loaded(object sender, RoutedEventArgs e) => testsList.Items.Refresh();

        /// <summary>
        /// Обработчик события клика по кнопке Настройки
        /// </summary>
        private void ToSettings_ButtonClick(object sender, RoutedEventArgs e) 
            => NavigationService.Navigate(new AuthPage());

        /// <summary>
        /// Обработчик события клика по кнопке Начать тестирование
        /// </summary>
        private void StartTest_ButtonClick(object sender, RoutedEventArgs e)
        {
            var selectedTest = (Test)testsList.SelectedItem;
            NavigationService.Navigate(new TestPage(selectedTest));
        }
    }
}
