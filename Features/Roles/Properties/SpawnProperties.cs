using YongAnFrame.Features.Roles.Enums;

namespace YongAnFrame.Features.Roles.Properties
{
    /// <summary>
    /// 给<seealso cref="CustomRolePlus"/>准备的生成属性
    /// </summary>
    public struct SpawnProperties()
    {
        /// <summary>
        /// 获取或设置每次生成的最多数量
        /// </summary>
        public int MaxCount { get; set; } = 1;
        /// <summary>
        /// 获取或设置生成需要的最少玩家
        /// </summary>
        public int MinPlayer { get; set; } = 0;
        /// <summary>
        /// 获取或设置生成需要的最多玩家
        /// </summary>
        public int MaxPlayer { get; set; } = 1000;
        /// <summary>
        /// 获取或设置生成时播放音频文件
        /// </summary>
        public string? MusicNameName { get; set; } = null;
        /// <summary>
        /// 获取或设置生成时跟随的队伍
        /// </summary>
        public RefreshTeamType RefreshTeam { get; set; } = RefreshTeamType.Start;
        /// <summary>
        /// 暂时弃用
        /// </summary>
        public string? Info { get; set; } = null;
        /// <summary>
        /// 获取或设置生成的数量限制
        /// </summary>
        public uint Limit { get; set; } = 1;
        /// <summary>
        /// 获取或设置每次生成的概率
        /// </summary>
        public float Chance { get; set; } = 100;
        /// <summary>
        /// 获取或设置的刷新波次
        /// </summary>
        /// <remarks>
        /// 只适用于除 <seealso cref="RefreshTeamType.Start"/> 以外的所有内容
        /// </remarks>
        public uint StartWave { get; set; } = 1;
    }
}
