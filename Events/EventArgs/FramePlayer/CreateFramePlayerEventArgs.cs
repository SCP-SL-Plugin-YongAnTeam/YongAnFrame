using Exiled.Events.EventArgs.Interfaces;

namespace YongAnFrame.Events.EventArgs.FramePlayer
{
    /// <summary>
    /// FramePlayer被创建时的事件数据
    /// </summary>
    public class CreateFramePlayerEventArgs : IExiledEvent
    {
        public Players.FramePlayer FPlayer { get; }

        public CreateFramePlayerEventArgs(Players.FramePlayer fPlayer)
        {
            FPlayer = fPlayer;
        }
    }
}
