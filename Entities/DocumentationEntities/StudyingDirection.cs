namespace ProjectAlfa.Entities.DocumentationEntities
{
    public class StudyingDirection
    {

        public int DirectionCode { get; set; }
        public string? DirectionName { get; set; }
        //???????????????
        public string? Level {  get; set; }


        public int GroupNumber { get; set; }
        public BigGroup? Group { get; set; }

        public ICollection<Profile>? Profiles { get; set; }
    }
}
