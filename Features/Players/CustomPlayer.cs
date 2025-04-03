using Exiled.API.Features;
using System.Collections.Generic;
using System.Linq;
using YongAnFrame.Features.Players.Interfaces;
using YongAnFrame.Features.Roles;
using YongAnFrame.Features.UIs;

namespace YongAnFrame.Features.Players
{
    public abstract class CustomPlayer(FramePlayer player)
    {
        /// <summary>
        /// 获取拥有该实例的<seealso cref="Players.FramePlayer"/>
        /// </summary>
        public FramePlayer FramePlayer { get; private set; } = player;
        ///<inheritdoc cref="FramePlayer.ExPlayer"/>
        public Player ExPlayer => FramePlayer.ExPlayer;
        ///<inheritdoc cref="FramePlayer.IsInvalid"/>
        public bool IsInvalid => FramePlayer == null;
        ///<inheritdoc cref="FramePlayer.CustomRolePlus"/>
        public CustomRolePlus CustomRolePlus => FramePlayer.CustomRolePlus;
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
        public PlayerTitle UsingTitles { get => FramePlayer.UsingTitles; set => FramePlayer.UsingTitles = value; }
        ///<inheritdoc cref="FramePlayer.UsingRankTitles"/>
        public PlayerTitle UsingRankTitles { get => FramePlayer.UsingRankTitles; set => FramePlayer.UsingRankTitles = value; }
        ///<inheritdoc cref="FramePlayer.AddExp(ulong, string)"/>
        public void AddExp(ulong exp, string name = "未知原因") => FramePlayer.AddExp(exp, name);
        public void UpdateShowInfo() => FramePlayer.UpdateShowInfo();
        ///<inheritdoc cref="FramePlayer.Invalid()"/>
        public virtual void Invalid() => FramePlayer = null;

        public static implicit operator FramePlayer(CustomPlayer yPlayer) => yPlayer.FramePlayer;
    }
}
