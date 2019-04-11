using LUN_Converter.Files;
using LUN_Converter.Other;
using LUN_Converter.XmlFile;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;

namespace LUN_Converter.ViewModel
{
    /// <summary>
    /// ViewModel: Для ContentView
    /// </summary>
    public class ContentViewModel : MVVM
    {
        #region Fields
        //Доступность кнопок
        private bool selectFile; //Кнопка "Выбрать файл"
        private bool convertXML; //Кнопка  "Конвертировать в XML"
        private bool saveFile; //Кнопка  "Сохранить файл"
        //Прогресс
        private bool indeterminate; //Индикатор ожидания прогресса
        private int progressValue; //Значение прогресса конвертации
        private int progressMaximum; //Значение максимального значения прогресса

        private string nameFile; //Имя файла
        private Visibility visibil; //Отображение имени файла

        private Dictionary<string, string[]> dataFiles; //Данные с файлов
        private page pageLUN; //Фид для сайта LUN
        #endregion

        #region Constructors
        /// <summary>
        /// Конструктор для инициализации компонентов
        /// </summary>
        public ContentViewModel()
        {
            //Инициализация кнопок
            SelectFile = true;
            ConvertXML = false;
            SaveFile = false;

            Indeterminate = false;
            ProgressValue = 0;
            ProgressMaximum = 1;
            Visibil = Visibility.Collapsed;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Доступность кнопки "Выбрать файл"
        /// </summary>
        public bool SelectFile
        {
            get => selectFile;
            set
            {
                selectFile = value;
                OnPropertyChanged(nameof(selectFile));
            }
        }

        /// <summary>
        /// Доступность кнопки "Конвертировать в XML"
        /// </summary>
        public bool ConvertXML
        {
            get => convertXML;
            set
            {
                convertXML = value;
                OnPropertyChanged(nameof(convertXML));
            }
        }

        /// <summary>
        /// Доступность кнопки "Сохранить файл"
        /// </summary>
        public bool SaveFile
        {
            get => saveFile;
            set
            {
                saveFile = value;
                OnPropertyChanged(nameof(saveFile));
            }
        }

        /// <summary>
        /// Индикатор ожидания прогреса
        /// </summary>
        public bool Indeterminate
        {
            get => indeterminate;
            set
            {
                indeterminate = value;
                OnPropertyChanged(nameof(indeterminate));
            }
        }

        /// <summary>
        /// Значение прогресса конвертации
        /// </summary>
        public int ProgressValue
        {
            get => progressValue;
            set
            {
                progressValue = value;
                OnPropertyChanged(nameof(progressValue));
            }
        }

        /// <summary>
        /// Значение максимального значения прогресса
        /// </summary>
        public int ProgressMaximum
        {
            get => progressMaximum;
            set
            {
                progressMaximum = value;
                OnPropertyChanged(nameof(progressMaximum));
            }
        }

        /// <summary>
        /// Отображение имени файла
        /// </summary>
        public Visibility Visibil
        {
            get => visibil;
            set
            {
                visibil = value;
                OnPropertyChanged(nameof(visibil));
            }
        }

        /// <summary>
        /// Имя файла
        /// </summary>
        public string NameFile
        {
            get => nameFile;
            set
            {
                nameFile = value;
                OnPropertyChanged(nameof(nameFile));
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Функция для конвертирования TXT файла в XML
        /// </summary>
        private void ConvertTo()
        {
            pageLUN = new page();

            foreach (var dataFile in dataFiles)
            {
                foreach (string cont in dataFile.Value)
                {
                    announcement ann = null;
                    bool error = false;

                    try
                    {
                        string[] data = cont.Split('\t');
                        if (dataFile.Key == "kv.txt")
                        {
                            //Инициализируем данные для квартир
                            ann = new announcement()
                            {
                                url = $"http://avers.in.ua/flat/{data[0]}.htm", //Получаем ссылку на объявление
                                contract_type = "Продажа",
                                realty_type = "Квартира",
                                currency = "у.е.",
                                agency_code = data[0], //Получаем код
                                street = data[5], //Получаем улицу
                                room_count = Convert.ToInt16(data[1][0].ToString()), //Получаем количество комнат
                                floor = Convert.ToInt16(data[6]), //Получаем этаж
                                floor_count = Convert.ToInt16(data[7]), //Получаем этажность дома
                                total_area = Convert.ToDouble(data[8]), //Получаем общую площадь, кв. м.
                                living_area = Convert.ToDouble(data[9]), //Получаем жилую площадь, кв. м.
                                kitchen_area = Convert.ToDouble(data[10]), //Получаем площадь кухни, кв. м.
                                room_type = data[11], //Получаем тип комнат
                                text = $"{data[0]} {data[23]}" //Получаем текст объявления
                            };

                            var tt = data[23].Split('.');
                            ann.title = $"{tt[0]}."; //Получаем заголовок
                            if (ann.title[ann.title.Length - 2] == 'л' && ann.title[ann.title.Length - 3] == 'у')
                                ann.title += $"{tt[1]}.";

                            #region Получаем город и район
                            using (TextFieldParser parser = new TextFieldParser("Districts.csv"))
                            {
                                parser.TextFieldType = FieldType.Delimited;
                                parser.SetDelimiters(";");
                                while (!parser.EndOfData)
                                {
                                    string[] fields = parser.ReadFields();
                                    if (fields[3] == data[3])
                                    {
                                        if (fields[2] == "Харьков")
                                        {
                                            ann.city = "Харьков";
                                            ann.district = FileCSV(data[3]); //Получаем административный район города
                                        }
                                        else
                                            ann.city = data[4];

                                        if (fields[2] == "Харьков")
                                            ann.rajon = "Харьковский район";
                                        else
                                            ann.rajon = data[3];
                                    }
                                }
                            }
                            #endregion

                            #region Определяем есть ли балкон
                            if (data[15] != "Балк. НЕТ")
                                ann.has_balcony = true;
                            #endregion

                            #region Получаем тип стен
                            switch (data[12])
                            {
                                case "кир.":
                                    {
                                        ann.wall_type = "Кирпич";
                                        break;
                                    }
                                case "пан.":
                                    {
                                        ann.wall_type = "Панель";
                                        break;
                                    }
                                case "монол":
                                    {
                                        ann.wall_type = "Монолит";
                                        break;
                                    }
                                case "блоч.":
                                    {
                                        ann.wall_type = "Блочный";
                                        break;
                                    }
                            }
                            #endregion
                        }
                        else
                        {
                            //Инициализируем данные для домов
                            ann = new announcement()
                            {
                                url = $"http://avers.in.ua/house/{data[0]}.htm", //Получаем ссылку на объявление
                                contract_type = "Продажа",
                                currency = "у.е.",
                                agency_code = data[0], //Получаем код
                                street = data[5], //Получаем улицу
                                house = data[6], //Номер дома
                                total_area = Convert.ToDouble(data[7]), //Получаем общую площадь, кв. м.
                                land_area = Convert.ToDouble(data[8]), //Получаем площадь участка, сотка
                                room_count = Convert.ToInt16(data[9]), //Получаем количество комнат
                                floor_count = Convert.ToInt16(data[10]), //Получаем этажность дома
                                text = $"{data[0]} {data[23]}" //Получаем текст объявления
                            };

                            var tt = data[23].Split('.');
                            ann.title = $"{tt[0]}."; //Получаем заголовок
                            if (ann.title[ann.title.Length - 2] == 'л' && ann.title[ann.title.Length - 3] == 'у')
                                ann.title += $"{tt[1]}.";

                            #region Получаем город и район
                            using (TextFieldParser parser = new TextFieldParser("Districts.csv"))
                            {
                                parser.TextFieldType = FieldType.Delimited;
                                parser.SetDelimiters(";");
                                while (!parser.EndOfData)
                                {
                                    string[] fields = parser.ReadFields();
                                    if (fields[3] == data[3])
                                    {
                                        if (fields[2] == "Харьков")
                                        {
                                            ann.city = "Харьков";
                                            ann.district = FileCSV(data[3]); //Получаем административный район города
                                        }
                                        else
                                            ann.city = data[3];

                                        if (fields[2] == "Харьков" && fields[1] == "Харьковский")
                                            ann.rajon = "Харьковский район";
                                        else
                                            ann.rajon = data[3];
                                    }
                                }
                            }
                            #endregion

                            #region Тип недвижимости
                            switch (data[1])
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

                            #region Получаем тип стен
                            switch (data[11])
                            {
                                case "кир.":
                                    {
                                        ann.wall_type = "Кирпич";
                                        break;
                                    }
                                case "пан.":
                                    {
                                        ann.wall_type = "Панель";
                                        break;
                                    }
                                case "монол":
                                    {
                                        ann.wall_type = "Монолит";
                                        break;
                                    }
                                case "блоч.":
                                    {
                                        ann.wall_type = "Блочный";
                                        break;
                                    }
                            }
                            #endregion
                        }

                        #region Получаем цену
                        var pr = data[2].Split('.');
                        if (pr.Length == 1)
                            ann.price = Convert.ToInt32(pr[0]) * 1000;
                        else
                        {
                            switch (pr[1].Length)
                            {
                                case 1:
                                    {
                                        ann.price = Convert.ToInt32($"{pr[0]}{pr[1]}00");
                                        break;
                                    }
                                case 2:
                                    {
                                        ann.price = Convert.ToInt32($"{pr[0]}{pr[1]}0");
                                        break;
                                    }
                                case 3:
                                    {
                                        ann.price = Convert.ToInt32($"{pr[0]}{pr[1]}");
                                        break;
                                    }
                            }
                        }
                        #endregion

                        #region Получаем номера телефонов
                        for (int i = 17; i < 21; i++)
                        {
                            if (ann.phones == null)
                                ann.phones = data[i];
                            else
                            {
                                if (data[i].Length == 0)
                                    break;
                                else
                                    ann.phones += $", {data[i]}";
                            }
                        }
                        #endregion

                        #region Получаем дату добавления
                        string[] date = data[21].Split('.');
                        DateTime dateTime = new DateTime(Convert.ToInt16(date[2]), Convert.ToInt16(date[1]), Convert.ToInt16(date[0]));
                        ann.add_time = dateTime.ToString("yyyy-MM-ddTzzz");
                        #endregion

                        #region Получаем дату обновления
                        date = data[22].Split('.');
                        dateTime = new DateTime(Convert.ToInt16(date[2]), Convert.ToInt16(date[1]), Convert.ToInt16(date[0]));
                        ann.update_time = dateTime.ToString("yyyy-MM-ddTzzz");
                        #endregion

                        #region Получаем изображения
                        for (int i = 29; i < data.Length; i++)
                        {
                            if (data[i].Length == 0)
                                break;
                            else
                            {
                                var imgData = data[i].Split('.');
                                if (imgData[1] == "jpg" || imgData[1] == "jpeg")
                                    ann.images.Add($"https://cabinet2015.in.ua/adverts/files/{data[i]}");
                            }
                        }
                        #endregion
                    }
                    catch
                    {
                        error = true;
                    }
                    finally
                    {
                        if (!error)
                            pageLUN.announcements.Add(ann);

                        ProgressValue++;
                    }
                }
            }
        }

        /// <summary>
        /// Нахождение поля district в csv файле
        /// </summary>
        /// <param name="value">Параметр поиска</param>
        /// <returns>Возвращает district</returns>
        public string FileCSV(string value)
        {
            using (TextFieldParser parser = new TextFieldParser("Districts.csv"))
            {
                parser.TextFieldType = FieldType.Delimited;
                parser.SetDelimiters(";");
                while (!parser.EndOfData)
                {
                    //Processing row
                    string[] fields = parser.ReadFields();

                    if (fields[3] == value)
                        return fields[1];
                }

                return "";
            }
        }
        #endregion

        #region Commands
        /// <summary>
        /// Клик по кнопке "Выбрать файл"
        /// </summary>
        public RelayCommand SelectFile_Click => new RelayCommand(obj =>
        {
            File file = new File();
            var openFile = file.OpenFile();

            if (openFile.Item1)
            {
                dataFiles = openFile.Item2;

                NameFile = null;
                foreach (var data in dataFiles)
                {
                    if (NameFile == null)
                        NameFile = data.Key;
                    else
                        NameFile += $", {data.Key}";

                    ProgressMaximum += data.Value.Length;
                }

                if (SaveFile)
                    SaveFile = false;
                else
                {
                    Visibil = Visibility.Visible;
                    ConvertXML = true;
                    Indeterminate = true;
                }
            }
        });

        /// <summary>
        /// Клик по кнопке "Конвертировать в XML"
        /// </summary>
        public RelayCommand ConvertXML_Click => new RelayCommand(obj =>
        {
            Task.Factory.StartNew(() =>
            {
                SelectFile = false;
                ConvertXML = false;
                Indeterminate = false;

                ConvertTo();

                SelectFile = true;
                ConvertXML = true;
                ProgressValue = 0;
                Indeterminate = true;
                SaveFile = true;

                MessageBox.Show("Конвертирование файла завершено", "Конвертация", MessageBoxButton.OK, MessageBoxImage.Information);
            });
        });

        /// <summary>
        /// Клик по кнопке "Сохранить файл"
        /// </summary>
        public RelayCommand SaveFile_Click => new RelayCommand(obj =>
        {
            File file = new File();
            if (file.SaveFile(new System.Xml.Serialization.XmlSerializer(typeof(page)), pageLUN))
                MessageBox.Show("Файл успешно сохранён", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        });
        #endregion
    }
}