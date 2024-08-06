namespace YongAnFrame.Role.Core
{
    public struct SkillsAttributes
    {
        public SkillsAttributes(string name, string statement, string description, float activeMaxTime, float burialMaxTime)
        {
            Name = name;
            Statement = statement;
            Description = description;
            ActiveMaxTime = activeMaxTime;
            BurialMaxTime = burialMaxTime;
        }
        public string Name { get; }
        public string Statement { get; }
        public string Description { get; }
        public float ActiveMaxTime { get; set; }
        public float BurialMaxTime { get; set; }
    }
}
