namespace ProjectAlfa.Entities.DocumentationEntities
{
    public class VariantSelection
    {
        public int VariantCode { get; set; }
        public int TaskCode { get; set; }

        public string? Value { get; set; }
        public bool IsTrue { get; set; }
    }
}
