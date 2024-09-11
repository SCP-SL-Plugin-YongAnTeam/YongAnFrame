using System.Collections.Generic;

namespace YongAnFrame.Players
{
    /// <summary>
    /// 玩家称号
    /// </summary>
    public sealed class PlayerTitle
    {
        public uint Id { get; set; }
        public string Name { get; set; }
        public string Color { get; set; }
        public bool IsRank { get; set; }
        public List<string[]> DynamicCommand { get; set; }

        public PlayerTitle(uint id, string name, string color, bool isRank, string dynamicCommandString)
        {
            Id = id;
            Name = name;
            Color = color;
            IsRank = isRank;
            SetDynamicCommand(dynamicCommandString);
        }

        public void SetDynamicCommand(string dynamicCommandString)
        {
            List<string[]> dynamicCommands = null;
            if (!string.IsNullOrEmpty(dynamicCommandString))
            {
                dynamicCommands = [];
                foreach (string dCommand in dynamicCommandString.Split(';'))
                {
                    dynamicCommands.Add(dCommand.Split(','));
                }
            }
            DynamicCommand = dynamicCommands;
        }
    }
}
