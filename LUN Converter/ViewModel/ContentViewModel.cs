using LUN_Converter.Files;
using LUN_Converter.Other;
using System.Threading;
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
            progressMaximum = 100;
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
                OnPropertyChanged(nameof(progressValue));
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
                
                Visibil = Visibility.Visible;
                ConvertXML = true;
                Indeterminate = true;
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

                foreach (var cont in contentFile)
                {
                    string[] data = cont.Split('\t');
                    ProgressValue++;
                    Thread.Sleep(500);
                }

                SelectFile = true;
                ConvertXML = true;
                ProgressValue = 0;
                Indeterminate = true;
                SaveFile = true;
            });
        });

        /// <summary>
        /// Клик по кнопке "Сохранить файл"
        /// </summary>
        public RelayCommand SaveFile_Click => new RelayCommand(obj =>
        {

        });
        #endregion
    }
}