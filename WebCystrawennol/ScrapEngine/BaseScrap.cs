using System.Collections.Generic;
using WebCystrawennol.Model;

namespace WebCystrawennol.ScrapEngine
{
    /// <summary>
    /// Абстрактный класс, для создания уникальных парсеров под конкретный сайт
    /// </summary>
    public abstract class BaseScrap
    {
        /// <summary>
        /// Абсткартный метод для парсера
        /// </summary>
        /// <param name="url">Ссылка на сайт, который нужно спарсить</param>
        public abstract void GetItems(string url);
        /// <summary>
        /// Контейнер, для хранения ссылок под каждый сайт
        /// </summary>
        public abstract List<string> Urls { get; set; }
        public abstract List<SaveToJson.Device> Stuffs { get; set; } 
    }
}
