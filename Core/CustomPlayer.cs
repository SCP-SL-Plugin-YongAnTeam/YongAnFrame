using YongAnFrame.Core.Data;
using YongAnFrame.Core.Manager;

namespace YongAnFrame.Core
{
    public abstract class CustomPlayer(FramePlayer player)
    {
        public FramePlayer FramePlayer { get; private set; } = player;
        public bool IsInvalid => FramePlayer == null;
        public ulong Level { get { return FramePlayer.Level; } set { FramePlayer.Level = value; } }
        public HintManager HintManager => FramePlayer.HintManager;
        public PlayerTitle UsingTitles { get { return FramePlayer.UsingTitles; } set { FramePlayer.UsingTitles = value; } }
        public PlayerTitle UsingRankTitles { get { return FramePlayer.UsingRankTitles; } set { FramePlayer.UsingRankTitles = value; } }

        public virtual void Invalid()
        {
            FramePlayer = null;
        }
    }
}
