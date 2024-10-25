using YongAnFrame.Roles;

namespace YongAnFrame.Players
{
    public abstract class CustomPlayer(FramePlayer player)
    {
        /// <summary>
        /// 拥有该实例的框架玩家
        /// </summary>
        public FramePlayer FramePlayer { get; private set; } = player;
        /// <summary>
        /// 是否有效
        /// </summary>
        public bool IsInvalid => FramePlayer == null;
        /// <summary>
        /// 实例拥有的自定义角色
        /// </summary>
        public CustomRolePlus CustomRolePlus => FramePlayer.CustomRolePlus;
        /// <summary>
        /// 提示系统管理器
        /// </summary>
        public HintManager HintManager => FramePlayer.HintManager;
        /// <summary>
        /// 正在使用的主要自定义算法
        /// </summary>
        public ICustomAlgorithm CustomAlgorithm { get => FramePlayer.CustomAlgorithm; set=> FramePlayer.CustomAlgorithm = value; }
        /// <summary>
        /// 玩家等级
        /// </summary>
        public ulong Level { get => FramePlayer.Level; set => FramePlayer.Level = value; }
        /// <summary>
        /// 玩家经验
        /// </summary>
        public ulong Exp { get => FramePlayer.Exp; set => FramePlayer.Exp = value; }
        /// <summary>
        /// 玩家经验倍率
        /// </summary>
        public float ExpMultiplier { get => FramePlayer.ExpMultiplier; set => FramePlayer.ExpMultiplier = value; }
        /// <summary>
        /// 玩家批准绕过DNT
        /// </summary>
        public bool IsBDNT { get => FramePlayer.IsBDNT; set => FramePlayer.IsBDNT = value; }
        /// <summary>
        /// 正在使用的名称称号
        /// </summary>
        public PlayerTitle UsingTitles { get => FramePlayer.UsingTitles; set => FramePlayer.UsingTitles = value; }

        /// <summary>
        /// 正在使用的排名称号
        /// </summary>
        public PlayerTitle UsingRankTitles { get => FramePlayer.UsingRankTitles; set => FramePlayer.UsingRankTitles = value; }
        
        /// <summary>
        /// 添加经验
        /// </summary>
        /// <param name="exp">数值</param>
        /// <param name="name">原因</param>
        public void AddExp(ulong exp, string name = "未知原因")
        {
            FramePlayer.AddExp(exp, name);
        }
        /// <summary>
        /// 调用后该实例会立刻无效<br/>
        /// 调用后该实例会立刻无效<br/>
        /// 调用后该实例会立刻无效
        /// </summary>
        public virtual void Invalid()
        {
            FramePlayer = null;
        }
    }
}
