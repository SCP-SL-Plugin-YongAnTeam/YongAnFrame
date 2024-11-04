using YongAnFrame.Roles;
using static PlayerRoles.Spectating.SpectatableModuleBase;

namespace YongAnFrame.Players
{
    public abstract class CustomPlayer(FramePlayer player)
    {
        /// <summary>
        /// 获取拥有该实例的 <seealso cref="Players.FramePlayer"/>
        /// </summary>
        public FramePlayer FramePlayer { get; private set; } = player;
        ///<inheritdoc cref="FramePlayer.IsInvalid"/>
        public bool IsInvalid => FramePlayer == null;
        ///<inheritdoc cref="FramePlayer.CustomRolePlus"/>
        public CustomRolePlus CustomRolePlus => FramePlayer.CustomRolePlus;
        ///<inheritdoc cref="FramePlayer.HintManager"/>
        public HintManager HintManager => FramePlayer.HintManager;
        ///<inheritdoc cref="FramePlayer.CustomAlgorithm"/>
        public ICustomAlgorithm CustomAlgorithm { get => FramePlayer.CustomAlgorithm; set=> FramePlayer.CustomAlgorithm = value; }
        ///<inheritdoc cref="FramePlayer.Level"/>
        public ulong Level { get => FramePlayer.Level; set => FramePlayer.Level = value; }
        ///<inheritdoc cref="FramePlayer.Exp"/>
        public ulong Exp { get => FramePlayer.Exp; set => FramePlayer.Exp = value; }
        ///<inheritdoc cref="FramePlayer.ExpMultiplier"/>
        public float ExpMultiplier { get => FramePlayer.ExpMultiplier; set => FramePlayer.ExpMultiplier = value; }
        ///<inheritdoc cref="FramePlayer.IsBDNT"/>
        public bool IsBDNT { get => FramePlayer.IsBDNT; set => FramePlayer.IsBDNT = value; }
        ///<inheritdoc cref="FramePlayer.UsingTitles"/>
        public PlayerTitle UsingTitles { get => FramePlayer.UsingTitles; set => FramePlayer.UsingTitles = value; }
        ///<inheritdoc cref="FramePlayer.UsingRankTitles"/>
        public PlayerTitle UsingRankTitles { get => FramePlayer.UsingRankTitles; set => FramePlayer.UsingRankTitles = value; }

        ///<inheritdoc cref="FramePlayer.AddExp(ulong, string)"/>
        public void AddExp(ulong exp, string name = "未知原因") => FramePlayer.AddExp(exp, name);

        public void UpdateShowInfoList() => FramePlayer.UpdateShowInfo();

        ///<inheritdoc cref="FramePlayer.Invalid()"/>
        public virtual void Invalid()
        {
            FramePlayer = null;
        }
    }
}
