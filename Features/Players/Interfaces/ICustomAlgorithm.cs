namespace YongAnFrame.Features.Players.Interfaces
{
    /// <summary>
    /// 自定义算法接口
    /// </summary>
    public interface ICustomAlgorithm
    {
        /// <summary>
        /// 获取升级所需要的经验
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public ulong GetNeedUpLevel(ulong level);
    }
}
