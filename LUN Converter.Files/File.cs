using System.IO;
using System.Windows.Forms;

namespace LUN_Converter.Files
{
    /// <summary>
    /// Класс с методами для работы с файлами
    /// </summary>
    public class File
    {
        #region Fields
        private string name; //Имя файла
        private string path; //Путь к файлу
        private string[] content; //Массив содержимого файла
        #endregion

        #region Function
        /// <summary>
        /// Функция открытия файла
        /// </summary>
        /// <returns>Результат открытия файла</returns>
        public bool OpenFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                //openFileDialog.InitialDirectory = "c:\\";
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    //Get the path of specified file
                    path = openFileDialog.FileName;
                    name = openFileDialog.SafeFileName;

                    //Read the contents of the file into a stream
                    var fileStream = openFileDialog.OpenFile();

                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        var cont = reader.ReadToEnd();
                        content = cont.Split('\n');
                    }

                    return true;
                }
                else
                    return false;
            }
        }
        #endregion

        #region Methods
        /// <summary>
        /// Имя файла
        /// </summary>
        public string Name { get => name; }

        /// <summary>
        /// Путь к файлу
        /// </summary>
        public string Path { get => path; }

        /// <summary>
        /// Содержимое файла
        /// </summary>
        public string[] Content { get => content; }
        #endregion
    }
}