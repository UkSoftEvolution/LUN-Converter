using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace LUN_Converter.XmlFile
{
    /// <summary>
    /// Класс для работы с структурой XML файла
    /// </summary>
    public class page
    {
        #region Fields
        /// <summary>
        /// содержит время начала генерации фида в формате ISO_8601 (пример значения - 2017-11-13T11:39:57+03:00)
        /// </summary>
        public string generation_time;
        /// <summary>
        /// Содержит произвольное количество блоков announcement с описанием объявлений
        /// </summary>
        public List<announcement> announcements;
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор по умолчанию для инициализации переменных
        /// </summary>
        public page()
        {
            generation_time = ConvertIOS();
            announcements = new List<announcement>();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Конвертирует дату в формате ISO 8601 (пример значения - 2017-11-13T11:39:57+03:00)
        /// </summary>
        /// <returns>Возвращает дату в виде ISO 8601</returns>
        private string ConvertIOS()
        {
            DateTime dateTime = DateTime.Now;
            return dateTime.ToString("yyyy-MM-ddTHH':'mm':'sszzz");
        }
        #endregion
    }
}