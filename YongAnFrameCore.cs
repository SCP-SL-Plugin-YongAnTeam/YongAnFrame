using Exiled.API.Enums;
using Exiled.API.Features;
using SCPSLAudioApi;
using YongAnFrame.Core;
using YongAnFrame.Core.Manager;
using YongAnFrame.Role.Core;

namespace YongAnFrame
{
    public class YongAnFramePlugin : Plugin<YongAnFrameConfig>
    {
        private static YongAnFramePlugin instance;
        public static YongAnFramePlugin Instance => instance;
        public override PluginPriority Priority => PluginPriority.First;

        public override void OnEnabled()
        {
            Log.Info("\r\n __  __     ______     __   __     ______     ______     __   __    \r\n/\\ \\_\\ \\   /\\  __ \\   /\\ \"-.\\ \\   /\\  ___\\   /\\  __ \\   /\\ \"-.\\ \\   \r\n\\ \\____ \\  \\ \\ \\/\\ \\  \\ \\ \\-.  \\  \\ \\ \\__ \\  \\ \\  __ \\  \\ \\ \\-.  \\  \r\n \\/\\_____\\  \\ \\_____\\  \\ \\_\\\\\"\\_\\  \\ \\_____\\  \\ \\_\\ \\_\\  \\ \\_\\\\\"\\_\\ \r\n  \\/_____/   \\/_____/   \\/_/ \\/_/   \\/_____/   \\/_/\\/_/   \\/_/ \\/_/ \r\n                                                                    \r\n ______   ______     ______     __    __     ______                 \r\n/\\  ___\\ /\\  == \\   /\\  __ \\   /\\ \"-./  \\   /\\  ___\\                \r\n\\ \\  __\\ \\ \\  __<   \\ \\  __ \\  \\ \\ \\-./\\ \\  \\ \\  __\\                \r\n \\ \\_\\    \\ \\_\\ \\_\\  \\ \\_\\ \\_\\  \\ \\_\\ \\ \\_\\  \\ \\_____\\              \r\n  \\/_/     \\/_/ /_/   \\/_/\\/_/   \\/_/  \\/_/   \\/_____/              \r\n                                                                    \r\n ");
            instance = this;
            Log.Info("============System============");
            FramePlayer.SubscribeStaticEvents();
            MusicManager.Instance.Init();
            CustomRolePlus.SubscribeStaticEvents();
            Startup.SetupDependencies();
            base.OnEnabled();
        }

        public override void OnDisabled()
        {
            instance = null;
            FramePlayer.UnsubscribeStaticEvents();
            CustomRolePlus.UnsubscribeStaticEvents();
            base.OnDisabled();
        }
    }
}
