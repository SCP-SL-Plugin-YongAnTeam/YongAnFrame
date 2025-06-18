using YongAnFrame.Features.UI.Texts;

namespace YongAnFrame.Features.UI.Enums
{
    /// <summary>
    /// <seealso cref="MessageText"/>的类型
    /// </summary>
    public enum MessageType : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 管理员
        /// </summary>
        Admin = 1,
        /// <summary>
        /// 反馈
        /// </summary>
        Feedback = 2,
        /// <summary>
        /// 系统
        /// </summary>
        System = 3,
        /// <summary>
        /// 安全
        /// </summary>
        Safety = 4,
        /// <summary>
        /// 异常
        /// </summary>
        Abnormal = 5
    }
}
