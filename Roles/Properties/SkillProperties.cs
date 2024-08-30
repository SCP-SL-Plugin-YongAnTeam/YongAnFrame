namespace YongAnFrame.Roles.Properties
{
    public struct SkillProperties
    {
        public SkillProperties(string name, string statement, string description, float activeMaxTime, float burialMaxTime, ItemType useItem = ItemType.Coin)
        {
            Name = name;
            Statement = statement;
            Description = description;
            ActiveMaxTime = activeMaxTime;
            BurialMaxTime = burialMaxTime;
            UseItem = useItem;
        }
        public string Name { get; }
        public ItemType UseItem { get; }
        public string Statement { get; }
        public string Description { get; }
        public float ActiveMaxTime { get; }
        public float BurialMaxTime { get; }
    }
}
