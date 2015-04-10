using System.Text.RegularExpressions;

namespace WebCystrawennol.Utility
{
    public static class Helper
    {
        /// <summary>
        /// Удаляет html теги
        /// </summary>
        /// <param name="value">Ссылка, которую нужно обработать</param>
        /// <returns>Чстый текс без тегов</returns>
        public static string ScrubHtml(this string value)
        {
            var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
            var step2 = Regex.Replace(step1, @"\s{2,}", " ");
            return step2;
        }
        /// <summary>
        /// Оставляет только цифры в string
        /// </summary>
        /// <param name="value">Текст, который нужно обработать</param>
        /// <returns>Числа без текста</returns>
        public static string GetNumsFromStr(this string value)
        {
            var resultString = Regex.Match(value, @"\d+").Value;
            return resultString;
        }
    }
}
