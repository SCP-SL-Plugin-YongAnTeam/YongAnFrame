using CommandSystem;
using Exiled.API.Features;
using System;
using YongAnFrame.Players;
using YongAnFrame.Roles;
using YongAnFrame.Roles.Properties;

namespace YongAnFrame.Commands
{
    /// <summary>
    /// 未完成请勿乱用
    /// </summary>
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
                FramePlayer fPlayer = FramePlayer.Get(player);
                
                if (fPlayer.CustomRolePlus != null && fPlayer.CustomRolePlus.Check(fPlayer, out CustomRolePlusProperties data)) 
                {
                    SkillManager skillManager = data.SkillManagers[num];
                    skillManager.Run();
                    fPlayer.HintManager.MessageTexts.Add(new HintManager.Text($"技能[{skillManager.SkillProperties.Name}:{fPlayer.CustomRolePlus.GetType().GUID.ToString() + 10000}]已经发动，持续时间：{skillManager.SkillProperties.ActiveMaxTime}", skillManager.SkillProperties.ActiveMaxTime));
                    response = "OK";
                    return true;
                }
                return false;
            }
            return false;
        }
    }
}
