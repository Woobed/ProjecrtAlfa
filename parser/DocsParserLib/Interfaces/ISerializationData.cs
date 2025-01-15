namespace ProjectAlfa.parser.DocsParserLib.Interfaces;

/// <summary>
/// Интерфейс для работы с процессами сериализации и десериализации данных
/// </summary>


public interface ISerializationData<T>
{
    /// <summary>
    /// Сериализует данные типа ParsedDataBundle в файл с использованием указанного механизма сериализации
    /// </summary>
    /// <typeparam name="ParsedDataBundle">Тип данных для сериализации</typeparam>
    /// <param name="data">Данные для сериализации</param>
    /// <param name="filePath">Путь к файлу, в который будут сохранены данные</param>
    /// <param name="serializer">Объект, отвечающий за процесс сериализации</param>
    public void SerializeData<T>(T data, string filePath, ISerialization serializer);

    /// <summary>
    /// Десериализует данные из файла в объект типа ParsedDataBundle
    /// </summary>
    /// <typeparam name="ParsedDataBundle">Тип данных для десериализации</typeparam>
    /// <param name="filePath">Путь к файлу, из которого будут загружены данные</param>
    /// <param name="serializer">Объект, отвечающий за процесс десериализации</param>
    /// <returns>Объект типа ParsedDataBundle, восстановленный из файла</returns>
    public T DeserializeData<T>(string filePath, ISerialization serializer);
}
