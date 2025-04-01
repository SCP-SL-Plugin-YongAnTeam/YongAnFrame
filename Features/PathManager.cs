using Exiled.API.Features;

namespace YongAnFrame.Features
{
    public static class PathManager
    {
        public static string Music => $"{Paths.Exiled}/YongAnFrame/{Server.Port}/Music";
        public static string Log => $"{Paths.Exiled}/YongAnFrame/{Server.Port}/Log";
    }
}
