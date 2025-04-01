using Exiled.Events.EventArgs.Interfaces;

namespace YongAnFrame.Events.EventArgs.FramePlayer
{
    /// <summary>
    /// FramePlayer被创建时的事件数据
    /// </summary>
    public sealed class FramePlayerCreatedEventArgs(Features.Players.FramePlayer fPlayer) : IExiledEvent
    {
        public Features.Players.FramePlayer FPlayer { get; } = fPlayer;
    }
}
