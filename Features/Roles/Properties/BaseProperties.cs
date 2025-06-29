﻿namespace YongAnFrame.Features.Roles.Properties
{
    /// <summary>
    /// <seealso cref="CustomRolePlus"/>的基础属性
    /// </summary>
    public struct BaseProperties()
    {
        /// <summary>
        /// 获取或设置伤害加成倍数
        /// </summary>
        public float AttackDamageMultiplier { get; set; } = 1;
        /// <summary>
        /// 获取或设置攻击无视护甲
        /// </summary>
        public bool IsAttackIgnoresArmor { get; set; } = false;
        /// <summary>
        /// 获取或设置攻击无视减Ahp伤盾
        /// </summary>
        public bool IsAttackIgnoresAhp { get; set; } = false;
        /// <summary>
        /// 获取或设置伤害减伤倍数
        /// </summary>
        public float DamageResistanceMultiplier { get; set; } = 1;
        /// <summary>
        /// 获取或设置基础移动速度倍数
        /// </summary>
        public float BaseMovementSpeedMultiplier { get; set; } = 1;
    }
}
