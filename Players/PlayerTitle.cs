using System.Collections.Generic;

namespace YongAnFrame.Players
{
    /// <summary>
    /// 玩家称号
    /// </summary>
    public sealed class PlayerTitle
    {
        /// <summary>
        /// 称号ID
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// 称号名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 称号颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 是否为Rank
        /// </summary>
        public bool IsRank { get; set; }
        /// <summary>
        /// 动态指令集
        /// </summary>
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
