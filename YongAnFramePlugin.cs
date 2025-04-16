using Exiled.API.Enums;
using Exiled.API.Features;
using HarmonyLib;
using SCPSLAudioApi;
using YongAnFrame.Features.Players;
using YongAnFrame.Features.Roles;
using YongAnFrame.Patch;

namespace YongAnFrame
{
    /// <summary>
    /// 插件的驱动
    /// </summary>
    public sealed class YongAnFramePlugin : Plugin<Config, Translation>
    {
        private static YongAnFramePlugin? instance;
        /// <summary>
        /// 获取<seealso cref="YongAnFramePlugin"/>单例
        /// </summary>
        public static YongAnFramePlugin Instance => instance!;
        /// <summary>
        /// 获取<seealso cref="HarmonyLib.Harmony"/>实例
        /// </summary>
        public Harmony Harmony { get; } = new Harmony("YongAnFrame.Harmony");

        ///<inheritdoc/>
        public override PluginPriority Priority => PluginPriority.First - 1;

        ///<inheritdoc/>
        public override void OnEnabled()
        {
            instance = this;
            Log.Info("\r\n __  __     ______     __   __     ______     ______     __   __    \r\n/\\ \\_\\ \\   /\\  __ \\   /\\ \"-.\\ \\   /\\  ___\\   /\\  __ \\   /\\ \"-.\\ \\   \r\n\\ \\____ \\  \\ \\ \\/\\ \\  \\ \\ \\-.  \\  \\ \\ \\__ \\  \\ \\  __ \\  \\ \\ \\-.  \\  \r\n \\/\\_____\\  \\ \\_____\\  \\ \\_\\\\\"\\_\\  \\ \\_____\\  \\ \\_\\ \\_\\  \\ \\_\\\\\"\\_\\ \r\n  \\/_____/   \\/_____/   \\/_/ \\/_/   \\/_____/   \\/_/\\/_/   \\/_/ \\/_/ \r\n                                                                    \r\n ______   ______     ______     __    __     ______                 \r\n/\\  ___\\ /\\  == \\   /\\  __ \\   /\\ \"-./  \\   /\\  ___\\                \r\n\\ \\  __\\ \\ \\  __<   \\ \\  __ \\  \\ \\ \\-./\\ \\  \\ \\  __\\                \r\n \\ \\_\\    \\ \\_\\ \\_\\  \\ \\_\\ \\_\\  \\ \\_\\ \\ \\_\\  \\ \\_____\\              \r\n  \\/_/     \\/_/ /_/   \\/_/\\/_/   \\/_/  \\/_/   \\/_____/              \r\n                                                                    \r\n ");
            Log.Info("============System============");
            FramePlayer.SubscribeStaticEvents();
            CustomRolePlus.SubscribeStaticEvents();
            Startup.SetupDependencies();
            AddLogPatch.StartTask();
            Harmony.PatchAll();
            base.OnEnabled();
        }

        ///<inheritdoc/>
        public override void OnDisabled()
        {
            instance = null;
            FramePlayer.UnsubscribeStaticEvents();
            CustomRolePlus.UnsubscribeStaticEvents();
            Harmony.UnpatchAll();
            base.OnDisabled();
        }
    }
}
