using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using YongAnFrame;
using YongAnFrame.Commands;

namespace YongAnFrame.Command
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ExpCommand : ICommand
    {
        public string Command => "pexperience";

        public string[] Aliases => ["pexp"];

        public string Description => "用于经验的设置";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "NO";
            if (sender.CheckPermission("yongan404.level.add"))
            {
                if (arguments.Array.Length > 2)
                {
                    Player.Get(arguments.Array[1]).ToFPlayer().Level += ulong.Parse(arguments.Array[2]);
                    response = "OK";
                    return true;
                }
            }
            else
            {
                response = "请保证你有yongan404.level.add权限";
            }

            return false;
        }
    }
}
