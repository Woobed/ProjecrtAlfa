using System.Text.Json;
using System.Xml.Serialization;
using ProjectAlfa.parser.DocsParserLib.Interfaces;


namespace ProjectAlfa.parser.DocsParserLib
{
    //
    // Структура (файл вывода)
    //


    /// <summary>
    /// Структура для хранения всех распарсенных данных, таких как компетенции, вопросы и практические задачи
    /// </summary>
    public struct ParsedDataBundle // Хранение всех распарсенных экземпляров
    {
        /// <summary>
        /// Список распарсенных компетенций
        /// </summary>
        public List<Competention> Competentions { get; set; }

        /// <summary>
        /// Список распарсенных вопросов
        /// </summary>
        public List<Question> Questions { get; set; }

        /// <summary>
        /// Список распарсенных практических задач
        /// </summary>
        public List<PracticTask> PracticTasks { get; set; }
    }

    /// <summary>
    /// Интерфейс для получения распарсенных данных из документа
    /// </summary>
    

    /// <summary>
    /// Реализация интерфейса IDataOutput для получения распарсенных данных
    /// </summary>
    public class DataOutput : IDataOutput // Реализация вывода информации
    {
        /// <summary>
        /// Парсит документ, извлекая компетенции, вопросы и практические задачи
        /// </summary>
        /// <param name="document">Документ для парсинга</param>
        /// <returns>Структура ParsedDataBundle с распарсенной информацией</returns>
        public ParsedDataBundle GetParsedData(Document document)
        {
            CompetentionParser c_parser = new CompetentionParser(document);
            QuestionParser q_parser = new QuestionParser(document, c_parser);
            PracticTasksParser p_parser = new PracticTasksParser(document, c_parser);

            return new ParsedDataBundle
            {
                Competentions = c_parser.Parse(),
                Questions = q_parser.Parse(),
                PracticTasks = p_parser.Parse()
            };
        }
    }



    //
    // Объеденение полученных данных с процессом сериализации (Файл сериализации данных)
    //

    /// <summary>
    /// Реализация интерфейса ISerializationData для объединения процесса сериализации и десериализации
    /// </summary>
    public class SerializationData : ISerializationData<ParsedDataBundle>
    {
        //private ISerializationData _serializationDataImplementation;

        /// <summary>
        /// Выполняет сериализацию данных в файл
        /// </summary>
        /// <typeparam name="T">Тип данных для сериализации</typeparam>
        /// <param name="data">Объект данных</param>
        /// <param name="filePath">Путь к файлу для сохранения</param>
        /// <param name="serializer">Объект для выполнения сериализации</param>
        public void SerializeData<ParsedDataBundle>(ParsedDataBundle data, string filePath, ISerialization serializer)
        {
            serializer.Serialize(data, filePath);
        }

        /// <summary>
        /// Выполняет десериализацию данных из файла
        /// </summary>
        /// <typeparam name="T">Тип данных для десериализации</typeparam>
        /// <param name="filePath">Путь к файлу с данными</param>
        /// <param name="serializer">Объект для выполнения десериализации</param>
        /// <returns>Объект типа <typeparamref name="T"/></returns>
        public ParsedDataBundle DeserializeData<ParsedDataBundle>(string filePath, ISerialization serializer)
        {
            return serializer.Deserialize<ParsedDataBundle>(filePath);
        }

        
    }


    //
    // Сухая сериализация данных (Файл сухой сериализации)
    //

    

    /// <summary>
    /// Класс для выполнения сериализации и десериализации объектов в формате XML
    /// </summary>
    public class SerializeXML : ISerialization
    {
        /// <summary>
        /// Сериализует объект указанного типа в XML-файл по заданному пути
        /// </summary>
        /// <typeparam name="T">Тип объекта, который необходимо сериализовать</typeparam>
        /// <param name="parsObject">Объект для сериализации</param>
        /// <param name="filePath">Путь к файлу, в который будет сохранен сериализованный XML</param>
        public void Serialize<T>(T parsObject, string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, parsObject);
            }
        }

        /// <summary>
        /// Десериализует XML-файл по заданному пути в объект указанного типа
        /// </summary>
        /// <typeparam name="T">Тип объекта, в который необходимо десериализовать данные</typeparam>
        /// <param name="filePath">Путь к XML-файлу, из которого будет загружен объект</param>
        /// <returns>Объект типа <typeparamref name="T"/>, восстановленный из XML</returns>
        public T Deserialize<T>(string filePath)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StreamReader reader = new StreamReader(filePath))
            {
                return (T)serializer.Deserialize(reader);
            }
        }
    }

    public class SerializeJSON : ISerialization
    {
        /// <summary>
        /// Сериализует объект указанного типа в JSON-файл по заданному пути
        /// </summary>
        /// <typeparam name="T">Тип объекта, который необходимо сериализовать</typeparam>
        /// <param name="parsObject">Объект для сериализации</param>
        /// <param name="filePath">Путь к файлу, в который будет сохранен сериализованный JSON</param>
        public void Serialize<T>(T parsObject, string filePath)
        {
            string jsonString = JsonSerializer.Serialize(parsObject);
            File.WriteAllText(filePath, jsonString);
        }

        /// <summary>
        /// Десериализует JSON-файл по заданному пути в объект указанного типа
        /// </summary>
        /// <typeparam name="T">Тип объекта, в который необходимо десериализовать данные</typeparam>
        /// <param name="filePath">Путь к JSON-файлу, из которого будет загружен объект</param>
        /// <returns>Объект типа <typeparamref name="T"/>, восстановленный из JSON</returns>
        public T Deserialize<T>(string filePath)
        {
            string jsonString = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<T>(jsonString);
        }
    }
}