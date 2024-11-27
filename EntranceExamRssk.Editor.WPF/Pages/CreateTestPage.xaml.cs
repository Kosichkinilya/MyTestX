using EntranceExamRssk.Common.Helpers;
using EntranceExamRssk.Common.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace EntranceExamRssk.Editor.WPF.Pages
{
    /// <summary>
    /// Логика взаимодействия для CreateTestPage.xaml
    /// </summary>
    public partial class CreateTestPage : Page
    {
        /// <summary>
        /// Модель теста для заполнения на форме
        /// </summary>
        private readonly Test test = new();

        public CreateTestPage(Test test)
        {
            InitializeComponent();
            this.test = test;
            DataContext = test;
        }

        /// <summary>
        /// Обработчик события клика по кнопке Добавить вопрос
        /// </summary>
        private void NewQuestion_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (!float.TryParse(txtSummaryAnswerCost.Text, out float cost))
                cost = 1;
            test.Questions.Add(new Question() { AnswerCost = cost });
        }

        /// <summary>
        /// Обработчик события клика по кнопке Удалить вопрос
        /// </summary>
        private void DeleteQuestion_ButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var question = (Question)btn.DataContext;
            test.Questions.Remove(question);
        }

        /// <summary>
        /// Обработчик события клика по кнопке Добавить вариант ответа
        /// </summary>
        private void NewAnswer_ButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var question = (Question)btn.DataContext;
            var answerType = btn.ToolTip;
            Answer asnwer = answerType switch
            {
                "Один правильный ответ" => new Answer(),
                "Несколько правильных ответов" => new Answer(),
                _ => throw new NotImplementedException()
            };
            question.Answers.Add(asnwer);
        }

        /// <summary>
        /// Обработчик события клика по кнопке Удалить вариант ответа
        /// </summary>
        private void DeleteAnswer_ButtonClick(object sender, RoutedEventArgs e)
        {
            var btn = (Button)sender;
            var answer = (Answer)btn.DataContext;
            var question = (ObservableCollection<Answer>)btn.CommandParameter;
            question.Remove(answer);
        }


        /// <summary>
        /// Обработчик события клика по кнопке Назначить всем ответам
        /// </summary>
        private void SetScore_ButtonClick(object sender, RoutedEventArgs e)
        {
            if (float.TryParse(txtSummaryAnswerCost.Text, out float cost))
            {
                // Устанавливаем всем вопросам одинаковое количество баллов
                foreach (var question in test.Questions)
                    question.AnswerCost = cost;

                // Обновляем привязку
                questionsListView.Items.Refresh();
            }
        }

        /// <summary>
        /// Обработчик события клика по кнопке Сохранить
        /// </summary>
        private void SaveTest_ButtonClick(object sender, RoutedEventArgs e)
        {
            var stringBuilder = new StringBuilder();
            // Проверка заполненности строкоых полей
            if (string.IsNullOrEmpty(test.Name))
                stringBuilder.AppendLine("Укажите название теста");
            if (string.IsNullOrEmpty(test.Subject))
                stringBuilder.AppendLine("Укажите предмет теста");

            // Количество вопросов не должно быть нулевым
            if (test.Questions.Count == 0)
                stringBuilder.AppendLine("Добавьте вопросы");

            // Проверка заполненности вопросов 
            if (test.Questions.Any(q =>
                    string.IsNullOrEmpty(q.Task) || // Должен быть указан текст вопроса
                    q.AnswerCost == 0))             // Стоимость вопроса должна быть не равна нулю
                stringBuilder.AppendLine("Заполните все вопросы");

            // Проверка заполненности ответов
            if (test.Questions.Any(q =>
                    q.Answers.Count == 0 ||                   // Количество ответов не должно быть нулевым
                    q.Answers.Any(a => a.Content == null) ||  // Должен быть указан текст вопроса
                    q.Answers.All(a => a.IsRight == false)))  // Должен быть отмечен хотя бы 1 ответ
                stringBuilder.AppendLine("Заполните все ответы");

            // Проверка продолжительности теста
            if (test.Duration == 0)
                stringBuilder.AppendLine("Укажите время теста в минутах");
                
            // Проверка проходного балла
            if (test.PassScore == 0)
                stringBuilder.AppendLine("Укажите проходной балл");

            if (test.PassScore > test.Questions.Sum(q => q.AnswerCost * q.Answers.Where(a => a.IsRight).Count()))
                stringBuilder.AppendLine("Указанный проходной балл больше, чем максимально возможное количество баллов");

            if (stringBuilder.Length > 0)
            {
                MessageBox.Show(stringBuilder.ToString(), "Ошибка при сохранении", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            StorageHelper.SaveTest(test);
            MessageBox.Show("Сохранено!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            NavigationService.GoBack();
        }

        private void Cancel_ButtonClick(object sender, RoutedEventArgs e) => NavigationService.GoBack();

        /// <summary>
        /// Обработчик события ввода текста в текстбокс с float числом
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FloatTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var tb = (TextBox)sender;
            if (e.Text == ",")
            {
                e.Handled = tb.Text.Contains(',');
                return;
            }
            if(!float.TryParse(e.Text, out float value))
                e.Handled = true;
        }
    }
}
