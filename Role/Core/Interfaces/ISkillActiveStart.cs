using YongAnFrame.Core.Data;

namespace YongAnFrame.Role.Core.Interfaces
{
    public interface ISkillActiveStart : ISkill
    {
        string ActiveStart(FramePlayer yPlayer);
    }
}
