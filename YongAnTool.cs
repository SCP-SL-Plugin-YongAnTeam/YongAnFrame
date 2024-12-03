using Exiled.API.Features;
using Respawning;
using System;
using System.Collections.Generic;
using System.Reflection;
using YongAnFrame.Players;
using YongAnFrame.Roles.Enums;

namespace YongAnFrame
{
    public static class YongAnTool
    {
        public static int StrictNext(this Random r, int min, int max)
        {
            return new Random(BitConverter.ToInt32(Guid.NewGuid().ToByteArray(), 0)).Next(min, max);
        }

        public static FramePlayer ToFPlayer(this Player p)
        {
            return FramePlayer.Get(p);
        }

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
