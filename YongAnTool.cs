using Exiled.API.Features;
using Respawning;
using System;
using System.Collections.Generic;
using System.Reflection;
using YongAnFrame.Players;
using YongAnFrame.Roles.Enums;

namespace YongAnFrame
{
    /// <summary>
    /// 扩展方法工具类
    /// </summary>
    public static class YongAnTool
    {
        /// <summary>
        /// <seealso cref="Guid"/>作为种子取随机数
        /// </summary>
        /// <param name="r"></param>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        /// <returns></returns>
        public static int StrictNext(this Random r, int min, int max)
        {
            return new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0)).Next(min, max);
        }

        /// <summary>
        /// <seealso cref="Player"/>转换为<seealso cref="FramePlayer"/>
        /// </summary>
        /// <param name="p"></param>
        /// <returns></returns>
        public static FramePlayer ToFPlayer(this Player p)
        {
            return FramePlayer.Get(p);
        }

        /// <summary>
        /// <seealso cref="SpawnableTeamType"/>转换为<seealso cref="RefreshTeamType"/>
        /// </summary>
        /// <param name="stp"></param>
        /// <returns></returns>
        public static RefreshTeamType ToRefreshTeamType(this SpawnableTeamType stp)
        {
            return stp switch
            {
                SpawnableTeamType.ChaosInsurgency => RefreshTeamType.CI,
                SpawnableTeamType.NineTailedFox => RefreshTeamType.MTF,
                _ => RefreshTeamType.Start,
            };
        }
    }
}
