using Exiled.API.Features;
using Exiled.API.Features.DamageHandlers;
using Exiled.API.Features.Spawn;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.Features;
using Exiled.Loader;
using PlayerRoles;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using YongAnFrame.Extensions;
using YongAnFrame.Features.Players;
using YongAnFrame.Features.Roles.Enums;
using YongAnFrame.Features.Roles.Interfaces;
using YongAnFrame.Features.Roles.Properties;
using YongAnFrame.Features.UI.Enums;
using YongAnFrame.Features.UI.Texts;

namespace YongAnFrame.Features.Roles
{
    public abstract class CustomRolePlus : CustomRole
    {
        /// <summary>
        /// 不要修改这个值
        /// </summary>
        public override bool IgnoreSpawnSystem { get; set; } = false;
        /// <summary>
        /// 获取或设置自定义角色的生成属性
        /// </summary>
        public new virtual Properties.SpawnProperties SpawnProperties { get; set; } = new Properties.SpawnProperties();
        /// <summary>
        /// 获取或设置自定义角色是否开启生成
        /// </summary>
        public bool IsStartSpawn { get; set; } = true;
        internal Dictionary<FramePlayer, DataProperties> BaseData { get; } = [];
        /// <summary>
        /// 获取或设置自定义角色的基础属性
        /// </summary>
        public virtual BaseProperties BaseProperties { get; set; } = new BaseProperties();
        /// <summary>
        /// 获取或设置自定义角色的名字颜色
        /// </summary>
        public abstract string NameColor { get; set; }
        /// <summary>
        /// 获取自定义角色的联动死亡文本表
        /// </summary>
        public Dictionary<uint, string> RoleDeathText { get; } = [];
        /// <summary>
        /// 获取或设置自定义角色的生成前的目标角色
        /// </summary>
        public virtual RoleTypeId OldRole { get; set; } = RoleTypeId.None;

        #region Static

