using HarmonyLib;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using YongAnFrame.Features;

namespace YongAnFrame.Patch
{
    /// <summary>
    /// 在<seealso cref="ServerConsole"/>添加<seealso cref="ServerConsole.AddLog"/>的补丁
    /// </summary>
    [HarmonyPatch(typeof(ServerConsole), nameof(ServerConsole.AddLog))]
    public static class AddLogPatch
    {
#pragma warning disable IDE0060 // 删除未使用的参数
        private static void Prefix(string q, ConsoleColor color = ConsoleColor.Gray, bool hideFromOutputs = false)
#pragma warning restore IDE0060 // 删除未使用的参数
        {
            StackFrame stack = new StackTrace().GetFrame(3);
            if (stack is not null)
            {
                SaveLog(q, stack.GetMethod().ReflectedType.Name);
                return;
            }
            SaveLog(q);
        }

        private static readonly Queue<InfoData> logQueue = new();
        private static readonly Task logTask = new(async () =>
        {
            while (true)
            {
                while (logQueue.Count != 0)
                {
                    InfoData infoData = logQueue.Dequeue();
                    string path = $"{PathManager.Log}/{(infoData.ClassName is null ? "__unknown__" : infoData.ClassName)}";
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    using StreamWriter writer = new($"{path}/{DateTime.Now:yyyy-MM-dd}.log", true, Encoding.UTF8);
                    writer.WriteLine(infoData);
                }
                await Task.Delay(1000);
            }
        });

        /// <summary>
        /// 启动日志任务
        /// </summary>
        public static void StartTask()
        {
            if (logTask.Status == TaskStatus.Created)
            {
                logTask.Start();
            }
        }

        private static void SaveLog(string log, string? className = null)
        {
            logQueue.Enqueue(new InfoData(log, className));
        }

        private readonly struct InfoData(string content, string? className)
        {
            public string Content { get; } = content;
            public string? ClassName { get; } = className;
            public override readonly string ToString() => Content;
            public static implicit operator string(InfoData data) => data.ToString();
        }
    }
}
