using Exiled.Events.EventArgs.Interfaces;

namespace YongAnFrame.Events.EventArgs.FramePlayer
{
    /// <summary>
    /// FramePlayer被无效时的事件数据
    /// </summary>
    public sealed class FramePlayerInvalidatingEventArgs(Features.Players.FramePlayer fPlayer) : IExiledEvent
    {
        public Features.Players.FramePlayer FPlayer { get; } = fPlayer;
    }
}
