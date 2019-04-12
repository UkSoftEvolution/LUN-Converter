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
                    Parser parser = new Parser();
                    var value = parser.XML(dataFile.Key, cont.Split('\t'));

                    if (value.Item1)
                        pageLUN.announcements.Add(value.Item2);

                    ProgressValue++;
                }
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