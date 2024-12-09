namespace ProjectAlfa.parser.DocsParserLib.Exceptions;

public class MainPartNotFoundException : Exception
{
	public MainPartNotFoundException(string message) : base(message) { }
	public MainPartNotFoundException() : base("Основная часть файла не найдена") { }
}