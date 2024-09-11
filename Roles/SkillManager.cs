using MEC;
using System.Collections.Generic;
using YongAnFrame.Players;
using YongAnFrame.Roles.Interfaces;
using YongAnFrame.Roles.Properties;
using static YongAnFrame.Roles.MusicManager;

namespace YongAnFrame.Roles
{
    public sealed class SkillManager(FramePlayer fPlayer, ISkill skill, byte Id)
    {

        public byte Id { get; } = Id;
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
        public SkillProperties SkillProperties { get => skill.SkillProperties[Id]; }

        public int SkillsEffectSwitchId { get; set; }
        /// <summary>
        /// 是否运行
        /// </summary>
        public bool IsActive { get => ActiveRemainingTime > 0; }
        /// <summary>
        /// 是否冷却
        /// </summary>
        public bool IsBurial { get => BurialRemainingTime > 0; }
        public float ActiveRemainingTime { get; private set; }
        public float BurialRemainingTime { get; private set; }

        private CoroutineHandle coroutineHandle;


        /// <summary>
        /// 有计时任务会直接覆盖
        /// </summary>
        public void Run()
        {
            if (coroutineHandle != null)
            {
                Timing.KillCoroutines(coroutineHandle);
            }

            ActiveRemainingTime = SkillProperties.ActiveMaxTime;
            BurialRemainingTime = SkillProperties.BurialMaxTime;

            coroutineHandle = Timing.RunCoroutine(Timer());
        }

        private IEnumerator<float> Timer()
        {
            string musicFileName = SkillActiveStart?.ActiveStart(fPlayer, Id);
            if (musicFileName != null) Instance.Play(musicFileName, $"技能发动语音", new TrackEvent(), fPlayer, 10);
            while (IsActive)
            {
                ActiveRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicFileName = SkillActiveEnd?.ActiveEnd(fPlayer, Id);
            if (musicFileName != null) Instance.Play(musicFileName, $"技能结束语音", new TrackEvent(), fPlayer, 10);
            while (IsBurial)
            {
                BurialRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicFileName = SkillBurialEnd?.BurialEnd(fPlayer,Id);
            if (musicFileName != null) Instance.Play(musicFileName, $"技能准备好语音", new TrackEvent(), fPlayer, 10);
        }
    }
}
