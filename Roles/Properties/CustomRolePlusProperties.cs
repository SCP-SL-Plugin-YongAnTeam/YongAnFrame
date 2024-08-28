using YongAnFrame.Roles;

namespace YongAnFrame.Role.Properties
{
    /// <summary>
    /// 自定义角色玩家数据
    /// </summary>
    public class CustomRolePlusProperties
    {
        /// <summary>
        /// 技能管理器
        /// </summary>
        public SkillManager SkillsManager { get; set; }
        /// <summary>
        /// 是否正常死亡
        /// </summary>
        public bool IsDeathHandling { get; set; }
    }
}
