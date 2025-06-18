using YongAnFrame.Features.UI.Texts;

namespace YongAnFrame.Features.UI.Enums
{
    /// <summary>
    /// <seealso cref="ChatText"/>的类型
    /// </summary>
    public enum ChatType : byte
    {
        /// <summary>
        /// 未知
        /// </summary>
        Unknown = 0,
        /// <summary>
        /// 全部
        /// </summary>
        All = 1,
        /// <summary>
        /// 队伍
        /// </summary>
        Team = 2,
        /// <summary>
        /// 私聊
        /// </summary>
        Private = 3,
    }
}
