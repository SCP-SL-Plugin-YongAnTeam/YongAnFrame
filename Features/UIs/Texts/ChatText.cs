using YongAnFrame.Features.Players;
using YongAnFrame.Features.UIs.Enums;

namespace YongAnFrame.Features.UIs.Texts
{
    public class ChatText(string text, float duration, FramePlayer player = null, ChatType type = ChatType.Unknown) : Text(text, duration)
    {
        public ChatType Type { get; } = type;
        /// <inheritdoc/>
        public override string ToString()
        {
            string text = null;
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
                default:
                    break;
            }
            return text;
        }


        public static implicit operator string(ChatText text) => text.ToString();
        public static implicit operator ChatText(string text) => new(text, 60);
    }
}
