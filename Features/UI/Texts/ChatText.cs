using YongAnFrame.Features.Players;
using YongAnFrame.Features.UI.Enums;

namespace YongAnFrame.Features.UI.Texts
{
    /// <summary>
    /// 给<seealso cref="PlayerUI.MessageList"/>准备的消息文本
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="duration">时效</param>
    /// <param name="type">聊天类型</param>
    /// <param name="player">发送者(null时是匿名)</param>
    public class ChatText(string text, float duration, ChatType type = ChatType.Unknown, FramePlayer? player = null) : Text(text, duration)
    {
        /// <summary>
        /// 获取聊天类型
        /// </summary>
        public ChatType Type { get; } = type;
        /// <inheritdoc/>
        public override string ToString()
        {
            string text = "Error";
            switch (Type)
            {
                case ChatType.Unknown:
                    text = $"[未知]|[{(player is null ? $"<color=purple>匿名</color>" : $"{player.ExPlayer.Nickname}({player.ExPlayer.Role.Team})")}]:<noparse>{Content}</noparse>";
                    break;
                case ChatType.All:
                    text = $"[全部]|[{(player is null ? $"<color=purple>匿名</color>" : $"{player.ExPlayer.Nickname}({player.ExPlayer.Role.Team})")}]:<noparse>{Content}</noparse>";
                    break;
                case ChatType.Team:
                    text = $"[队伍]|[{(player is null ? $"<color=purple>匿名</color>" : $"{player.ExPlayer.Nickname}({player.ExPlayer.Role.Team})")}]:<noparse>{Content}</noparse>";
                    break;
                case ChatType.Private:
                    text = $"[私聊]|[{(player is null ? $"<color=purple>匿名</color>" : $"{player.ExPlayer.Nickname}({player.ExPlayer.Role.Team})")}]:<noparse>{Content}</noparse>";
                    break;
            }
            return text;
        }

        /// <summary>
        /// 隐性转换
        /// </summary>
        /// <param name="text">准备转换的对象</param>
        public static implicit operator string(ChatText text) => text.ToString();
        /// <summary>
        /// 隐性转换
        /// </summary>
        /// <param name="text">准备转换的对象</param>
        public static implicit operator ChatText(string text) => new(text, 60);
    }
}
