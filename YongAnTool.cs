﻿using Exiled.API.Features;
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

        public static T Reflection<T, V>(Dictionary<string, V> value)
        {
            Type type = typeof(T);
            object obj = Activator.CreateInstance(type);

            foreach (var item in value)
            {
                FieldInfo variable = type.GetField(item.Key);
                if (variable == null)
                {
                    continue;
                }
                variable.SetValue(obj, Convert.ChangeType(item.Value, variable.FieldType));
            }
            return (T)obj;
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
