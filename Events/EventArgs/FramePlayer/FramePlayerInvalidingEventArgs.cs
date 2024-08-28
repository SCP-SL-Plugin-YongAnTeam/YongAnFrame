using Exiled.Events.EventArgs.Interfaces;

namespace YongAnFrame.Events.EventArgs.FramePlayer
{
    /// <summary>
    /// FramePlayer被无效时的事件数据
    /// </summary>
    public class FramePlayerInvalidatingEventArgs : IExiledEvent
    {
        public Players.FramePlayer FPlayer { get; }

        public FramePlayerInvalidatingEventArgs(Players.FramePlayer fPlayer)
        {
            FPlayer = fPlayer;
        }
    }
}
