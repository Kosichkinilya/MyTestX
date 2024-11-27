using EntranceExamRssk.Common.Helpers;
using EntranceExamRssk.Common.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;

namespace EntranceExamRssk.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для SettingsPage.xaml
    /// </summary>
    public partial class SettingsPage : Page
    {
        /// <summary>
        /// Список тестов
        /// </summary>
        private ObservableCollection<Test> tests;

        public SettingsPage()
        {
            InitializeComponent();

            var path = ConfigurationHelper.TestsPath;
            folderPath.Text = path;
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

        #region Header buttons handlers
        private void FindTestsInFolder_ButtonClick(object sender, RoutedEventArgs e)
        {
            var path = folderPath.Text;

            if (!Directory.Exists(path))
            {
                // Отображаем ошибку
                folderPath.BorderBrush = Brushes.Red;
                return;
            }

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
        /// Обработчик клика по кнопке Открыть папку по указанному пути
        /// </summary>
        private void OpenTestsFolder_ButtonClick(object sender, RoutedEventArgs e)
        {
            // Проверяем сущестование папки
            if (Directory.Exists(folderPath.Text))
                // Открываем папку
                System.Diagnostics.Process.Start("explorer.exe", "\"" + folderPath.Text + "\"");
            else
                // Отображаем ошибку
                folderPath.BorderBrush = Brushes.Red;
        }

        private void OpenDirectoryDialog_ButtonClick(object sender, RoutedEventArgs e)
        {
            using var dialog = new FolderBrowserDialog()
            {
                Description = "Укажите папку хранения тестов",
                UseDescriptionForTitle = true,
                SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory)
                + Path.DirectorySeparatorChar,
                ShowNewFolderButton = true
            };
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                folderPath.Text = dialog.SelectedPath + Path.DirectorySeparatorChar;
                ConfigurationHelper.TestsPath = folderPath.Text;
            }
        }
        #endregion

        /// <summary>
        /// Сброс фокуса с контролов по нажатию на форму (для удобства).
        /// </summary>
        private void Page_MouseDown(object sender, MouseButtonEventArgs e)
            => Keyboard.ClearFocus();

        /// <summary>
        /// Обработчик события изменения текста в поле ввода пути к папке
        /// </summary>
        private void FolderPath_TextChanged(object sender, TextChangedEventArgs e)
            => folderPath.BorderBrush = border.BorderBrush; // Убираем ошибку

        private void Page_Loaded(object sender, RoutedEventArgs e) => testsList.Items.Refresh();

        /// <summary>
        /// Обработчик события клика по кнопке Закрыть настройки
        /// </summary>
        private void ClosePage_ButtonClick(object sender, RoutedEventArgs e) 
            => NavigationService.Navigate(new MainMenuPage());
    }
}
