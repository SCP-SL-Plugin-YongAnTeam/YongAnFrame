using Exiled.API.Features;
using Exiled.CustomRoles.API;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using MEC;
using System;
using System.Collections.Generic;
using YongAnFrame.Events.EventArgs.FramePlayer;
using YongAnFrame.Extensions;
using YongAnFrame.Features.Players.Interfaces;
using YongAnFrame.Features.Roles;
using YongAnFrame.Features.UI.Enums;
using YongAnFrame.Features.UI.Texts;

namespace YongAnFrame.Features.Players
{
    public sealed class FramePlayer : ICustomAlgorithm
    {
        private PlayerTitle? usingTitles = null;
        private PlayerTitle? usingRankTitles = null;
        private static readonly Dictionary<int, FramePlayer> dictionary = [];

        private Player? exPlayer;
        /// <summary>
        /// 获取拥有该实例的<seealso cref="Player"/>
        /// </summary>
        /// <remarks>
        /// 在运行<seealso cref="Events.Handlers.FramePlayer.FramePlayerInvalidating"/>后实例无效，再调用可能会引发<seealso cref="InvalidCastException"/>异常<br/>
        /// 玩家退出后<seealso cref="Player"/>必须无引用，否则会造成数字ID重复的问题
        /// </remarks>
        public Player ExPlayer 
        {
            get 
            {
                if (exPlayer is null)
                {
                    throw new InvalidCastException("FramePlayer实例已无效");
                }
                return exPlayer;
            }
        }
        /// <summary>
        /// 获取有效的框架玩家列表
        /// </summary>
        public static IReadOnlyCollection<FramePlayer> List => [.. dictionary.Values];
        /// <summary>
        /// 获取玩家拥有的自定义角色
        /// </summary>
        public CustomRolePlus? CustomRolePlus
        {
            get
            {
                if (ExPlayer.GetCustomRoles().Count is not 0 && ExPlayer.GetCustomRoles()[0] is CustomRolePlus custom)
                {
                    return custom;
                }
                return null;
            }
        }
        /// <summary>
        /// 获取玩家的UI
        /// </summary>
        public PlayerUI UI { get; private set; }
        /// <summary>
        /// 获取或设置玩家正在使用的主要自定义算法
        /// </summary>
        public ICustomAlgorithm CustomAlgorithm { get; set; }

        /// <summary>
        /// 获取或设置玩家的等级
        /// </summary>
        public ulong Level { get; set; }
        /// <summary>
        /// 获取或设置玩家的经验
        /// </summary>
        public ulong Exp { get; set; }
        /// <summary>
        /// 获取或设置玩家的经验倍率
        /// </summary>
        public float ExpMultiplier { get; set; }
        /// <summary>
        /// 获取或设置玩家的批准绕过DNT
        /// </summary>
        public bool IsBDNT { get; set; }
        /// <summary>
        /// 获取或设置玩家正在使用的名称称号
        /// </summary>
        public PlayerTitle? UsingTitles
        {
            get => usingTitles;
            set
            {
                if (value is not null && !value.IsRank)
                {
                    usingTitles = value;
                }
            }
        }

        /// <summary>
        /// 获取或设置玩家正在使用的地位称号
        /// </summary>
        public PlayerTitle? UsingRankTitles
        {
            get => usingRankTitles;
            set
            {
                if (value is not null && value.IsRank)
                {
                    usingRankTitles = value;
                }
            }
        }

        #region EX增强
        /// <summary>
        /// 获取或设置玩家的地位名称。
        /// </summary>
        public string? RankName
        {
            get => ExPlayer.RankName;
            set
            {
                if (RankName != value)
                {
                    ExPlayer.RankName = value;
                }
            }
        }
        /// <summary>
        /// 获取或设置玩家的地位颜色。
        /// </summary>
        public string? RankColor
        {
            get => ExPlayer.RankColor;
            set
            {
                if (RankColor != value)
                {
                    ExPlayer.RankColor = value;
                }
            }
        }
        /// <summary>
        /// 获取或设置玩家的昵称，如果为 null，则设置原始昵称。
        /// </summary>
        public string CustomName
        {
            get => ExPlayer.CustomName;
            set
            {
                if (CustomName != value)
                {
                    ExPlayer.CustomName = value;
                }
            }
        }
        #endregion

