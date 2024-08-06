using Exiled.Events.EventArgs.Interfaces;

namespace YongAnFrame.Events.EventArgs.FramePlayer
{
    public class CreateFramePlayerEventArgs : IExiledEvent
    {
        public Core.Data.FramePlayer FPlayer { get; }

        public CreateFramePlayerEventArgs(Core.Data.FramePlayer fPlayer)
        {
            FPlayer = fPlayer;
        }
    }
}
