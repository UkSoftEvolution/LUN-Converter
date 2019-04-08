using System.Collections.Generic;

namespace LUN_Converter.XmlFile
{
    /// <summary>
    /// Класс с описанием объявлений
    /// </summary>
    public class announcement
    {
        /// <summary>
        /// Район области
        /// </summary>
        public string rajon;
        /// <summary>
        /// Улица
        /// </summary>
        public string street;
        /// <summary>
        /// Тип комнат
        /// </summary>
        public string room_type;
        /// <summary>
        /// Контактные телефоны (Обязательный параметр)
        /// </summary>
        public string phones;
        /// <summary>
        /// Блок-контейнер для ссылок на изображения
        /// </summary>
        public List<object> images;

        #region Fields
        /// <summary>
        /// Тип сделки (Обязательный параметр)
        /// </summary>
        public string contract_type;
        /// <summary>
        /// Тип недвижимости (Обязательный параметр)
        /// </summary>
        public string realty_type;
        /// <summary>
        /// Область (Обязательный параметр)
        /// </summary>
        public string region;
        /// <summary>
        /// Общая цена (Обязательный параметр)
        /// </summary>
        public int price;
        /// <summary>
        /// Валюта (Обязательный параметр)
        /// </summary>
        public string currency;
        /// <summary>
        /// Адрес страницы с объявлением на вашем сайте (Обязательный параметр)
        /// </summary>
        public string url;
        /// <summary>
        /// Время обновления объявления в формате ISO_8601 (Обязательный параметр)
        /// </summary>
        public string update_time;
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для инициализации переменных
        /// </summary>
        public announcement() => images = new List<object>();
        #endregion
    }
}