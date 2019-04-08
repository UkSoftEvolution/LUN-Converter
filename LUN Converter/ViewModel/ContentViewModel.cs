using LUN_Converter.Files;
using LUN_Converter.Other;
using LUN_Converter.XmlFile;
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
        private bool selectFile; //Доступность кнопки "Выбрать файл"
        private bool convertXML; //Доступность кнопки "Конвертировать в XML"
        private bool saveFile; //Доступность кнопки "Сохранить файл"
        private bool indeterminate; //Индикатор ожидания прогресса
        private int progressValue; //Значение прогресса конвертации
        private int progressMaximum; //Значение максимального значения прогресса
        private Visibility visibil; //Отображение имени файла
        private string nameFile; //Имя файла
        private string[] contentFile; //Массив содержимого файла
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
        private void Convert()
        {
            pageLUN = new page();

            foreach (var cont in contentFile)
            {
                string[] data = cont.Split('\t');

                announcement ann = new announcement();
                for (int index = 0; index < data.Length; index++)
                {
                    switch (index)
                    {
                        case 0: break;
                        case 1: break;
                        case 2: break;
                        case 3:
                            {
                                ann.rajon = data[index];
                                break;
                            }
                        case 4: break;
                        case 5:
                            {
                                ann.street = data[index];
                                break;
                            }
                        case 6: break;
                        case 7: break;
                        case 8: break;
                        case 9: break;
                        case 10: break;
                        case 11:
                            {
                                ann.room_type = data[index];
                                break;
                            }
                        case 12: break;
                        case 13: break;
                        case 14: break;
                        case 15: break;
                        case 16: break;
                        case 17:
                        case 18:
                        case 19:
                        case 20:
                            {
                                if (ann.phones == null)
                                    ann.phones = data[index];
                                else
                                {
                                    if (data[index].Length == 0)
                                        continue;
                                    else
                                        ann.phones += $", {data[index]}";
                                }

                                break;
                            }
                        case 21: break;
                        case 22: break;
                        case 23: break;
                        case 24: break;
                        case 25: break;
                        case 26: break;
                        case 27: break;
                        default:
                            {
                                var imgData = data[index].Split('.');
                                if (imgData.Length == 2)
                                {
                                    if (imgData[1] == "jpg")
                                    {
                                        img i = new img();
                                        i.image = data[index];
                                        object image = data[index];
                                        ann.images.Add(image);
                                    }
                                    else
                                        continue;
                                }
                                else
                                    continue;

                                break;
                            }
                    }
                }
                pageLUN.announcements.Add(ann);

                ProgressValue = pageLUN.announcements.Count;
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
            if (file.OpenFile())
            {
                NameFile = file.Name;
                contentFile = file.Content;

                if (SaveFile)
                    SaveFile = false;
                else
                {
                    Visibil = Visibility.Visible;
                    ConvertXML = true;
                    Indeterminate = true;
                }

                ProgressMaximum = contentFile.Length;
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

                Convert();

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
            if (file.SaveFile(NameFile, new System.Xml.Serialization.XmlSerializer(typeof(page)), pageLUN))
                MessageBox.Show("Файл успешно сохранён", "Сохранение", MessageBoxButton.OK, MessageBoxImage.Information);
        });
        #endregion
    }
}