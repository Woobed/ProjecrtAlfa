namespace ProjectAlfa.parser.DocsParserLib.Interfaces;

public interface IDataOutput
{
	/// <summary>
	/// Возвращает структуру ParsedDataBundle, содержащую все распарсенные данные из документа
	/// </summary>
	/// <param name="document">Документ, который необходимо распарсить</param>
	/// <returns>Структура ParsedDataBundle с распарсенными данными</returns>
	ParsedDataBundle GetParsedData(Document document);
}