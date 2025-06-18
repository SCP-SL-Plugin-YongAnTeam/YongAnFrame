using Exiled.API.Features;
using System;
using System.Collections.Generic;

namespace YongAnFrame.Features.Players
{
    /// <summary>
    /// <seealso cref="FramePlayer"/>的称号
    /// </summary>
    public sealed class PlayerTitle
    {
        private static readonly Dictionary<uint, PlayerTitle> dictionary = [];
        /// <summary>
        /// 获取有效的玩家称号列表
        /// </summary>
        public static IReadOnlyCollection<PlayerTitle> List => [.. dictionary.Values];
        /// <summary>
        /// 获取或设置加载称号委托
        /// </summary>
        public static Func<uint, PlayerTitle?>? LoadFunc { get; set; }
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

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="id">ID</param>
        /// <param name="name">名称</param>
        /// <param name="color">颜色</param>
        /// <param name="isRank">是否为Rank</param>
        /// <param name="dynamicCommandString">动态指令集</param>
        public PlayerTitle(uint id, string name, string color, bool isRank, string? dynamicCommandString = null)
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
        public void SetDynamicCommand(string? dynamicCommandString)
        {
            List<string[]>? dynamicCommands = null;
            if (!string.IsNullOrEmpty(dynamicCommandString))
            {
                dynamicCommands = [];
                foreach (string dCommand in dynamicCommandString!.Split(';'))
                {
                    dynamicCommands.Add(dCommand.Split(','));
                }
            }
            DynamicCommand = dynamicCommands;
        }

        /// <summary>
        /// 获取称号
        /// </summary>
        /// <param name="id">称号ID</param>
        /// <returns>获取的称号</returns>
        public static PlayerTitle? Get(uint id)
        {
            if (LoadFunc is null)
            {
                Log.Error("称号功能无法在框架内获取，请设置PlayerTitle.LoadFunc属性或写个缓存");
                return null;
            }

            if (dictionary.TryGetValue(id, out PlayerTitle? title))
            {
                return title;
            }
            title = LoadFunc.Invoke(id);
            if (title != null) dictionary.Add(id, title);
            return title;
        }
    }
}
