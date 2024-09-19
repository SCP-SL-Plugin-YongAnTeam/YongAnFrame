using YongAnFrame.Roles.Enums;

namespace YongAnFrame.Role.Properties
{
    public struct SpawnProperties
    {
        public SpawnProperties()
        {
        }

        /// <summary>
        /// 每次生成最多数量
        /// </summary>
        public int MaxCount { get; set; } = 1;
        /// <summary>
        /// 生成最少玩家
        /// </summary>
        public int MinPlayer { get; set; } = 0;
        /// <summary>
        /// 生成最多玩家
        /// </summary>
        public int MaxPlayer { get; set; } = 1000;
        /// <summary>
        /// 生成时播放音频文件
        /// </summary>
        public string MusicFileName { get; set; } = null;
        /// <summary>
        /// 生成跟随的队伍
        /// </summary>
        public RefreshTeamType RefreshTeam { get; set; } = RefreshTeamType.Start;
        /// <summary>
        /// 暂时弃用
        /// </summary>
        public string Info { get; set; } = null;
        /// <summary>
        /// 角色生成数量限制
        /// </summary>
        public uint Limit { get; set; } = 1;
        /// <summary>
        /// 每次会生成的概率
        /// </summary>
        public float Chance { get; set; } = 100;
        /// <summary>
        /// 在第几次开始刷新，只适用于除RefreshTeamType.Start以外的所有内容
        /// </summary>
        public uint StartWave { get; set; } = 1;
    }
}
