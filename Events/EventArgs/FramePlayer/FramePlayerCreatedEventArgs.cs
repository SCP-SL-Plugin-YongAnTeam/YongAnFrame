using Exiled.Events.EventArgs.Interfaces;

namespace YongAnFrame.Events.EventArgs.FramePlayer
{
    /// <summary>
    /// FramePlayer被创建时的事件数据
    /// </summary>
    public sealed class FramePlayerCreatedEventArgs(Players.FramePlayer fPlayer) : IExiledEvent
    {
        public Players.FramePlayer FPlayer { get; } = fPlayer;
    }
}
