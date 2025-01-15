namespace ProjectAlfa.Entities.DocumentationEntities
{
    public class Competency
    {
        
        public int CompetencyCode { get; set; }
        public int ProfileCode { get; set; }


        public string? Wording { get; set; }
        public string? CompetencyCipher { get; set; }
        //???????
        public string? CompetencyType { get; set; }

        public Profile? Profile { get; set; }

    }
}
