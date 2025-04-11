using MEC;
using System.Collections.Generic;
using YongAnFrame.Features.Players;
using YongAnFrame.Features.Roles.Interfaces;
using YongAnFrame.Features.Roles.Properties;
using YongAnFrame.Features.UI.Enums;
using YongAnFrame.Features.UI.Texts;

namespace YongAnFrame.Features.Roles
{
    /// <summary>
    /// 玩家的自定义角色技能
    /// </summary>
    /// <param name="fPlayer">框架玩家</param>
    /// <param name="skill">技能接口</param>
    /// <param name="id">技能ID</param>
    public class Skill(FramePlayer fPlayer, ISkill skill, byte id)
    {
        /// <summary>
        /// 获取技能的ID
        /// </summary>
        public byte Id { get; } = id;
        private ISkillActiveStart SkillActiveStart
        {
            get
            {
                if (skill is ISkillActiveStart skillActiveStart)
                {
                    return skillActiveStart;
                }
                return null;
            }
        }
        private ISkillActiveEnd SkillActiveEnd
        {
            get
            {
                if (skill is ISkillActiveEnd skillActiveEnd)
                {
                    return skillActiveEnd;
                }
                return null;
            }
        }
        private ISkillBurialEnd SkillBurialEnd
        {
            get
            {
                if (skill is ISkillBurialEnd skillBurialEnd)
                {
                    return skillBurialEnd;
                }
                return null;
            }
        }
        /// <summary>
        /// 获取技能的属性
        /// </summary>
        public SkillProperties SkillProperties { get => skill.SkillProperties[Id]; }

        /// <summary>
        /// 获取技能是否行动
        /// </summary>
        public bool IsActive { get => ActiveRemainingTime > 0; }
        /// <summary>
        /// 获取技能是否冷却
        /// </summary>
        public bool IsBurial { get => BurialRemainingTime > 0; }
        /// <summary>
        /// 获取技能的行动时间
        /// </summary>
        public float ActiveRemainingTime { get; private set; }
        /// <summary>
        /// 获取技能的冷却时间
        /// </summary>
        public float BurialRemainingTime { get; private set; }

        private CoroutineHandle coroutineHandle;


        /// <summary>
        /// 使用技能
        /// </summary>
        /// <remarks>
        /// 有计时任务会直接覆盖
        /// </remarks>
        public void Run()
        {
            if (coroutineHandle.IsValid)
            {
                Timing.KillCoroutines(coroutineHandle);
            }

            ActiveRemainingTime = SkillProperties.ActiveMaxTime;
            BurialRemainingTime = SkillProperties.BurialMaxTime;

            coroutineHandle = Timing.RunCoroutine(Timer());
            fPlayer.UI.MessageList.Add(new MessageText($"技能[{SkillProperties.Name}]已经发动，持续时间：{SkillProperties.ActiveMaxTime}", SkillProperties.ActiveMaxTime, MessageType.System));
        }

        private IEnumerator<float> Timer()
        {
            string musicNameName = SkillActiveStart?.ActiveStart(fPlayer, Id);
            if (musicNameName is not null) MusicManager.Play(musicNameName, $"技能发动语音", fPlayer, 10);
            while (IsActive)
            {
                ActiveRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicNameName = SkillActiveEnd?.ActiveEnd(fPlayer, Id);
            if (musicNameName is not null) MusicManager.Play(musicNameName, $"技能结束语音", fPlayer, 10);
            while (IsBurial)
            {
                BurialRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicNameName = SkillBurialEnd?.BurialEnd(fPlayer, Id);
            if (musicNameName is not null) MusicManager.Play(musicNameName, $"技能准备好语音", fPlayer, 10);
        }
    }
}
