using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.Features;
using MEC;
using System.Collections.Generic;
using System.Linq;
using YongAnFrame.Events.EventArgs.FramePlayer;
using YongAnFrame.Role.Core;

namespace YongAnFrame.Core.Data
{
    public class FramePlayer
    {
        public Player ExPlayer { get; private set; }

        private static readonly Dictionary<int, FramePlayer> dictionary = [];
        public static IReadOnlyCollection<FramePlayer> List => dictionary.Values.Where((p) => !p.IsInvalid).ToList();
        public bool IsInvalid { get => ExPlayer == null; }

        public HintManager HintManager { get; private set; }
        public CustomRolePlusData CustomRoleData { get; set; }
        public ulong Level { get; set; }
        private PlayerTitle usingTitles = null;
        public PlayerTitle UsingTitles { get => usingTitles; set { if (value != null && !value.Pro) { usingTitles = value; } } }

        private PlayerTitle usingProTitles = null;
        public PlayerTitle UsingProTitles { get => usingProTitles; set { if (value != null && value.Pro) { usingProTitles = value; } } }



        #region Static
        public static void SubscribeStaticEvents()
        {
            Exiled.Events.Handlers.Player.Verified += new CustomEventHandler<VerifiedEventArgs>(OnStaticVerified);
            Exiled.Events.Handlers.Server.WaitingForPlayers += new CustomEventHandler(OnStaticWaitingForPlayers);
            Exiled.Events.Handlers.Player.Destroying += new CustomEventHandler<DestroyingEventArgs>(OnStaticDestroying);
        }

        private static void OnStaticVerified(VerifiedEventArgs args)
        {
            CustomRolePlus.NoCustomRole.Add(new(args.Player));
        }
        private static void OnStaticDestroying(DestroyingEventArgs args)
        {
            FramePlayer fPlayer = args.Player.ToFPlayer();
            fPlayer.Invalid();
            CustomRolePlus.NoCustomRole.Remove(fPlayer);
        }

        private static void OnStaticWaitingForPlayers()
        {
            dictionary.Clear();
        }

        #endregion

        public FramePlayer(Player player)
        {
            ExPlayer = player;
            HintManager = new HintManager(this);
            dictionary.Add(ExPlayer.Id, this);
            Events.Handlers.FramePlayer.OnCreateFramePlayer(new CreateFramePlayerEventArgs(this));
        }

        #region Show

        private CoroutineHandle[] coroutines = new CoroutineHandle[2];
        private string showName;
        private string showColor;

        public void ShowRank(string name = null, string color = null)
        {
            if (ExPlayer.IsNPC) return;

            showName = name;
            showColor = color;

            if (ExPlayer.GlobalBadge != null)
            {
                ExPlayer.CustomName = $"[LV:{Level}][全球徽章]{ExPlayer.Nickname}";
                if (showName != null)
                {
                    ExPlayer.RankName = $"*{ExPlayer.GlobalBadge.Value.Text}* {showName}";
                }
                else
                {
                    ExPlayer.RankName = $"{ExPlayer.GlobalBadge.Value.Text}";
                }
                ExPlayer.RankColor = $"{ExPlayer.GlobalBadge.Value.Color}";
                return;
            }

            if (usingProTitles != null)
            {
                if (usingProTitles.DynamicCommand != null)
                {
                    Timing.KillCoroutines(coroutines[0]);
                    coroutines[0] = Timing.RunCoroutine(DynamicProTitlesShow(showName));
                }
                else
                {
                    if (usingProTitles.Color != null)
                    {
                        ExPlayer.RankColor = usingProTitles.Color;
                    }
                    else
                    {
                        ExPlayer.RankColor = showColor ?? null;
                    }

                    if (showName != null)
                    {
                        ExPlayer.RankName = $"{showName} *{usingProTitles.Name}*";
                    }
                    else
                    {
                        ExPlayer.RankName = usingProTitles.Name;
                    }
                }
            }

            if (usingTitles != null)
            {
                if (usingTitles.DynamicCommand != null)
                {
                    Timing.KillCoroutines(coroutines[1]);
                    coroutines[1] = Timing.RunCoroutine(DynamicTitlesShow());
                }
                else
                {
                    ExPlayer.CustomName = $"[LV:{Level}][{usingTitles.Name}]{ExPlayer.Nickname}";
                    if (usingTitles.Color != null)
                    {
                        ExPlayer.RankColor = usingTitles.Color;
                    }
                }
            }
            else
            {
                if (showName != null)
                {
                    ExPlayer.RankName = showName;
                }
                if (showColor != null)
                {
                    ExPlayer.RankColor = showColor;
                }
            }

            if (usingTitles == null) ExPlayer.CustomName = $"[LV:{Level}]{ExPlayer.Nickname}";
        }

        private IEnumerator<float> DynamicProTitlesShow(string name = null)
        {
            while (true)
            {
                foreach (var command in usingProTitles.DynamicCommand)
                {
                    if (name != null)
                    {
                        ExPlayer.RankName = $"{name ?? name} *{command[0]}*";
                    }
                    else
                    {
                        ExPlayer.RankName = $"{command[0]}";
                    }

                    ExPlayer.RankColor = command[1] != "null" ? command[1] : ExPlayer.RankColor;
                    yield return Timing.WaitForSeconds(float.Parse(command[2]));
                }
            }
        }
        private IEnumerator<float> DynamicTitlesShow()
        {
            while (true)
            {
                foreach (var command in usingTitles.DynamicCommand)
                {
                    ExPlayer.CustomName = $"[LV:{Level}][{command[0]}]{ExPlayer.Nickname}";
                    if (usingProTitles == null)
                    {
                        ExPlayer.RankColor = command[1] != "null" ? command[1] : ExPlayer.RankColor;
                    }
                    yield return Timing.WaitForSeconds(float.Parse(command[2]));
                }
            }
        }
        #endregion

        public static FramePlayer Get(Player player)
        {
            if (dictionary.TryGetValue(player.Id, out FramePlayer yPlayer))
            {
                return yPlayer;
            }
            return null;
        }
        public static FramePlayer Get(int numId)
        {
            return Get(Player.Get(numId));
        }

        public void Invalid()
        {
            Events.Handlers.FramePlayer.OnInvalidFramePlayer(new InvalidFramePlayerEventArgs(this));
            HintManager?.Clean();
            ExPlayer = null;
        }

        public static implicit operator Player(FramePlayer yPlayer)
        {
            return yPlayer.ExPlayer;
        }
    }
}
