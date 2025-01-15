namespace ProjectAlfa.Entities.DocumentationEntities
{
    public class Task
    {
        public int TaskCode {  get; set; }
        public int TypeCode { get; set; }


        public string? Wording { get; set; }
        public string? CorrectAnswer { get; set; }
        public string? ReferenceAnswer { get; set; }
        public string? Difficulty { get; set; }
        public string? ExecutionTime { get; set; }

    }
}
