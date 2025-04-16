using System.Collections.Generic;

namespace YongAnFrame.Features.Players
{
    /// <summary>
    /// 玩家称号
    /// </summary>
    public sealed class PlayerTitle
    {
        /// <summary>
        /// 获取或设置称号的ID
        /// </summary>
        public uint Id { get; set; }
        /// <summary>
        /// 获取或设置称号的名称
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 获取或设置称号的颜色
        /// </summary>
        public string Color { get; set; }
        /// <summary>
        /// 获取或设置称号是否为Rank
        /// </summary>
        public bool IsRank { get; set; }
        /// <summary>
        /// 获取称号的动态指令集
        /// </summary>
        public List<string[]>? DynamicCommand { get; private set; }

        public PlayerTitle(uint id, string name, string color, bool isRank, string dynamicCommandString)
        {
            Id = id;
            Name = name;
            Color = color;
            IsRank = isRank;
            SetDynamicCommand(dynamicCommandString);
        }

        /// <summary>
        /// 设置称号的动态指令集
        /// </summary>
        /// <param name="dynamicCommandString"></param>
        public void SetDynamicCommand(string dynamicCommandString)
        {
            List<string[]>? dynamicCommands = null;
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