        #region Static
        public static void SubscribeStaticEvents()
        {
            Exiled.Events.Handlers.Player.Verified += new CustomEventHandler<VerifiedEventArgs>(OnStaticVerified);
            //Exiled.Events.Handlers.Server.WaitingForPlayers += new CustomEventHandler(OnStaticWaitingForPlayers);
            Exiled.Events.Handlers.Player.Destroying += new CustomEventHandler<DestroyingEventArgs>(OnStaticDestroying);
        }

        public static void UnsubscribeStaticEvents()
        {
            Exiled.Events.Handlers.Player.Verified += new CustomEventHandler<VerifiedEventArgs>(OnStaticVerified);
            //Exiled.Events.Handlers.Server.WaitingForPlayers += new CustomEventHandler(OnStaticWaitingForPlayers);
            Exiled.Events.Handlers.Player.Destroying += new CustomEventHandler<DestroyingEventArgs>(OnStaticDestroying);
        }

        private static void OnStaticVerified(VerifiedEventArgs args)
        {
            if (args.Player.IsNPC) return;
            new FramePlayer(args.Player);
        }
        private static void OnStaticDestroying(DestroyingEventArgs args)
        {
            FramePlayer fPlayer = args.Player.ToFPlayer();
            fPlayer.Invalid();
        }
        //private static void OnStaticWaitingForPlayers()
        //{
        //    dictionary.Clear();
        //}

        #endregion

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="player">Exiled玩家</param>
        internal FramePlayer(Player player)
        {
            exPlayer = player;
            dictionary.Add(ExPlayer.Id, this);
            UI = new(this);
            CustomAlgorithm = this;
            Events.Handlers.FramePlayer.OnFramePlayerCreated(new FramePlayerCreatedEventArgs(this));
            UpdateShowInfo();
        }

        /// <summary>
        /// 添加经验
        /// </summary>
        /// <param name="exp">数值</param>
        /// <param name="name">原因</param>
        public void AddExp(ulong exp, string name = "未知原因")
        {
            float globalExpMultiplier = YongAnFramePlugin.Instance.Config.GlobalExpMultiplier;
            float expMultiplier = ExpMultiplier * globalExpMultiplier;
            ulong addExp = (ulong)(exp * expMultiplier);

            Exp += addExp;
            UI.MessageList.Add(new MessageText($"{name}，获得{exp}+{addExp - exp}经验({expMultiplier}倍经验)", 5, MessageType.System));

            ulong needExp = CustomAlgorithm.GetNeedUpLevel(Level);
            ulong oldLevel = Level;
            while (Exp >= needExp)
            {
                Log.Debug($"{Exp}/{needExp}");
                Level++;
                Exp -= needExp;
                needExp = CustomAlgorithm.GetNeedUpLevel(Level);
            }
            if (oldLevel < Level)
            {
                UpdateShowInfo();
                UI.MessageList.Add(new MessageText($"恭喜你从{oldLevel}级到达{Level}级,距离下一级需要{Exp}/{needExp}经验", 8, MessageType.System));
            }
        }


        #region ShowRank

        private readonly CoroutineHandle[] coroutines = new CoroutineHandle[2];

