using YongAnFrame.Features.UI.Enums;

namespace YongAnFrame.Features.UI.Texts
{
    /// <summary>
    /// 给<seealso cref="PlayerUI.MessageList"/>准备的消息文本
    /// </summary>
    /// <param name="text">内容</param>
    /// <param name="duration">时效</param>
    /// <param name="type">信息类型</param>
    public class MessageText(string text, float duration, MessageType type = MessageType.Unknown) : Text(text, duration)
    {
        /// <summary>
        /// 获取信息类型
        /// </summary>
        public MessageType Type { get; } = type;

        /// <inheritdoc/>
        public override string ToString()
        {
            string text = null;
            switch (Type)
            {
                case MessageType.Unknown:
                    text = $"[{Duration}][未知] {text}";
                    break;
                case MessageType.Admin:
                    text = $"[{Duration}]<color=red>[管理员]</color> {Content}";
                    break;
                case MessageType.Feedback:
                    text = $"[{Duration}]<color=purple>[玩家反馈]</color> {Content}";
                    break;
                case MessageType.System:
                    text = $"[{Duration}][系统] {Content}";
                    break;
                case MessageType.Safety:
                    text = $"[{Duration}]<color=orange>[安全]</color> {Content}";
                    break;
                case MessageType.Abnormal:
                    text = $"[{Duration}]<color=red>[异常]</color> {Content}";
                    break;
                default:
                    break;
            }
            return text;
        }
        /// <summary>
        /// 隐性转换
        /// </summary>
        /// <param name="text">准备转换的对象</param>
        public static implicit operator string(MessageText text) => text.ToString();
        /// <summary>
        /// 隐性转换
        /// </summary>
        /// <param name="text">准备转换的对象</param>
        public static implicit operator MessageText(string text) => new(text, -1);
    }
}
