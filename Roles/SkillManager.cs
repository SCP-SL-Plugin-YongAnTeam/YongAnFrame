using MEC;
using System.Collections.Generic;
using YongAnFrame.Players;
using YongAnFrame.Roles.Interfaces;
using YongAnFrame.Roles.Properties;
using static YongAnFrame.Roles.MusicManager;

namespace YongAnFrame.Roles
{
    public class SkillManager
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
        public SkillProperties[] SkillProperties { get => skill.SkillProperties; }

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

        public SkillManager(FramePlayer fPlayer, ISkill skill)
        {
            this.fPlayer = fPlayer;
            this.skill = skill;
            coroutineHandle = new CoroutineHandle[SkillProperties.Length];
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

            ActiveRemainingTime = SkillProperties[id].ActiveMaxTime;
            BurialRemainingTime = SkillProperties[id].BurialMaxTime;

            coroutineHandle[id] = Timing.RunCoroutine(Timer(id));
        }

        private IEnumerator<float> Timer(int id)
        {
            string musicFileName = SkillActiveStart?.ActiveStart(fPlayer);
            if (musicFileName != null) Instance.Play(musicFileName, $"技能发动语音", new TrackEvent(), fPlayer, 10);
            while (IsActive)
            {
                ActiveRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicFileName = SkillActiveEnd?.ActiveEnd(fPlayer);
            if (musicFileName != null) Instance.Play(musicFileName, $"技能结束语音", new TrackEvent(), fPlayer, 10);
            while (IsBurial)
            {
                BurialRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicFileName = SkillBurialEnd?.BurialEnd(fPlayer);
            if (musicFileName != null) Instance.Play(musicFileName, $"技能准备好语音", new TrackEvent(), fPlayer, 10);
        }
    }
}
