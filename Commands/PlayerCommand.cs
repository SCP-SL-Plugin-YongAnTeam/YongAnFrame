using CommandSystem;
using Exiled.API.Features;
using System;
using YongAnFrame.Extensions;
using YongAnFrame.Features.Players;

namespace YongAnFrame.Commands
{
    /// <summary>
    /// 框架玩家指令
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class PlayerCommand : ICommand
    {
        ///<inheritdoc cref="ExpCommand.Command"/>
        public string Command => "FramePlayer";
        ///<inheritdoc cref="ExpCommand.Aliases"/>
        public string[] Aliases => ["player", "fp" ,"p"];
        ///<inheritdoc cref="ExpCommand.Description"/>
        public string Description => "用于管理自己的YongAnFrame用户";
        ///<inheritdoc cref="ExpCommand.Execute"/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "NULL";
            if (arguments.Count >= 1 && Player.TryGet(sender, out Player player))
            {
                //FramePlayer fPlayer = player.ToFPlayer();
                switch (arguments.Array[1])
                {
                    case "BDNT":
                        // 等待重置
                        //fPlayer.HintManager.Clean();
                        //fPlayer.ExPlayer.ShowHint($"<size=20>{YongAnFramePlugin.Instance.Translation.BypassDoNotTrack.Split('\n')}</size>", 10000f);
                        return true;
                    case "INFO":

                        return true;
                }
            }
            return false;
        }
    }
}
