using Exiled.API.Features;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace YongAnFrame.Features
{
    public static class LogManager
    {
        private static readonly Queue<string> logQueue = new();
        private static readonly Task logTask = new(() =>
        {
            while (true)
            {
                while (logQueue.Count != 0)
                {
                    SaveLog(logQueue.Dequeue());
                }
                Thread.Sleep(1000);
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
            logQueue.Enqueue(log);
        }

        public static void SaveLog(string log)
        {
            string path = $"{PathManager.Log}/{MethodBase.GetCurrentMethod().ReflectedType.Name}";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            using FileStream fs = new($"{path}/{DateTime.Now:yyyy-MM-dd}.log", FileMode.OpenOrCreate, FileAccess.Write);
            using StreamWriter writer = new(fs);
            writer.WriteLine(log);
        }
    }
}
