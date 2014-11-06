using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebCystrawennol.Utility
{
    public static class Helper
    {
        /// <summary>
        /// Удаляет html теги
        /// </summary>
        /// <param name="value">Ссылка, которую нужно обработать</param>
        /// <returns>Чстый текс без тегов</returns>
        public static string ScrubHtml(string value)
        {
            var step1 = Regex.Replace(value, @"<[^>]+>|&nbsp;", "").Trim();
            var step2 = Regex.Replace(step1, @"\s{2,}", " ");
            return step2;
        }

        public static string GeNumsFromStr(this string value)
        {
            var resultString = Regex.Match(value, @"\d+").Value;
            return resultString;
        }
    }
}
