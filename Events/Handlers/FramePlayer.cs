using Exiled.Events.Features;
using YongAnFrame.Events.EventArgs.FramePlayer;

namespace YongAnFrame.Events.Handlers
{
    public class FramePlayer
    {
        /// <summary>
        /// FramePlayer被创建时的事件
        /// </summary>
        public static Event<CreateFramePlayerEventArgs> CreateFramePlayer { get; set; } = new Event<CreateFramePlayerEventArgs>();
        /// <summary>
        /// FramePlayer被无效时的事件
        /// </summary>
        public static Event<InvalidFramePlayerEventArgs> InvalidFramePlayer { get; set; } = new Event<InvalidFramePlayerEventArgs>();

        public static void OnCreateFramePlayer(CreateFramePlayerEventArgs args)
        {
            CreateFramePlayer.InvokeSafely(args);
        }
        public static void OnInvalidFramePlayer(InvalidFramePlayerEventArgs args)
        {
            InvalidFramePlayer.InvokeSafely(args);
        }
    }
}
