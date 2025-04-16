using CommandSystem;
using Exiled.API.Features;
using System;
using YongAnFrame.Extensions;
using YongAnFrame.Features.Players;
using YongAnFrame.Features.Roles;
using YongAnFrame.Features.Roles.Properties;
using YongAnFrame.Features.UI.Enums;
using YongAnFrame.Features.UI.Texts;

namespace YongAnFrame.Commands
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public sealed class SkillsCommand : ICommand
    {
        public string Command => "skills";

        public string[] Aliases => ["sk"];

        public string Description => "skills";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "NO";

            if (arguments.Count >= 1 && int.TryParse(arguments.Array[1], out int num) && Player.TryGet(sender, out Player player))
            {
                FramePlayer fPlayer = player.ToFPlayer();

                if (fPlayer.CustomRolePlus is not null && fPlayer.CustomRolePlus.Check(fPlayer, out DataProperties data))
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
