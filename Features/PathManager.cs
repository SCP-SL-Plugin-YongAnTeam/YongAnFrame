using Exiled.API.Features;

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
    }
}
