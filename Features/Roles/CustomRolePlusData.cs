namespace YongAnFrame.Features.Roles
{
    /// <summary>
    /// <seealso cref="CustomRolePlus"/>的数据
    /// </summary>
    public class CustomRolePlusData
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
