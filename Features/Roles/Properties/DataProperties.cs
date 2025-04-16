namespace YongAnFrame.Features.Roles.Properties
{
    /// <summary>
    /// 给<seealso cref="CustomRolePlus{T}"/>准备的数据属性
    /// </summary>
    public class DataProperties
    {
        /// <summary>
        /// 获取或设置技能
        /// </summary>
        public Skill[]? Skills { get; internal set; }
        /// <summary>
        /// 获取或设置是否正常死亡
        /// </summary>
        public bool IsDeathHandling { get; set; }
    }
}
