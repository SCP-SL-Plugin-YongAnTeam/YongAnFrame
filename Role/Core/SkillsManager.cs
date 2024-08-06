using MEC;
using System.Collections.Generic;
using YongAnFrame.Core.Data;
using YongAnFrame.Role.Core.Interfaces;
using static YongAnFrame.Core.MusicManager;

namespace YongAnFrame.Role.Core
{
    public class SkillsManager
    {
        private readonly FramePlayer fPlayer;

        private readonly ISkill skill;
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
        public SkillsAttributes[] SkillsAttributes { get => skill.SkillsAttributes; }

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

        private CoroutineHandle[] coroutineHandle;

        public SkillsManager(FramePlayer fPlayer, ISkill skill)
        {
            this.fPlayer = fPlayer;
            this.skill = skill;
            coroutineHandle = new CoroutineHandle[SkillsAttributes.Length];
        }


        /// <summary>
        /// 有计时任务会直接覆盖
        /// </summary>
        public void Run(int id)
        {
            if (coroutineHandle != null)
            {
                Timing.KillCoroutines(coroutineHandle[id]);
                coroutineHandle = null;
            }

            ActiveRemainingTime = SkillsAttributes[id].ActiveMaxTime;
            BurialRemainingTime = SkillsAttributes[id].BurialMaxTime;

            coroutineHandle[id] = Timing.RunCoroutine(Timer(id));
        }

        private IEnumerator<float> Timer(int id)
        {
            string musicFileName = SkillActiveStart?.ActiveStart(fPlayer);
            if (musicFileName != null) Instance.Play(musicFileName, "Skills@localhost", $"技能发动语音", new TrackEvent(), fPlayer, 10);
            while (IsActive)
            {
                ActiveRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicFileName = SkillActiveEnd?.ActiveEnd(fPlayer);
            if (musicFileName != null) Instance.Play(musicFileName, "Skills@localhost", $"技能结束语音", new TrackEvent(), fPlayer, 10);
            while (IsBurial)
            {
                BurialRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicFileName = SkillBurialEnd?.BurialEnd(fPlayer);
            if (musicFileName != null) Instance.Play(musicFileName, "Skills@localhost", $"技能准备好语音", new TrackEvent(), fPlayer, 10);
        }
    }
}
