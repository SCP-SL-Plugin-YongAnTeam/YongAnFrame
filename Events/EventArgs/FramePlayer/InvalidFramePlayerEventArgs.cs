using Exiled.Events.EventArgs.Interfaces;

namespace YongAnFrame.Events.EventArgs.FramePlayer
{
    public class InvalidFramePlayerEventArgs : IExiledEvent
    {
        public Core.Data.FramePlayer FPlayer { get; }

        public InvalidFramePlayerEventArgs(Core.Data.FramePlayer fPlayer)
        {
            FPlayer = fPlayer;
        }
    }
}