        /// <summary>
        /// 更新显示的服务器列表信息
        /// </summary>
        public void UpdateShowInfo()
        {
            if (ExPlayer.IsNPC) return;

            if (ExPlayer.GlobalBadge is not null)
            {
                CustomName = $"[LV:{Level}][全球徽章]{ExPlayer.Nickname}";
                if (CustomRolePlus is not null)
                {
                    RankName = $"*{ExPlayer.GlobalBadge.Value.Text}* {CustomRolePlus.Name}";
                }
                else
                {
                    RankName = $"{ExPlayer.GlobalBadge.Value.Text}";
                }
                RankColor = $"{ExPlayer.GlobalBadge.Value.Color}";
                return;
            }

            string? rankColor = null;
            string? rankName = null;

            if (CustomRolePlus is not null)
            {
                rankName = CustomRolePlus.Name;
                rankColor = CustomRolePlus.NameColor;
            }

            if (usingTitles is not null)
            {
                if (usingTitles.DynamicCommand is not null)
                {
                    Timing.KillCoroutines(coroutines[1]);
                    coroutines[1] = Timing.RunCoroutine(DynamicTitlesShow());
                }
                else
                {
                    CustomName = $"[LV:{Level}][{usingTitles.Name}]{ExPlayer.Nickname}";
                    if (!string.IsNullOrEmpty(usingTitles.Color))
                    {
                        rankColor = usingTitles.Color;
                    }
                }
            }
            else
            {
                ExPlayer.CustomName = $"[LV:{Level}]{ExPlayer.Nickname}";
            }

            if (usingRankTitles is not null)
            {
                if (usingRankTitles.DynamicCommand is not null)
                {
                    Timing.KillCoroutines(coroutines[0]);
                    coroutines[0] = Timing.RunCoroutine(DynamicRankTitlesShow());
                }
                else
                {
                    if (CustomRolePlus is not null)
                    {
                        rankName = $"{CustomRolePlus.Name} *{usingRankTitles.Name}*";
                    }
                    else
                    {
                        rankName = usingRankTitles.Name;
                    }

                    if (!string.IsNullOrEmpty(usingRankTitles.Color))
                    {
                        rankColor = usingRankTitles.Color;
                    }
                }
            }

            RankColor = rankColor;
            RankName = rankName;
        }

        private IEnumerator<float> DynamicRankTitlesShow()
        {
            while (true)
            {
                if (usingRankTitles is null || usingRankTitles.DynamicCommand is null)
                {
                    yield break;
                }
                foreach (var command in usingRankTitles.DynamicCommand)
                {
                    if (CustomRolePlus is not null)
                    {
                        RankName = $"{CustomRolePlus.Name} *{command[0]}*";
                    }
                    else
                    {
                        RankName = $"{command[0]}";
                    }
                    if (usingRankTitles is null)
                    {
                        RankColor = command[1];
                    }
                    yield return Timing.WaitForSeconds(float.Parse(command[2]));
                }
            }
        }
        private IEnumerator<float> DynamicTitlesShow()
        {
            while (true)
            {
                if (usingTitles is null || usingTitles.DynamicCommand is null)
                {
                    yield break;
                }
                foreach (var command in usingTitles.DynamicCommand)
                {
                    CustomName = $"[LV:{Level}][{command[0]}]{ExPlayer.Nickname}";
                    if (usingRankTitles is null)
                    {
                        RankColor = command[1];
                    }
                    yield return Timing.WaitForSeconds(float.Parse(command[2]));
                }
            }
        }
        #endregion

        ///<inheritdoc/>
        public ulong GetNeedUpLevel(ulong level) => (ulong)(100 + Math.Floor(level / 10f) * 100);

        /// <summary>
        /// 获取框架玩家
        /// </summary>
        /// <param name="player">Exiled玩家</param>
        /// <returns>框架玩家</returns>
        public static FramePlayer Get(Player? player)
        {
            if (player is not null && dictionary.TryGetValue(player.Id, out FramePlayer yPlayer))
            {
                return yPlayer;
            }
            throw new InvalidCastException("Player实例无效?");
        }

        /// <summary>
        /// 获取框架玩家
        /// </summary>
        /// <param name="numId">玩家数字ID</param>
        /// <returns>框架玩家</returns>
        public static FramePlayer Get(int numId) => Get(Player.Get(numId));

        /// <summary>
        /// 调用后该实例会立刻无效<br/>
        /// 调用后该实例会立刻无效<br/>
        /// 调用后该实例会立刻无效
        /// </summary>
        public void Invalid()
        {
            Events.Handlers.FramePlayer.OnFramePlayerInvalidating(new FramePlayerInvalidatingEventArgs(this));
            CustomRolePlus?.RemoveRole(this);
            dictionary.Remove(ExPlayer.Id);
            UI.Clean();
            exPlayer = null;
        }

        public static implicit operator Player(FramePlayer yPlayer) => yPlayer.ExPlayer;
    }
}