        public static int RespawnWave { get; private set; } = 0;
        public static void SubscribeStaticEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted += new CustomEventHandler(OnStaticRoundStarted);
            Exiled.Events.Handlers.Server.RespawningTeam += new CustomEventHandler<RespawningTeamEventArgs>(OnStaticRespawningTeam);
        }
        public static void UnsubscribeStaticEvents()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= new CustomEventHandler(OnStaticRoundStarted);
            Exiled.Events.Handlers.Server.RespawningTeam -= new CustomEventHandler<RespawningTeamEventArgs>(OnStaticRespawningTeam);
        }

        private static void OnStaticRoundStarted() => RespawnWave = 0;
        private static void OnStaticRespawningTeam(RespawningTeamEventArgs args) => RespawnWave++;

        #endregion

        /// <summary>
        /// 获取这个角色所有自定义角色的属性
        /// </summary>
        /// <returns>获取的值</returns>
        public virtual DataProperties[] GetAllProperties() => [.. BaseData.Values];

        /// <summary>
        /// 检查玩家是否拥有该角色
        /// </summary>
        /// <param name="player">框架玩家</param>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public virtual bool Check(FramePlayer player, out DataProperties data) => BaseData.TryGetValue(player, out data);
        /// <summary>
        /// 检查玩家是否拥有该角色
        /// </summary>
        /// <param name="player">框架玩家</param>
        public virtual bool Check(FramePlayer player) => player.CustomRolePlus == this;
        /// <summary>
        /// 给玩家添加这个角色
        /// </summary>
        /// <param name="player">EX玩家</param>
        public override void AddRole(Player player) => AddRole(player.ToFPlayer());
        /// <summary>
        /// 给玩家添加这个角色
        /// </summary>
        /// <param name="fPlayer">框架玩家</param>
        public virtual void AddRole(FramePlayer fPlayer)
        {
            if (Check(fPlayer)) return;

            Log.Debug($"已添加{fPlayer.ExPlayer.Nickname}的{Name}({Id})角色");
 
            base.AddRole(fPlayer.ExPlayer);
            fPlayer.UI.UpdateCustomRoleUI();
            AddRoleData(fPlayer);

            if (BaseProperties.BaseMovementSpeedMultiplier < 1f)
            {
                fPlayer.ExPlayer.EnableEffect(Exiled.API.Enums.EffectType.Disabled);
                fPlayer.ExPlayer.ChangeEffectIntensity(Exiled.API.Enums.EffectType.Disabled, 1);
            }

            if (BaseProperties.BaseMovementSpeedMultiplier > 1f)
            {
                fPlayer.ExPlayer.EnableEffect(Exiled.API.Enums.EffectType.MovementBoost);
                fPlayer.ExPlayer.ChangeEffectIntensity(Exiled.API.Enums.EffectType.MovementBoost, (byte)((BaseProperties.BaseMovementSpeedMultiplier - 1f) * 100));
            }
            if (!string.IsNullOrEmpty(SpawnProperties.Info)) Cassie.MessageTranslated($""/*ADMINISTER TEAM DESIGNATED {CASSIEDeathName} HASENTERED*/, SpawnProperties.Info, true, true, true);
            if (!string.IsNullOrEmpty(SpawnProperties.musicNameName))
            {
                MusicManager.Play(SpawnProperties.musicNameName, $"{Name}");
            }
            fPlayer.UpdateShowInfo();
        }

        protected virtual void AddRoleData(FramePlayer fPlayer)
        {
            DataProperties properties = new();
            BaseData.Add(fPlayer, properties);
            if (this is ISkill skill)
            {
                properties.Skills = new Skill[skill.SkillProperties.Length];
                for (int i = 0; i < skill.SkillProperties.Length; i++)
                {
                    properties.Skills[i] = new(fPlayer, skill, (byte)i);
                }
            }
        }
        /// <summary>
        /// 给玩家移除这个角色
        /// </summary>
        /// <param name="player">EX玩家</param>
        public override void RemoveRole(Player player)
        {
            FramePlayer fPlayer = player.ToFPlayer();
            if (fPlayer is not null)
            {
                RemoveRole(player.ToFPlayer());
            }
        }
        /// <summary>
        /// 给玩家移除这个角色
        /// </summary>
        /// <param name="fPlayer">框架玩家</param>
        public virtual void RemoveRole(FramePlayer fPlayer)
        {
            if (!Check(fPlayer)) return;
            Log.Debug($"已删除{fPlayer.ExPlayer.Nickname}的{Name}({Id})角色");
            if (Check(fPlayer, out DataProperties data) && !data.IsDeathHandling)
            {
                Cassie.MessageTranslated($"Died", $"{Name}游玩二游被榨干而死(非常正常死亡)");
            }
            base.RemoveRole(fPlayer.ExPlayer);
            BaseData.Remove(fPlayer);
            fPlayer.ExPlayer.ShowHint($"", 0.1f);
            fPlayer.UpdateShowInfo();
        }

        #region TrySpawn
        private uint limitCount = 0;
        private uint spawnCount = 0;

        /// <summary>
        /// 尝试给这个玩家生成这个角色
        /// </summary>
        /// <param name="fPlayer">框架玩家</param>
        /// <param name="chanceRef">是否重置limitCount</param>
        /// <returns></returns>
        public virtual bool TrySpawn(FramePlayer fPlayer, bool chanceRef = false)
        {
            if (chanceRef)
            {
                limitCount = 0;
            }
            if (fPlayer.CustomRolePlus is null && spawnCount < SpawnProperties.MaxCount && Server.PlayerCount >= SpawnProperties.MinPlayer && SpawnChanceNum <= SpawnProperties.Chance && SpawnProperties.Limit > limitCount)
            {
                limitCount++;
                spawnCount++;
                AddRole(fPlayer);
                return true;
            }
            return false;
        }

        [Obsolete("旧算法遗留方法，不再进行兼容性维护")]
        public virtual bool TrySpawn(List<FramePlayer> noCustomRole, bool chanceRef = false)
        {
            if (noCustomRole is null || noCustomRole.Count == 0) { return false; }
            return TrySpawn(noCustomRole[Loader.Random.StrictNext(0, noCustomRole.Count)]);
        }
        #endregion

        #region Events
        private void OnRestartingRound()
        {
            limitCount = 0;
            spawnCount = 0;
        }

        //private void OnRoundStarted()
        //{
        //    if (IStaetSpawn && SpawnProperties.RefreshTeam == RefreshTeamType.Start)
        //    {
        //        TrySpawn(NoCustomRole.FindAll((p) => OldRole == RoleTypeId.None && Role == p.ExPlayer.Role.Type || p.ExPlayer.Role.Type == OldRole));
        //    }
        //}
        public int SpawnChanceNum { get; private set; } = Loader.Random.StrictNext(1, 101);

        private void OnStaticRestartingRound() => SpawnChanceNum = Loader.Random.StrictNext(1, 101);


        private void OnSpawning(SpawningEventArgs args)
        {
            FramePlayer fPlayer = args.Player.ToFPlayer();
            if (fPlayer.ExPlayer.GetCustomRoles().Count > 0)
            {
                return;
            }
            if (IsStartSpawn && (OldRole != RoleTypeId.None && args.Player.Role.Type == OldRole) || (OldRole == RoleTypeId.None && args.Player.Role.Type == Role))
            {
                switch (SpawnProperties.RefreshTeam)
                {
                    case RefreshTeamType.Start:
                        TrySpawn(fPlayer);
                        break;
                    case RefreshTeamType.MTF:
                    case RefreshTeamType.CI:
                        if (SpawnProperties.StartWave <= RespawnWave)
                        {
                            TrySpawn(fPlayer);
                        }
                        break;
                }
            }
        }
        private void OnDroppingItem(DroppingItemEventArgs args)
        {
            FramePlayer fPlayer = args.Player.ToFPlayer();
            if (Check(fPlayer, out DataProperties data))
            {
                if (data.Skills is not null)
                {
                    foreach (var skill in data.Skills)
                    {
                        if (args.Item.Type == skill.SkillProperties.UseItem)
                        {
                            if (skill.IsActive)
                            {
                                fPlayer.UI.MessageList.Add(new MessageText("技能正在持续", 5, MessageType.System));
                            }
                            else if (skill.IsBurial)
                            {
                                fPlayer.UI.MessageList.Add(new MessageText($"技能正在冷却(CD:{skill.BurialRemainingTime})", 5, MessageType.System));
                            }
                            else
                            {
                                skill.Run();
                            }
                            args.IsAllowed = false;
                        }
                    }
                }
            }
        }
        private void OnHurting(HurtingEventArgs args)
        {
            if (args.Attacker is not null && args.Player is not null)
            {
                if (Check(args.Player))
                {
                    args.Amount *= BaseProperties.DamageResistanceMultiplier;
                }
                else if (Check(args.Attacker))
                {
                    DamageHandler damageHandler = args.DamageHandler;
                    float damage = damageHandler.Damage * BaseProperties.AttackDamageMultiplier;
                    if (BaseProperties.IsAttackIgnoresArmor)
                    {
                        if (damageHandler is FirearmDamageHandler firearmDamageHandler)
                        {
                            damage += ((Exiled.API.Features.Roles.HumanRole)damageHandler.Target.Role).GetArmorEfficacy(firearmDamageHandler.Hitbox);
                        }
                    }
                    if (BaseProperties.IsAttackIgnoresAhp)
                    {
                        damage += damageHandler.AbsorbedAhpDamage;
                    }
                    else
                    {
                        damageHandler.AbsorbedAhpDamage = 0;
                    }

                    if (damage < 0)
                    {
                        damageHandler.DealtHealthDamage = 0;
                    }
                    else
                    {
                        damageHandler.Damage = damage;
                    }
                }
            }
        }

        private void OnDying(DyingEventArgs args)
        {
            FramePlayer fPlayer = args.Player.ToFPlayer();
            if (Check(fPlayer, out DataProperties data))
            {
                if (args.Attacker is null)
                {
                    Cassie.MessageTranslated($"Died", $"{Name}被充满恶意的游戏环境草飞了");
                    data.IsDeathHandling = true;
                }
                else
                {
                    if (args.Attacker.GetCustomRoles().Count != 0)
                    {
                        CustomRole customRole = args.Attacker.GetCustomRoles()[0];
                        if (RoleDeathText.TryGetValue(customRole.Id, out string text))
                        {
                            Cassie.MessageTranslated($"Died", text.Replace("{Name}", Name).Replace("{Attacker}", customRole.Name));
                        }
                        else
                        {
                            Cassie.MessageTranslated($"Died", $"({Name})被({customRole.Name})斩杀");
                        }
                    }
                    else
                    {
                        Cassie.MessageTranslated($"Died", $"({Name})被({args.Attacker.Nickname})斩杀");
                    }
                }
                data.IsDeathHandling = true;
            }
        }

        protected override void SubscribeEvents()
        {
            //Exiled.Events.Handlers.Server.RoundStarted += new CustomEventHandler(OnRoundStarted);
            Exiled.Events.Handlers.Player.Spawning += new CustomEventHandler<SpawningEventArgs>(OnSpawning);
            Exiled.Events.Handlers.Player.Hurting += new CustomEventHandler<HurtingEventArgs>(OnHurting);
            Exiled.Events.Handlers.Server.RestartingRound += new CustomEventHandler(OnRestartingRound);
            Exiled.Events.Handlers.Player.DroppingItem += new CustomEventHandler<DroppingItemEventArgs>(OnDroppingItem);
            Exiled.Events.Handlers.Player.Dying += new CustomEventHandler<DyingEventArgs>(OnDying);
            Exiled.Events.Handlers.Server.RestartingRound += new CustomEventHandler(OnStaticRestartingRound);
            base.SubscribeEvents();

            if (this is ISkill skill)
            {
                Inventory.Add(ItemType.Coin.ToString());
            }

        }
        protected override void UnsubscribeEvents()
        {
            //Exiled.Events.Handlers.Server.RoundStarted -= new CustomEventHandler(OnRoundStarted);
            Exiled.Events.Handlers.Player.Hurting -= new CustomEventHandler<HurtingEventArgs>(OnHurting);
            Exiled.Events.Handlers.Server.RestartingRound -= new CustomEventHandler(OnRestartingRound);
            Exiled.Events.Handlers.Player.DroppingItem -= new CustomEventHandler<DroppingItemEventArgs>(OnDroppingItem);
            Exiled.Events.Handlers.Player.Spawning -= new CustomEventHandler<SpawningEventArgs>(OnSpawning);
            Exiled.Events.Handlers.Player.Dying -= new CustomEventHandler<DyingEventArgs>(OnDying);
            Exiled.Events.Handlers.Server.RestartingRound -= new CustomEventHandler(OnStaticRestartingRound);
            base.UnsubscribeEvents();

            if (this is ISkill skill)
            {
                Inventory.Remove(ItemType.Coin.ToString());
            }
        }
        #endregion

        protected override void ShowMessage(Player player)
        {

        }

    }
    [Guid("913613e0-c6e7-4511-a079-bacc7bc9000c")]
    public abstract class CustomRolePlus<T> : CustomRolePlus where T : DataProperties, new()
    {
        /// <summary>
        /// 检查玩家是否拥有该角色
        /// </summary>
        /// <param name="player">框架玩家</param>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        public virtual bool Check(FramePlayer player, out T data)
        {
            if (BaseData.TryGetValue(player, out DataProperties baseData))
            {
                data = baseData as T;
                return true;
            }
            data = null;
            return false;
        }

        protected override void AddRoleData(FramePlayer fPlayer)
        {
            T properties = new();
            BaseData.Add(fPlayer, properties);
            if (this is ISkill skill)
            {
                properties.Skills = new Skill[skill.SkillProperties.Length];
                for (int i = 0; i < skill.SkillProperties.Length; i++)
                {
                    properties.Skills[i] = new(fPlayer, skill, (byte)i);
                }
            }
        }
    }
}
