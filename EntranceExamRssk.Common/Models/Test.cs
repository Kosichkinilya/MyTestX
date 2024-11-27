using System.Collections.ObjectModel;

namespace EntranceExamRssk.Common.Models
{
    /// <summary>
    /// Класс теста
    /// </summary>
    public sealed class Test
    {
        /// <summary>
        /// Название
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Описание
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Предмет тестирования
        /// </summary>
        public string Subject { get; set; } = null!;

        /// <summary>
        /// Проходной балл
        /// </summary>
        public float PassScore { get; set; } = 1;

        /// <summary>
        /// Продолжительность теста в минутах
        /// </summary>
        public int Duration { get; set; } = 15;

        /// <summary>
        /// Вопросы
        /// </summary>
        public ObservableCollection<Question> Questions { get; set; } = new ObservableCollection<Question>();
    }

    /// <summary>
    /// Класс вопроса
    /// </summary>
    /// <typeparam name="Tanswer">Тип ответа</typeparam>
    public class Question
    {
        /// <summary>
        /// Задание. <br/>
        /// Текст в формате xaml
        /// </summary>
        public string Task { get; set; } = null!;

        /// <summary>
        /// Баллы за выбор верного ответа<br/>
        /// если верных ответов несколько, цена за каждый
        /// </summary>
        public float AnswerCost { get; set; } = 1;

        /// <summary>
        /// Тип вопроса
        /// </summary>
        public int Type { get; set; }

        /// <summary>
        /// Список ответов
        /// </summary>
        public ObservableCollection<Answer> Answers { get; set; } = new ObservableCollection<Answer>();
    }

    /// <summary>
    /// Класс ответа
    /// </summary>
    public class Answer
    {
        /// <summary>
        /// Показатель верности ответа
        /// </summary>
        public bool IsRight { get; set; }

        /// <summary>
        /// Тело вопроса
        /// </summary>
        public object Content { get; set; } = null!;
    }
}
