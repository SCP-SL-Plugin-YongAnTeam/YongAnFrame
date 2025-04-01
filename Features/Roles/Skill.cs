using MEC;
using System.Collections.Generic;
using YongAnFrame.Features.Players;
using YongAnFrame.Features.Roles.Interfaces;
using YongAnFrame.Features.Roles.Properties;
using YongAnFrame.Features.UIs.Enums;
using YongAnFrame.Features.UIs.Texts;

namespace YongAnFrame.Features.Roles
{
    /// <summary>
    /// 技能
    /// </summary>
    /// <param name="fPlayer"></param>
    /// <param name="skill"></param>
    /// <param name="id"></param>
    public sealed class Skill(FramePlayer fPlayer, ISkill skill, byte id)
    {
        /// <summary>
        /// 获取或设置技能的ID
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
        /// 获取或设置技能是否行动
        /// </summary>
        public bool IsActive { get => ActiveRemainingTime > 0; }
        /// <summary>
        /// 获取或设置技能是否冷却
        /// </summary>
        public bool IsBurial { get => BurialRemainingTime > 0; }
        /// <summary>
        /// 获取或设置技能的行动时间
        /// </summary>
        public float ActiveRemainingTime { get; private set; }
        /// <summary>
        /// 获取或设置技能的冷却时间
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
            if (coroutineHandle != null)
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
            string musicFileName = SkillActiveStart?.ActiveStart(fPlayer, Id);
            if (musicFileName != null) MusicManager.Play(musicFileName, $"技能发动语音", fPlayer, 10);
            while (IsActive)
            {
                ActiveRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicFileName = SkillActiveEnd?.ActiveEnd(fPlayer, Id);
            if (musicFileName != null) MusicManager.Play(musicFileName, $"技能结束语音", fPlayer, 10);
            while (IsBurial)
            {
                BurialRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicFileName = SkillBurialEnd?.BurialEnd(fPlayer, Id);
            if (musicFileName != null) MusicManager.Play(musicFileName, $"技能准备好语音", fPlayer, 10);
        }
    }
}
