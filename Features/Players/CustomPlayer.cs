using Exiled.API.Features;
using YongAnFrame.Features.Players.Interfaces;
using YongAnFrame.Features.Roles;

namespace YongAnFrame.Features.Players
{
    /// <summary>
    /// 自定义玩家
    /// </summary>
    /// <remarks>
    /// 如果有自定义玩家需要挂载到<seealso cref="Player"/>的需求，请继承<seealso cref="CustomPlayer"/>从而自动处理挂载逻辑
    /// <br/>
    /// 但是目前暂无实质性功能
    /// </remarks>
    /// <param name="player">框架玩家</param>
    public abstract class CustomPlayer(FramePlayer player)
    {
        /// <summary>
        /// 获取拥有该实例的<seealso cref="Players.FramePlayer"/>
        /// </summary>
        public FramePlayer FramePlayer { get; private set; } = player;
        ///<inheritdoc cref="FramePlayer.ExPlayer"/>
        public Player ExPlayer => FramePlayer.ExPlayer;
        /// <summary>
        /// 获取<seealso cref="FramePlayer"/>是否无效
        /// </summary>
        public bool IsInvalid => FramePlayer is null;
        ///<inheritdoc cref="FramePlayer.CustomRolePlus"/>
        public CustomRolePlus? CustomRolePlus => FramePlayer.CustomRolePlus;
        ///<inheritdoc cref="FramePlayer.UI"/>
        public PlayerUI UI => FramePlayer.UI;
        ///<inheritdoc cref="FramePlayer.CustomAlgorithm"/>
        public ICustomAlgorithm CustomAlgorithm { get => FramePlayer.CustomAlgorithm; set => FramePlayer.CustomAlgorithm = value; }
        ///<inheritdoc cref="FramePlayer.Level"/>
        public ulong Level { get => FramePlayer.Level; set => FramePlayer.Level = value; }
        ///<inheritdoc cref="FramePlayer.Exp"/>
        public ulong Exp { get => FramePlayer.Exp; set => FramePlayer.Exp = value; }
        ///<inheritdoc cref="FramePlayer.ExpMultiplier"/>
        public float ExpMultiplier { get => FramePlayer.ExpMultiplier; set => FramePlayer.ExpMultiplier = value; }
        ///<inheritdoc cref="FramePlayer.IsBDNT"/>
        public bool IsBDNT { get => FramePlayer.IsBDNT; set => FramePlayer.IsBDNT = value; }
        ///<inheritdoc cref="FramePlayer.UsingTitles"/>
        public PlayerTitle? UsingTitles { get => FramePlayer.UsingTitles; set => FramePlayer.UsingTitles = value; }
        ///<inheritdoc cref="FramePlayer.UsingRankTitles"/>
        public PlayerTitle? UsingRankTitles { get => FramePlayer.UsingRankTitles; set => FramePlayer.UsingRankTitles = value; }
        ///<inheritdoc cref="FramePlayer.AddExp(ulong, string)"/>
        public void AddExp(ulong exp, string name = "未知原因") => FramePlayer.AddExp(exp, name);
        ///<inheritdoc cref="FramePlayer.UpdateShowInfo()"/>
        public void UpdateShowInfo() => FramePlayer.UpdateShowInfo();
        ///<inheritdoc cref="FramePlayer.Invalid()"/>
        public abstract void Invalid();

        /// <summary>
        /// 隐性转换
        /// </summary>
        /// <param name="yPlayer">自定义玩家</param>
        public static implicit operator FramePlayer(CustomPlayer yPlayer) => yPlayer.FramePlayer;
    }
}
