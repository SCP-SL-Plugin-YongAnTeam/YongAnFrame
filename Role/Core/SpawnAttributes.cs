using YongAnFrame.Role.Core.Enums;

namespace YongAnFrame.Role.Core
{
    public struct SpawnAttributes
    {
        public SpawnAttributes()
        {
        }

        /// <summary>
        /// 每次生成最多数量
        /// </summary>
        public int MaxCount { get; set; } = 1;
        public int MinPlayer { get; set; } = 0;
        public int MaxPlayer { get; set; } = 1000;
        public string MusicFileName { get; set; } = null;
        public RefreshTeamType RefreshTeam { get; set; } = RefreshTeamType.Start;
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
