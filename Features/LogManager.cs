using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace YongAnFrame.Features
{
    public static class LogManager
    {
        private static FileStream fs;
        private static readonly Queue<InfoData> logQueue = new();
        private static readonly Task logTask = new(async() =>
        {
            while (true)
            {
                while (logQueue.Count != 0)
                {
                    InfoData infoData = logQueue.Dequeue();
                    string path = $"{PathManager.Log}/{(infoData.Class is null? "Server": infoData.Class.Name)}";
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

        public static void StartTask()
        {
            if (logTask.Status == TaskStatus.Created)
            {
                logTask.Start();
            }
        }

        public static void Info(string log)
        {
            Log.Info(log);
            SaveLog(log, MethodBase.GetCurrentMethod().ReflectedType);
        }

        public static void SaveLog(string log,Type type = null)
        {
            logQueue.Enqueue(new InfoData(log, type));
        }

        public struct InfoData(string content, Type type)
        {
            public string Content { get; } = content;
            public Type Class { get; } = type;
            /// <inheritdoc/>
            public override readonly string ToString() => Content;
            public static implicit operator string(InfoData data) => data.ToString();
        }
    }
}
