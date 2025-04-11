using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using System;
using System.Collections.Generic;
using YongAnFrame.Extensions;
using YongAnFrame.Features.Players;
using YongAnFrame.Features.UI.Enums;
using YongAnFrame.Features.UI.Texts;

namespace YongAnFrame.Commands
{
    /// <summary>
    /// 发送消息指令
    /// </summary>
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public sealed class MessageCommand : ICommand
    {
        public string Command => "message";

        public string[] Aliases => ["mes", "msg"];

        public string Description => "用于发送消息";

        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            List<FramePlayer> choicePlayer = [];
            if (arguments.Count < 3)
            {
                response = "不允许的格式，格式应该是 int string int";
                return false;
            }
            switch (arguments.Array[1])
            {
                case "all":
                    if (!sender.CheckPermission("yongan404.message.choice.all"))
                    {
                        response = "请保证你有 yongan404.message.choice.all 权限";
                        return false;
                    }
                    foreach (Player player in Player.List)
                    {
                        choicePlayer.Add(player.ToFPlayer());
                    }
                    break;
                default:
                    string[] idStringArray = arguments.Array[1].Split(',');
                    foreach (string idString in idStringArray)
                    {
                        if (idStringArray.Length > 1 && !sender.CheckPermission("yongan404.message.choice.multiple"))
                        {
                            response = "请保证你有 yongan404.message.choice.multiple 权限";
                            return false;
                        }

                        if (int.TryParse(idString, out int id))
                        {
                            FramePlayer yPlayer = FramePlayer.Get(id);
                            if (yPlayer is not null)
                            {
                                choicePlayer.Add(yPlayer);
                            }
                        }
                        break;
                    }
                    break;
            }

            if (choicePlayer.Count <= 0)
            {
                response = "在你的选择中没有任何一个可用的玩家ID";
                return false;
            }

            if (int.TryParse(arguments.Array[3], out int duration))
            {
                if (duration > 10 && !sender.CheckPermission("yongan404.message.send.large_duration"))
                {
                    response = "请保证你有 yongan404.message.send.large_duration 权限";
                    return false;
                }

                foreach (FramePlayer yPlayer in choicePlayer)
                {
                    yPlayer.UI.MessageList.Add(new MessageText($"{arguments.Array[2]}", duration, MessageType.Admin));
                }
                response = "已成功运行";
                return true;
            }
            else
            {
                response = "不是一个数字类型";
                return false;
            }
        }

    }
}
