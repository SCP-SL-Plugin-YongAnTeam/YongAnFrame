using YongAnFrame.Features.Roles.Properties;

namespace YongAnFrame.Features.Roles.Interfaces
{
    public interface ISkill
    {
        /// <summary>
        /// 技能属性
        /// </summary>
        SkillProperties[] SkillProperties { get; }
    }
}
