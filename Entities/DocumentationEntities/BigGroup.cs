namespace ProjectAlfa.Entities.DocumentationEntities
{
    public class BigGroup
    {
        public int GroupNumber { get; set; }
        public string? Name { get; set; }

        public ICollection<StudyingDirection>? StudyingDirections { get; set; }
    }
}
