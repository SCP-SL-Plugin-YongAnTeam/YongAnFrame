using CommandSystem;
using Exiled.API.Features;
using System;

namespace YongAnFrame.Commands
{
    /// <summary>
    /// 未完成请勿乱用
    /// </summary>
    [CommandHandler(typeof(ClientCommandHandler))]
    public class SkillsCommand : CommandPlus
    {
        public override string Command => "skills";

        public override string[] Aliases => ["sk"];

        public override string Description => "skills";

        public override bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "NO";

            if (arguments.Array.Length > 1 && int.TryParse(arguments.Array[1], out int num) && Player.TryGet(sender, out Player player))
            {
                //TODO
                return true;
            }

            return false;
        }
    }
}
