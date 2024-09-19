using YongAnFrame.Players;

namespace YongAnFrame.Roles.Interfaces
{
    public interface ISkillBurialEnd : ISkill
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="yPlayer"></param>
        /// <returns>方法的音乐文件名称</returns>
        string BurialEnd(FramePlayer yPlayer,byte id);
    }
}
