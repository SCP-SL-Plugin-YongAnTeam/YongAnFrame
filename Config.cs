using Exiled.API.Interfaces;
using System.Collections.Generic;
using System.ComponentModel;

namespace YongAnFrame
{
    /// <summary>
    /// 插件的配置
    /// </summary>
    public sealed class Config : IConfig
    {
        ///<inheritdoc/>
        public bool IsEnabled { get; set; } = true;
        ///<inheritdoc/>
        public bool Debug { get; set; }
        /// <summary>
        /// 获取或设置全局的经验加成
        /// </summary>
        [Description("全局的经验加成")]
        public float GlobalExpMultiplier { get; set; } = 1;

        /// <summary>
        /// 获取或设置禁用自定义角色生成
        /// </summary>
        /// <remarks>
        /// 只能用于继承<seealso cref="CustomRolePlus"/>的类
        /// </remarks>
        [Description("禁用自定义角色生成(只能用于继承CustomRolePlus的类)")]
        public List<string> DisableCustomRolePlus { get; set; } = ["114514", "1"];
    }
}
