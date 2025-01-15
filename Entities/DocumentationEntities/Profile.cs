namespace ProjectAlfa.Entities.DocumentationEntities
{
    public class Profile 
    {
        public int ProfileCode { get; set; }

        public string? ProfileName { get; set; }
        //??????????????????????
        public int RecruitmentYear { get; set; }
        public int DirectionCode { get; set; }
        public StudyingDirection? Direction { get; set; }
        public ICollection<Competency>? Competencies { get; set; }
    }
}
