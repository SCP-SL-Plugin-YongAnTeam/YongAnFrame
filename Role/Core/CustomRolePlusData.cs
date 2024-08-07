namespace YongAnFrame.Role.Core
{
    /// <summary>
    /// 自定义角色玩家数据
    /// </summary>
    public class CustomRolePlusData
    {
        /// <summary>
        /// 技能管理器
        /// </summary>
        public SkillsManager SkillsManager { get; set; }
        /// <summary>
        /// 是否正常死亡
        /// </summary>
        public bool IsDeatHandling { get; set; }
    }
}
