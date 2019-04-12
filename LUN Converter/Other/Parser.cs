using LUN_Converter.XmlFile;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;

namespace LUN_Converter.Other
{
    /// <summary>
    /// Класс для парсинга данных файла
    /// </summary>
    public class Parser
    {
        #region Fields
        private announcement ann; //Класс с описанием объявлений
        #endregion

        #region Constructors
        /// <summary>
        /// Конуструктор для инициализации переменных
        /// </summary>
        public Parser()
        {
            ann = new announcement()
            {
                region = "Харьковская область",
                contact_name = "АН Аверс",
                contract_type = "Продажа",
                currency = "у.е."
            };
        }
        #endregion

        #region Function
        /// <summary>
        /// Функция для парсинга данных
        /// </summary>
        /// <param name="FileName">Имя файла</param>
        /// <param name="Data">Данные для парсинга</param>
        /// <returns>Успешность парсинга данных. Данные</returns>
        public (bool, announcement) XML(string FileName, string[] Data)
        {
            //Определение действий в зависимости от имени файла с которого взяты данные
            switch (FileName)
            {
                case "kv.txt":
                    {
                        if (Flat(Data))
                            return (true, ann);
                        else
                            return (false, null);
                    }
                case "dom.txt":
                    {
                        if (House(Data))
                            return (true, ann);
                        else
                            return (false, null);
                    }
                default:
                    { return (false, null); }
            }
        }
        /// <summary>
        /// Функция для сбора данных для квартир
        /// </summary>
        /// <param name="Data">Данные для парсинга</param>
        /// <returns>Возвращает результат сбора данных</returns>
        private bool Flat(string[] Data)
        {
            try
            {
                ann.url = $"{Properties.Settings.Default.Url_kv}{Data[0]}.htm"; //Получили адресс страницы на сайте
                ann.agency_code = Data[0]; //Получаем внутренний код объекта
                ann.price = Price(Data[2]); //Получаем цену
                ann.street = Data[5]; //Получаем улицу
                ann.room_count = Convert.ToInt16(Data[1][0].ToString()); //Получаем количество комнат
                ann.floor = Convert.ToInt16(Data[6]); //Получаем этаж
                ann.floor_count = Convert.ToInt16(Data[7]); //Получаем этажность дома
                ann.total_area = Convert.ToDouble(Data[8]); //Получаем общую площадь, кв. м.
                ann.living_area = Convert.ToDouble(Data[9]); //Получаем жилую площадь, кв. м.
                ann.kitchen_area = Convert.ToDouble(Data[10]); //Получаем площадь кухни, кв. м.
                ann.room_type = Data[11]; //Получаем тип комнат
                ann.wall_type = WallType(Data[12]); //Получаем тип стен
                ann.phones = Phones(Data, 17, 21); //Получаем номера телефонов
                ann.add_time = DateISO(Data[21]); //Получаем дату добавления
                ann.update_time = DateISO(Data[22]); //Получаем дату обновления
                ann.title = Title(Data[23]); //Получаем заголовок
                ann.text = $"{Data[0]} {Data[23]}"; //Получаем текст объявления
                ann.images = new List<string>(Images(Data, 29)); //Получаем изображения

                #region Тип недвижимости
                if (Data[1][1].ToString() == "г" || Data[1][1].ToString() == "и" || Data[1][1].ToString() == "п")
                    ann.realty_type = "Комната";
                else
                    ann.realty_type = "Квартира";
                #endregion

                #region Определяем расположение
                var location = Location(Data[3]);
                ann.rajon = location.Item1;
                ann.city = location.Item2;
                ann.district = location.Item3;
                #endregion

                #region Определяем есть ли балкон
                if (Data[15] != "Балк. НЕТ")
                    ann.has_balcony = true;
                #endregion
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Функция для сбора данных для домов
        /// </summary>
        /// <param name="Data">Данные для парсинга</param>
        /// <returns>Возвращает результат сбора данных</returns>
        private bool House(string[] Data)
        {
            try
            {
                ann.url = $"{Properties.Settings.Default.Url_house}{Data[0]}.htm"; //Получили адресс страницы на сайте
                ann.agency_code = Data[0]; //Получаем внутренний код объекта
                ann.price = Price(Data[2]); //Получаем цену
                ann.street = Data[5]; //Получаем улицу
                ann.house = Data[6]; //Номер дома
                ann.total_area = Convert.ToDouble(Data[7]); //Получаем общую площадь, кв. м.
                ann.land_area = Convert.ToDouble(Data[8]); //Получаем площадь участка, сотка
                ann.room_count = Convert.ToInt16(Data[9]); //Получаем количество комнат
                ann.floor_count = Convert.ToInt16(Data[10]); //Получаем этажность дома
                ann.wall_type = WallType(Data[11]); //Получаем тип стен
                ann.phones = Phones(Data, 17, 21); //Получаем номера телефонов
                ann.add_time = DateISO(Data[21]); //Получаем дату добавления
                ann.update_time = DateISO(Data[22]); //Получаем дату обновления
                ann.title = Title(Data[23]); //Получаем заголовок
                ann.text = $"{Data[0]} {Data[23]}"; //Получаем текст объявления
                ann.images = new List<string>(Images(Data, 29)); //Получаем изображения

                #region Тип недвижимости
                switch (Data[1])
                {
                    case "Уч":
                        {
                            ann.realty_type = "Участок";
                            break;
                        }
                    case "Д":
                        {
                            ann.realty_type = "Дом";
                            break;
                        }
                    case "Дача":
                        {
                            ann.realty_type = "Дача";
                            break;
                        }
                    case "Д1/2":
                        {
                            ann.realty_type = "1/2 часть дома";
                            break;
                        }
                    case "Д1/3":
                        {
                            ann.realty_type = "1/3 часть дома";
                            break;
                        }
                    case "Д2/3":
                        {
                            ann.realty_type = "2/3 часть дома";
                            break;
                        }
                }
                #endregion

                #region Определяем расположение
                var location = Location(Data[3]);
                ann.rajon = location.Item1;
                ann.city = location.Item2;
                ann.district = location.Item3;
                #endregion
            }
            catch
            {
                return false;
            }

            return true;
        }
        /// <summary>
        /// Функция для определения заголовка
        /// </summary>
        /// <param name="Value">Значение из которого нужно достать заголовок</param>
        /// <returns>Возвращает заголовок</returns>
        private string Title(string Value)
        {
            string[] title = Value.Split('.');
            string valueTitle = $"{title[0]}."; //Получаем заголовок до первой точки

            //Проверяем есть ли в конце упоминание сокращения слова "улица"
            if (valueTitle[valueTitle.Length - 3] == 'у' && valueTitle[valueTitle.Length - 2] == 'л')
                return $"{valueTitle}{title[1]}.";
            else
                return valueTitle;
        }
        /// <summary>
        /// Функция для определения типа стен
        /// </summary>
        /// <param name="Value">Значение по которому нужно определить тип стен</param>
        /// <returns>Тип стен</returns>
        private string WallType(string Value)
        {
            switch (Value)
            {
                case "кир.":
                    {
                        return "Кирпич";
                    }
                case "пан.":
                    {
                        return "Панель";
                    }
                case "монол":
                    {
                        return "Монолит";
                    }
                case "блоч.":
                    {
                        return "Блочный";
                    }
                default:
                    {
                        return "";
                    }
            }
        }
        /// <summary>
        /// Функция для определения цены
        /// </summary>
        /// <param name="Value">Значение по которому определяеться цена</param>
        /// <returns>Цена</returns>
        private int Price(string Value)
        {
            string[] price = Value.Split('.');
            if (price.Length == 1)
                return Convert.ToInt32(price[0]) * 1000;
            else
            {
                switch (price[1].Length)
                {
                    case 1:
                        {
                            return Convert.ToInt32($"{price[0]}{price[1]}00");
                        }
                    case 2:
                        {
                            return Convert.ToInt32($"{price[0]}{price[1]}0");
                        }
                    case 3:
                        {
                            return Convert.ToInt32($"{price[0]}{price[1]}");
                        }
                    default:
                        {
                            return 0;
                        }
                }
            }
        }
        /// <summary>
        /// Функция для нахождения номеров телефоно
        /// </summary>
        /// <param name="Data">Данные где искать</param>
        /// <param name="IndexStart">Индекс начала поиска</param>
        /// <param name="IndexEnd">Индекс конца поиска</param>
        /// <returns>Строка с номерами телефонов</returns>
        private string Phones(string[] Data, int IndexStart, int IndexEnd)
        {
            string phones = "";

            for (int i = IndexStart; i < IndexEnd; i++)
            {
                if (phones.Length == 0)
                    phones = Data[i];
                else
                {
                    if (Data[i].Length == 0)
                        break;
                    else
                        phones += $", {Data[i]}";
                }
            }

            return phones;

        }
        /// <summary>
        /// Функция для определения даты в формате ISO 8601
        /// </summary>
        /// <param name="Value">Значение с которого нужно определить дату</param>
        /// <returns>Дата в формате ISO 8601</returns>
        private string DateISO(string Value)
        {
            string[] date = Value.Split('.');
            DateTime dateTime = new DateTime(Convert.ToInt16(date[2]), Convert.ToInt16(date[1]), Convert.ToInt16(date[0]), 1, 0, 0);
            return dateTime.ToString("yyyy-MM-ddTHH':'mm':'sszzz");
        }
        /// <summary>
        /// Функция для нахождения изображений
        /// </summary>
        /// <param name="Data">Данные где нужно искать</param>
        /// <param name="IndexStart">Индекс начала</param>
        /// <returns>Список изображений</returns>
        private List<string> Images(string[] Data, int IndexStart)
        {
            List<string> images = new List<string>();

            for (int i = 29; i < Data.Length; i++)
            {
                if (Data[i].Length == 0)
                    break;
                else
                {
                    var imageData = Data[i].Split('.');
                    if (imageData[1] == "jpg" || imageData[1] == "jpeg")
                        images.Add($"{Properties.Settings.Default.Url_image}{Data[i]}");
                }
            }

            return images;
        }
        /// <summary>
        /// Функция для определения расположения
        /// </summary>
        /// <param name="Value">Значение по которому нужно определять</param>
        /// <returns>rajon, city, district</returns>
        private (string, string, string) Location(string Value)
        {
            string rajon = null, city = null, district = null;

            using (TextFieldParser fieldParser = new TextFieldParser("Districts.csv"))
            {
                fieldParser.TextFieldType = FieldType.Delimited;
                fieldParser.SetDelimiters(";");
                while (!fieldParser.EndOfData)
                {
                    string[] fields = fieldParser.ReadFields();
                    if (fields[3] == Value)
                    {
                        if (fields[2] == "Харьков")
                        {
                            city = "Харьков";
                            district = fields[1]; //Получаем административный район города
                        }
                        else
                            city = Value;

                        if (fields[2] == "Харьков" && fields[1] == "Харьковский")
                            rajon = "Харьковский район";
                        else
                            rajon = Value;
                    }
                }
            }

            return (rajon, city, district);
        }
        #endregion
    }
}