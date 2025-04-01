using YongAnFrame.Features.Players;

namespace YongAnFrame.Features.Roles.Interfaces
{
    public interface ISkillActiveStart : ISkill
    {
        /// <summary>
        /// 行动开始
        /// </summary>
        /// <param name="yPlayer"></param>
        /// <returns>方法的音乐文件名称</returns>
        string ActiveStart(FramePlayer yPlayer, byte id);
    }
}
