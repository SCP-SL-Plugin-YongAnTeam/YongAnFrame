namespace YongAnFrame.Features.Roles.Properties
{
    public readonly struct SkillProperties(string name, string statement, string description, float activeMaxTime, float burialMaxTime, ItemType useItem = ItemType.Coin)
    {
        /// <summary>
        /// 获取技能的名称
        /// </summary>
        public string Name { get; } = name;
        /// <summary>
        /// 获取技能的绑定物品
        /// </summary>
        public ItemType UseItem { get; } = useItem;
        /// <summary>
        /// 获取技能的发动描述
        /// </summary>
        public string Statement { get; } = statement;
        /// <summary>
        /// 获取技能的介绍
        /// </summary>
        public string Description { get; } = description;
        /// <summary>
        /// 获取技能的最大作用时间
        /// </summary>
        public float ActiveMaxTime { get; } = activeMaxTime;
        /// <summary>
        /// 获取技能的最大冷却时间
        /// </summary>
        public float BurialMaxTime { get; } = burialMaxTime;
    }
}
