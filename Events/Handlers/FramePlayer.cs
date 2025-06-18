using Exiled.Events.Features;
using YongAnFrame.Events.EventArgs.FramePlayer;

namespace YongAnFrame.Events.Handlers
{
    /// <summary>
    /// 框架玩家事件处理器
    /// </summary>
    public sealed class FramePlayer
    {
        /// <summary>
        /// <seealso cref="FramePlayer"/>被创建时事件
        /// </summary>
        public static Event<FramePlayerCreatedEventArgs> FramePlayerCreated { get; set; } = new Event<FramePlayerCreatedEventArgs>();
        /// <summary>
        /// <seealso cref="FramePlayer"/>被无效时事件
        /// </summary>
        public static Event<FramePlayerInvalidatingEventArgs> FramePlayerInvalidating { get; set; } = new Event<FramePlayerInvalidatingEventArgs>();
        /// <summary>
        /// <seealso cref="FramePlayer"/>提示刷新前事件
        /// </summary>
        public static Event FramePlayerHintUpdate { get; set; } = new Event();
        /// <summary>
        /// 触发<seealso cref="FramePlayer"/>被创建时事件
        /// </summary>
        /// <param name="args">事件数据</param>
        public static void OnFramePlayerCreated(FramePlayerCreatedEventArgs args) => FramePlayerCreated.InvokeSafely(args);
        /// <summary>
        /// <seealso cref="FramePlayer"/>提示刷新前事件
        /// </summary>
        public static void OnFramerHintUpdate() => FramePlayerHintUpdate.InvokeSafely();
        /// <summary>
        /// <seealso cref="FramePlayer"/>提示刷新前事件
        /// </summary>
        /// <param name="args">事件数据</param>
        public static void OnFramePlayerInvalidating(FramePlayerInvalidatingEventArgs args) => FramePlayerInvalidating.InvokeSafely(args);
    }
}
