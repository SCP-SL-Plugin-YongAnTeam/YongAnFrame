using YongAnFrame.Features.Roles.Properties;

namespace YongAnFrame.Features.Roles.Interfaces
{
    /// <summary>
    /// 技能接口
    /// </summary>
    /// <remarks>
    /// 所有有技能的自定义角色必须实现此接口
    /// </remarks>
    public interface ISkill
    {
        /// <summary>
        /// 获取技能属性
        /// </summary>
        SkillProperties[] SkillProperties { get; }
    }
}
