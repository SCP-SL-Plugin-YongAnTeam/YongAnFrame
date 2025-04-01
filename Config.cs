using Exiled.API.Interfaces;
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
        /// 全局的经验加成
        /// </summary>
        [Description("全局的经验加成")]
        public float GlobalExpMultiplier { get; set; } = 1;

    }
}
