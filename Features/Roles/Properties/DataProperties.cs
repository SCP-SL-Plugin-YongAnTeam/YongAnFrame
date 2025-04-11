namespace YongAnFrame.Features.Roles.Properties
{
    /// <summary>
    /// 自定义角色数据
    /// </summary>
    public class DataProperties
    {
        /// <summary>
        /// 获取或设置自定义角色的技能管理器
        /// </summary>
        public Skill[] Skills { get; set; }
        /// <summary>
        /// 获取或设置自定义角色是否正常死亡
        /// </summary>
        public bool IsDeathHandling { get; set; }
    }
}
