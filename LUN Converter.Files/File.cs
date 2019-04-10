using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace LUN_Converter.Files
{
    /// <summary>
    /// Класс с методами для работы с файлами
    /// </summary>
    public class File
    {
        #region Function
        /// <summary>
        /// Функция открытия файла
        /// </summary>
        /// <returns>Результат открытия файла</returns>
        /// <returns>Данные файлов</returns>
        public (bool, Dictionary<string, string[]>) OpenFile()
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "txt files (*.txt)|*.txt";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;
                openFileDialog.Multiselect = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();

                    foreach (var filePath in openFileDialog.FileNames)
                    {
                        openFileDialog.FileName = filePath;

                        var fileStream = openFileDialog.OpenFile();

                        using (StreamReader reader = new StreamReader(fileStream))
                        {
                            var cont = reader.ReadToEnd();
                            dictionary.Add(openFileDialog.SafeFileName, cont.Split('\n'));
                        }
                    }                   

                    return (true, dictionary);
                }
                else
                    return (false, null);
            }
        }

        /// <summary>
        /// Сохранение файла
        /// </summary>
        /// <param name="xmlSerializer"></param>
        /// <param name="data"></param>
        /// <returns>Результат открытия файла</returns>
        public bool SaveFile(XmlSerializer xmlSerializer, object data)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.FileName = "avers";
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
    }
}