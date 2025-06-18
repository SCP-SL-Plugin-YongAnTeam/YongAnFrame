using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using YongAnFrame.Extensions;

namespace YongAnFrame.Commands
{
    /// <summary>
    /// 玩家经验指令
    /// </summary>
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ExpCommand : ICommand
    {
        /// <summary>
        /// 主要指令名
        /// </summary>
        public string Command => "PlayerExp";
        /// <summary>
        /// 次要指令名
        /// </summary>
        public string[] Aliases => ["pexp","pe"];
        /// <summary>
        /// 指令描述
        /// </summary>
        public string Description => "用于经验的设置";

        /// <summary>
        /// 指令逻辑
        /// </summary>
        /// <param name="arguments">指令集</param>
        /// <param name="sender">发送者</param>
        /// <param name="response">原因</param>
        /// <returns>是否运行成功</returns>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "NO";
            if (sender.CheckPermission("yongan404.level.add"))
            {
                if (arguments.Count >= 1)
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
