using Exiled.API.Features;
using System;
using YongAnFrame.Features.Players;

namespace YongAnFrame.Extensions
{
    /// <summary>
    /// 扩展方法通用工具类
    /// </summary>
    public static class YongAnExtension
    {
        /// <summary>
        /// <seealso cref="Guid"/>作为种子取随机数
        /// </summary>
        /// <param name="r"></param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int StrictNext(this Random r, int min, int max) => new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0)).Next(min, max);

        /// <summary>
        /// <seealso cref="Player"/>转换为<seealso cref="FramePlayer"/>
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static FramePlayer ToFPlayer(this Player p) => FramePlayer.Get(p);
    }
}
