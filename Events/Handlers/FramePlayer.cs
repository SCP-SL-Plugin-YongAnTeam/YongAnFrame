using Exiled.Events.Features;
using YongAnFrame.Events.EventArgs.FramePlayer;

namespace YongAnFrame.Events.Handlers
{
    public sealed class FramePlayer
    {
        /// <summary>
        /// FramePlayer被创建时的事件
        /// </summary>
        public static Event<FramePlayerCreatedEventArgs> FramePlayerCreated { get; set; } = new Event<FramePlayerCreatedEventArgs>();
        /// <summary>
        /// FramePlayer被无效时的事件
        /// </summary>
        public static Event<FramePlayerInvalidatingEventArgs> FramePlayerInvalidating { get; set; } = new Event<FramePlayerInvalidatingEventArgs>();
        /// <summary>
        /// FramePlayer提示刷新前的事件
        /// </summary>
        public static Event FramePlayerHintUpdate { get; set; } = new Event();

        public static void OnFramePlayerCreated(FramePlayerCreatedEventArgs args)
        {
            FramePlayerCreated.InvokeSafely(args);
        }
        public static void OnFramerHintUpdate()
        {
            FramePlayerHintUpdate.InvokeSafely();
        }
        public static void OnFramePlayerInvalidating(FramePlayerInvalidatingEventArgs args)
        {
            FramePlayerInvalidating.InvokeSafely(args);
        }
    }
}
