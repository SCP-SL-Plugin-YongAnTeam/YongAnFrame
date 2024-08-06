using Exiled.API.Interfaces;

namespace YongAnFrame
{
    public class YongAnFrameConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; }
    }
}
