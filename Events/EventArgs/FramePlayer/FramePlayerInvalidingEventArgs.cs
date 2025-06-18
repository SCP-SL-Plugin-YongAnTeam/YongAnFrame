using Exiled.Events.EventArgs.Interfaces;

namespace YongAnFrame.Events.EventArgs.FramePlayer
{
    /// <summary>
    /// FramePlayer被无效时的事件数据
    /// </summary>
    public sealed class FramePlayerInvalidatingEventArgs(Features.Players.FramePlayer fPlayer) : IExiledEvent
    {
        /// <summary>
        /// 获取框架玩家
        /// </summary>
        public Features.Players.FramePlayer FPlayer { get; } = fPlayer;
    }
}
