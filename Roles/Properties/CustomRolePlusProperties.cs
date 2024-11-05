using YongAnFrame.Roles;

namespace YongAnFrame.Roles.Properties
{
    /// <summary>
    /// 自定义角色玩家数据
    /// </summary>
    public class CustomRolePlusProperties
    {
        /// <summary>
        /// 获取或设置自定义角色的技能管理器
        /// </summary>
        public SkillManager[] SkillManagers { get; set; }
        /// <summary>
        /// 获取或设置自定义角色是否正常死亡
        /// </summary>
        public bool IsDeathHandling { get; set; }
    }
}
