namespace YongAnFrame.Roles.Properties
{
    public readonly struct SkillProperties(string name, string statement, string description, float activeMaxTime, float burialMaxTime, ItemType useItem = ItemType.Coin)
    {
        /// <summary>
        /// 技能名称
        /// </summary>
        public string Name { get; } = name;
        /// <summary>
        /// 使用技能绑定的物品
        /// </summary>
        public ItemType UseItem { get; } = useItem;
        /// <summary>
        /// 发动技能描述
        /// </summary>
        public string Statement { get; } = statement;
        /// <summary>
        /// 技能介绍
        /// </summary>
        public string Description { get; } = description;
        /// <summary>
        /// 最大作用时间
        /// </summary>
        public float ActiveMaxTime { get; } = activeMaxTime;
        /// <summary>
        /// 最大冷却时间
        /// </summary>
        public float BurialMaxTime { get; } = burialMaxTime;
    }
}
