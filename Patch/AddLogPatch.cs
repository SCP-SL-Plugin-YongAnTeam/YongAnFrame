using HarmonyLib;
using System;
using YongAnFrame.Features;

namespace YongAnFrame.Patch
{
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
    public class AddLogPatch
    {
#pragma warning disable IDE0060 // 删除未使用的参数
        private static void Prefix(string q, ConsoleColor color = ConsoleColor.Gray, bool hideFromOutputs = false)
#pragma warning restore IDE0060 // 删除未使用的参数
        {
            LogManager.SaveLog(q);
        }
    }
}
