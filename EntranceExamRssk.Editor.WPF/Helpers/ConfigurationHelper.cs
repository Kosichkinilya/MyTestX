using System;
using System.Configuration;

namespace EntranceExamRssk.Editor.WPF.Helpers
{
    /// <summary>
    /// Класс упрощенного доступа к конфигурации приложения
    /// </summary>
    public class ConfigurationHelper
    {
        private static readonly Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

        /// <summary>
        /// Получить значение параметра
        /// </summary>
        /// <param name="settingName">имя параметра</param>
        public static string Get(string settingName) 
            => config.AppSettings.Settings[settingName].Value;

        /// <summary>
        /// Установить значение параметра
        /// </summary>
        /// <param name="settingName">имя параметра</param>
        /// <param name="value">значение параметра</param>
        /// <remarks>Конфигурация поддерживает только строковые значения параметров</remarks>
        public static void Set(string settingName, string value)
        {
            config.AppSettings.Settings[settingName].Value = value;
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        /// <summary>
        /// Путь к папке с тестами
        /// </summary>
        public static string TestsPath 
        {
            get
            {
                var path = Get(nameof(TestsPath));
                // Если пустая, то установим текущую директорию
                if (string.IsNullOrWhiteSpace(path))
                {
                    path = Environment.CurrentDirectory;
                    TestsPath = path;
                }
                return path;
            }
            set => Set(nameof(TestsPath), value);
        }
    }
}
