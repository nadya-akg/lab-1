using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace NotebookConsole
{
    /// <summary>
    /// Класс для работы с сохранением и считыванием данных из файла
    /// </summary>
    public class DataManager
    {
        private readonly string dataFolder = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location));

        private const string NOTEBOOK_FILE_NAME = "notebook.xml";

        private string notebookItemsFilePath;

        public DataManager()
        {
            notebookItemsFilePath = Path.Combine(dataFolder, NOTEBOOK_FILE_NAME);
        }

        /// <summary>
        /// Загрузка информации об записях из файла
        /// </summary>
        public List<NotebookItem> LoadNotebookItems()
        {
            try
            {
                if (!File.Exists(notebookItemsFilePath)) return new List<NotebookItem>();

                DataContractSerializer dcs = new DataContractSerializer(typeof(List<NotebookItem>));
                using (FileStream fs = new FileStream(notebookItemsFilePath, FileMode.Open))
                {
                    using (XmlDictionaryReader reader = XmlDictionaryReader.CreateTextReader(fs, new XmlDictionaryReaderQuotas()))
                    {
                        return (List<NotebookItem>)dcs.ReadObject(reader);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Не удалось считать данные из файла: {e.Message}");
                return new List<NotebookItem>();
            }
        }

        /// <summary>
        /// Сохранение данных в файл списка записей
        /// </summary>
        public void SaveNotebookItems(List<NotebookItem> notebookItems)
        {
            try
            {
                DataContractSerializer dcs = new DataContractSerializer(typeof(List<NotebookItem>));

                using (Stream stream = new FileStream(notebookItemsFilePath, FileMode.Create, FileAccess.Write))
                {
                    using (XmlDictionaryWriter writer = XmlDictionaryWriter.CreateTextWriter(stream, Encoding.UTF8))
                    {
                        writer.WriteStartDocument();
                        dcs.WriteObject(writer, notebookItems);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine($"Не удалось сохранить данные в файл: {e.Message}");
            }
        }
    }
}
