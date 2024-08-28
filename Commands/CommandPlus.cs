using CommandSystem;
using System;

namespace YongAnFrame.Commands
{
    /// <summary>
    /// 防止不兼容目标框架
    /// </summary>
    [Obsolete("官方开眼了，终于没有SanitizeResponse这个答辩了")]
    public abstract class CommandPlus : ICommand
    {
        public bool SanitizeResponse => true;

        public abstract string Command { get; }
        public abstract string[] Aliases { get; }
        public abstract string Description { get; }

        public abstract bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response);
    }
}
