using EntranceExamRssk.Editor.WPF.Models;
using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace EntranceExamRssk.Editor.WPF.Helpers
{
    public class StorageHelper
    {
        /// <summary>
        /// Загрузить тест из файла
        /// </summary>
        /// <param name="fileName">Имя файла с раширением</param>
        /// <returns>null если файл не найден или поврежден</returns>
        public static Test? LoadTest(string fileName)
        {
            var folderPath = ConfigurationHelper.TestsPath;
            var path = Path.Combine(folderPath, fileName);

            if (!File.Exists(path))
                return null;

            var bytes = File.ReadAllBytes(path);
            bytes = Encoding.Convert(Encoding.BigEndianUnicode, Encoding.UTF8, bytes);
            var data = Encoding.UTF8.GetString(bytes);
            data = HideString(data);
            try
            {
                return JsonConvert.DeserializeObject<Test>(data);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.Message);
                return null;
            }
        }

        /// <summary>
        /// Сохранить тест в файл
        /// </summary>
        public static void SaveTest(Test test)
        {
            var data = JsonConvert.SerializeObject(test);
            data = HideString(data);

            var folderPath = ConfigurationHelper.TestsPath;
            var fileName = test.Name;

            var badChars = Path.GetInvalidFileNameChars();
            foreach (var badChar in badChars)
                fileName = fileName.Replace(badChar.ToString(), string.Empty);

            var path = Path.Combine(folderPath, fileName + ".rssk");

            var bytes = Encoding.Convert(Encoding.UTF8, Encoding.BigEndianUnicode, Encoding.UTF8.GetBytes(data));
            using var stream = File.Open(path, FileMode.Create, FileAccess.Write);
            stream.Write(bytes);
        }

        private static string HideString(string input)
        {
            string key = "rssk";
            StringBuilder sb = new();
            for (int i = 0; i < input.Length; i++)
                sb.Append((char)(input[i] ^ key[(i % key.Length)]));
            var result = sb.ToString();

            return result;
        }
    }
}
