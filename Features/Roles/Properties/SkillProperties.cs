using static YongAnFrame.Features.Roles.Skill;

namespace YongAnFrame.Features.Roles.Properties
{
    /// <summary>
    /// 给<seealso cref="Skill"/>准备的原始技能属性
    /// </summary>
    /// <remarks>
    /// 你无法修改结构体里的任何对象，如果想修改对象请从<seealso cref="Skill"/>对象修改，因为要保留技能的原始属性，从而保证可以恢复到原始属性
    /// </remarks>
    /// <param name="name">名称</param>
    /// <param name="statement">发动描述</param>
    /// <param name="description">介绍</param>
    /// <param name="activeMaxTime">最大作用时间</param>
    /// <param name="burialMaxTime">最大冷却时间</param>
    /// <param name="activeStart">激活开始委托</param>
    /// <param name="activeEnd">激活结束委托</param>
    /// <param name="burialEnd">冷却结束委托</param>
    /// <param name="useItem">绑定物品</param>
    public readonly struct SkillProperties(string name, string statement, string description, float activeMaxTime, float burialMaxTime,
        ActiveStart? activeStart = null, ActiveEnd? activeEnd = null, BurialEnd? burialEnd = null, ItemType useItem = ItemType.Coin)
    {
        /// <summary>
        /// 获取名称
        /// </summary>
        public string Name { get; } = name;
        /// <summary>
        /// 获取绑定物品
        /// </summary>
        public ItemType UseItem { get; } = useItem;
        /// <summary>
        /// 获取发动描述
        /// </summary>
        public string Statement { get; } = statement;
        /// <summary>
        /// 获取介绍
        /// </summary>
        public string Description { get; } = description;
        /// <summary>
        /// 获取最大作用时间
        /// </summary>
        public float ActiveMaxTime { get; } = activeMaxTime;
        /// <summary>
        /// 获取最大冷却时间
        /// </summary>
        public float BurialMaxTime { get; } = burialMaxTime;
        /// <summary>
        /// 获取激活开始委托
        /// </summary>
        public ActiveStart? ActiveStartAction { get; } = activeStart;
        /// <summary>
        /// 获取激活结束委托
        /// </summary>
        public ActiveEnd? ActiveEndAction { get; } = activeEnd;
        /// <summary>
        /// 获取冷却结束委托
        /// </summary>
        public BurialEnd? BurialEndAction { get; } = burialEnd;
    }
}
