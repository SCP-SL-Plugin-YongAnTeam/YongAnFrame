using Exiled.Events.EventArgs.Interfaces;

namespace YongAnFrame.Events.EventArgs.FramePlayer
{
    /// <summary>
    /// FramePlayer被无效时的事件数据
    /// </summary>
    public class InvalidFramePlayerEventArgs : IExiledEvent
    {
        public Core.Data.FramePlayer FPlayer { get; }

        public InvalidFramePlayerEventArgs(Core.Data.FramePlayer fPlayer)
        {
            FPlayer = fPlayer;
        }
    }
}
