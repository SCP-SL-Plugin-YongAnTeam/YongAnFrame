namespace YongAnFrame.Roles.Properties
{
    public struct MoreProperties
    {
        /// <summary>
        /// 伤害加成倍数
        /// </summary>
        public float AttackDamageMultiplier { get; set; } = 1;
        /// <summary>
        /// 攻击无视护甲
        /// </summary>
        public bool IsAttackIgnoresArmor { get; set; } = false;
        /// <summary>
        /// 攻击无视减Ahp伤盾
        /// </summary>
        public bool IsAttackIgnoresAhp { get; set; } = false;
        /// <summary>
        /// 伤害减伤倍数
        /// </summary>
        public float DamageResistanceMultiplier { get; set; } = 1;
        /// <summary>
        /// 基础移动速度倍数
        /// </summary>
        public float BaseMovementSpeedMultiplier { get; set; } = 1;

        public MoreProperties()
        {
        }
    }
}
