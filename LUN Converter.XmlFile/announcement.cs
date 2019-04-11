using System.Collections.Generic;
using System.Xml.Serialization;

namespace LUN_Converter.XmlFile
{
    /// <summary>
    /// Класс с описанием объявлений
    /// </summary>
    public class announcement
    {
        #region Fields
        /// <summary>
        /// Время добавления объявления
        /// </summary>
        public string add_time;
        /// <summary>
        /// Время обновления объявления в формате ISO_8601 (Обязательный параметр)
        /// </summary>
        public string update_time;
        /// <summary>
        /// Тип сделки (Обязательный параметр)
        /// </summary>
        public string contract_type;
        /// <summary>
        /// Тип недвижимости (Обязательный параметр)
        /// </summary>
        public string realty_type;
        /// <summary>
        /// Область
        /// </summary>
        public string region;
        /// <summary>
        /// Район области
        /// </summary>
        public string rajon;
        /// <summary>
        /// Населённый пункт
        /// </summary>
        public string city;
        /// <summary>
        /// Административный район города
        /// </summary>
        public string district;
        /// <summary>
        /// Улица
        /// </summary>
        public string street;
        /// <summary>
        /// Номер дома
        /// </summary>
        public string house;
        /// <summary>
        /// Количество комнат
        /// </summary>
        public int room_count;
        /// <summary>
        /// Тип комнат
        /// </summary>
        public string room_type;
        /// <summary>
        /// Этаж
        /// </summary>
        public int floor;
        /// <summary>
        /// Этажность дома
        /// </summary>
        public int floor_count;
        /// <summary>
        /// общая площадь, кв. м.
        /// </summary>
        public double total_area;
        /// <summary>
        /// Жилая площадь, кв. м.
        /// </summary>
        public double living_area;
        /// <summary>
        /// Площадь участка, сотка
        /// </summary>
        public double land_area;
        /// <summary>
        /// Площадь кухни, кв. м.
        /// </summary>
        public double kitchen_area;
        /// <summary>
        /// Общая цена (Обязательный параметр)
        /// </summary>
        public int price;
        /// <summary>
        /// Валюта (Обязательный параметр)
        /// </summary>
        public string currency;
        /// <summary>
        /// Тип стен
        /// </summary>
        public string wall_type;
        /// <summary>
        /// Наличие балкона
        /// </summary>
        public bool has_balcony;
        /// <summary>
        /// Текст объявления
        /// </summary>
        public string text;
        /// <summary>
        /// Контактные телефоны (Обязательный параметр)
        /// </summary>
        public string phones;
        /// <summary>
        /// Имя физ./юр. лица, которому принадлежат контактные данные
        /// </summary>
        public string contact_name;
        /// <summary>
        /// Внутренние код объекта агентства недвижимости
        /// </summary>
        public string agency_code;
        /// <summary>
        /// Адрес страницы с объявлением на вашем сайте
        /// </summary>
        public string url;
        /// <summary>
        /// Блок-контейнер для ссылок на изображения
        /// </summary>
        [XmlArrayItem("image")]
        public List<string> images;
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для инициализации переменных
        /// </summary>
        public announcement()
        {
            images = new List<string>();
            city = "Харьков";
            region = "Харьковская область";
            rajon = "Харьковский район";

            contact_name = "АН Аверс";
        }
        #endregion
    }
}