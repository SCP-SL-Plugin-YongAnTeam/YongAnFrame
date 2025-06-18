using CommandSystem;
using Exiled.API.Features;
using System;
using YongAnFrame.Extensions;
using YongAnFrame.Features.Players;
using YongAnFrame.Features.Roles;
using YongAnFrame.Features.UI.Enums;
using YongAnFrame.Features.UI.Texts;

namespace YongAnFrame.Commands
{
    /// <summary>
    /// 技能指令
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public sealed class SkillsCommand : ICommand
    {
        ///<inheritdoc cref="ExpCommand.Command"/>
        public string Command => "skills";
        ///<inheritdoc cref="ExpCommand.Aliases"/>
        public string[] Aliases => ["sk"];
        ///<inheritdoc cref="ExpCommand.Description"/>
        public string Description => "skills";
        ///<inheritdoc cref="ExpCommand.Execute"/>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "NO";

            if (arguments.Count >= 1 && int.TryParse(arguments.Array[1], out int num) && Player.TryGet(sender, out Player player))
            {
                FramePlayer fPlayer = player.ToFPlayer();

                if (fPlayer.CustomRolePlus is not null && fPlayer.CustomRolePlus.Check(fPlayer, out CustomRolePlusData data))
                {
                    if (data.Skills == null)
                    {
                        response = "角色没有技能";
                        return false;
                    }

                    Skill skill = data.Skills[num];
                    if (skill.IsActive)
                    {
                        fPlayer.UI.MessageList.Add(new MessageText("技能正在持续", 5, MessageType.System));
                    }
                    else if (skill.IsBurial)
                    {
                        fPlayer.UI.MessageList.Add(new MessageText($"技能正在冷却(CD:{skill.BurialRemainingTime})", 5, MessageType.System));
                    }
                    else
                    {
                        skill.Run();
                    }

                    response = "OK";
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
