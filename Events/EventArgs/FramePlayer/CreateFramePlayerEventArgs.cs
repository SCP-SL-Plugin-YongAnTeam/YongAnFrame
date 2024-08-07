using Exiled.Events.EventArgs.Interfaces;

namespace YongAnFrame.Events.EventArgs.FramePlayer
{
    /// <summary>
    /// FramePlayer被创建时的事件数据
    /// </summary>
    public class CreateFramePlayerEventArgs : IExiledEvent
    {
        public Core.Data.FramePlayer FPlayer { get; }

        public CreateFramePlayerEventArgs(Core.Data.FramePlayer fPlayer)
        {
            FPlayer = fPlayer;
        }
    }
}
