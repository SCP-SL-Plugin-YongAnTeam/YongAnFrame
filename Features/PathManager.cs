using Exiled.API.Features;
using System.IO;

namespace YongAnFrame.Features
{
    /// <summary>
    /// IO路径管理器
    /// </summary>
    public static class PathManager
    {
        /// <summary>
        /// 获取音频路径
        /// </summary>
        public static string Music => $"{Paths.Exiled}/YongAnFrame/{Server.Port}/Music";
        /// <summary>
        /// 获取日志路径
        /// </summary>
        public static string Log => $"{Paths.Exiled}/YongAnFrame/{Server.Port}/Log";

        /// <summary>
        /// 检查路径是否存在
        /// </summary>
        public static void CheckPath()
        {
            if (!Directory.Exists(Music))
            {
                Directory.CreateDirectory(Music);
            }
            if (!Directory.Exists(Log))
            {
                Directory.CreateDirectory(Log);
            }
        }
    }
}
