using EntranceExamRssk.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Timers;
using System.Windows;
using System.Windows.Controls;

namespace EntranceExamRssk.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для TestPage.xaml
    /// </summary>
    public partial class TestPage : Page, INotifyPropertyChanged
    {
        /// <summary>
        /// Модель теста для заполнения на форме
        /// </summary>
        private readonly Test test = new();

        public TestPage(Test test)
        {
            InitializeComponent();
            this.test = test;
            RemixCollection(test.Questions);
            Questions = new List<Question>(test.Questions);
            foreach (var question in Questions)
                RemixCollection(question.Answers);
            Duration = TimeSpan.FromMinutes(test.Duration);
            this.DataContext = this;
            testArea.DataContext = test;
            Timer timer = new() { Interval = 1000};
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            Application.Current.MainWindow.Topmost = true;
        }

        /// <summary>
        /// Список вопросов для студента
        /// </summary>
        public List<Question> Questions { get; }

        /// <summary>
        /// Событие для обновления привязки
        /// </summary>
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// Свойство оставшегося времени на тестирование
        /// </summary>
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Обработчик события тика таймера
        /// </summary>
        private void Timer_Elapsed(object? sender, ElapsedEventArgs e)
        {
            Duration = Duration.Subtract(TimeSpan.FromSeconds(1));
            if (Duration.TotalSeconds <= 0)
            {
                ((Timer)sender).Enabled = false;
                this.Dispatcher.Invoke(() =>
                {
                    testArea.IsEnabled = false;
                    MessageBox.Show("Время вышло!");
                });
            }

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Duration)));
        }

        /// <summary>
        /// Метод для перетасовки вопросов и ответов
        /// </summary>
        private void RemixCollection<T>(Collection<T> collection)
        {
            int remixIterationCount =collection.Count * 10;
            Random rnd = new();

            for (int i = 0; i < remixIterationCount; i++)
            {
                var first = rnd.Next(collection.Count);
                var second = rnd.Next(collection.Count);
                var tmp = collection[first];
                collection[first] = collection[second];
                collection[second] = tmp;
            }
        }


        /// <summary>
        /// Обработчик события нажатия на кнопку Сохранить
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Save_ButtonClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите закончить тестирование?", "Закончить тестирование", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;

            float score = 0;
            var studentQuestions = Questions;
            var testQuestions = test.Questions;

            foreach (var question in testQuestions) 
            {
                var studentQuestion = studentQuestions.FirstOrDefault(p => p.Task == question.Task);
                
                foreach (var answer in question.Answers)
                {
                    var studentAnswer = studentQuestion.Answers.FirstOrDefault(p => p.Content == answer.Content);
                    if (answer.IsRight && studentAnswer.IsRight)
                        score += question.AnswerCost;
                }   
            }
            MessageBox.Show($"Вы набрали {score} из {test.PassScore} возможных баллов.\nВы {(score < test.PassScore ? "не " : "")}прошли минималный порог. Позовите преподавателя для фиксации результата теста", "Результаты теста", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Topmost = true;
            Application.Current.MainWindow.WindowState = WindowState.Maximized;
            Application.Current.MainWindow.WindowStyle = WindowStyle.None;
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Topmost = false;
            Application.Current.MainWindow.WindowState = WindowState.Normal;
            Application.Current.MainWindow.WindowStyle = WindowStyle.ThreeDBorderWindow;
        }

        private void Cancel_ButtonClick(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Вы уверены, что хотите закончить тестирование?\nПрогресс будет потерян.", "Закончить тестирование", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result != MessageBoxResult.Yes)
                return;
            this.NavigationService.GoBack();
        }
    }
}
