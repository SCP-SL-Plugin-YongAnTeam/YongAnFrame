namespace YongAnFrame.Players
{
    public abstract class CustomPlayer(FramePlayer player)
    {
        public FramePlayer FramePlayer { get; private set; } = player;
        public bool IsInvalid => FramePlayer == null;
        public float ExpMultiplier { get { return FramePlayer.ExpMultiplier; } set { FramePlayer.ExpMultiplier = value; } }
        public ulong Exp { get { return FramePlayer.Exp; } set { FramePlayer.Exp = value; } }
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
