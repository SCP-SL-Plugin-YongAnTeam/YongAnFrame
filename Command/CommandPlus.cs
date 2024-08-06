using CommandSystem;
using System;

namespace YongAnFrame.Command
{
    public abstract class CommandPlus : ICommand
    {
        public bool SanitizeResponse => true;

        public abstract string Command { get; }
        public abstract string[] Aliases { get; }
        public abstract string Description { get; }

        public abstract bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response);
    }
}
