using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

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

        public bool SaveFile(string nameFile, XmlSerializer xmlSerializer, object data)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                string[] vs = nameFile.Split('.');
                saveFileDialog.FileName = vs[0];
                saveFileDialog.Filter = "XML Files (*.xml)|*.xml";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    using (Stream stream = new FileStream(saveFileDialog.FileName, FileMode.Create))
                    {
                        xmlSerializer.Serialize(stream, data);
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