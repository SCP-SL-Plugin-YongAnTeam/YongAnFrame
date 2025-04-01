using YongAnFrame.Features.UIs.Enums;

namespace YongAnFrame.Features.UIs.Texts
{
    public class MessageText(string text, float duration, MessageType type = MessageType.Unknown) : Text(text, duration)
    {
        public MessageType Type { get; } = type;

        /// <inheritdoc/>
        public override string ToString()
        {
            string text = null;
            switch (Type)
            {
                case MessageType.Unknown:
                    text = $"[未知] {text}";
                    break;
                case MessageType.Admin:
                    text = $"<color=red>[管理员]</color> {Content}";
                    break;
                case MessageType.Feedback:
                    text = $"<color=purple>[玩家反馈]</color> {Content}";
                    break;
                case MessageType.System:
                    text = $"[系统] {Content}";
                    break;
                case MessageType.Safety:
                    text = $"<color=orange>[安全]</color> {Content}";
                    break;
                case MessageType.Abnormal:
                    text = $"<color=red>[异常]</color> {Content}";
                    break;
                default:
                    break;
            }
            return text;
        }

        public static implicit operator string(MessageText text) => text.ToString();
        public static implicit operator MessageText(string text) => new(text, -1);
    }
}
