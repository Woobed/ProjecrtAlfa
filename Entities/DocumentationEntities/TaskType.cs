namespace ProjectAlfa.Entities.DocumentationEntities
{
    public class TaskType
    {
        public int TypeCode { get; set; }

        public string? TypeName { get; set; }
        public string? Manual { get; set; }
        public string? ExecutionScenario { get; set; }
        public string? EvaluationGuideline { get; set; }
        public string? EvaluationRule { get; set;}

    }
}
