using YongAnFrame.Features.Players;

namespace YongAnFrame.Features.Roles.Interfaces
{
    public interface ISkillActiveEnd : ISkill
    {
        /// <summary>
        /// 行动结束
        /// </summary>
        /// <param name="yPlayer"></param>
        /// <returns>方法的音乐文件名称</returns>
        string ActiveEnd(FramePlayer yPlayer, byte id);
    }
}
