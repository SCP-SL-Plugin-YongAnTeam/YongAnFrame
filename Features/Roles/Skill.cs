using MEC;
using System.Collections.Generic;
using YongAnFrame.Features.Players;
using YongAnFrame.Features.Roles.Properties;
using YongAnFrame.Features.UI.Enums;
using YongAnFrame.Features.UI.Texts;

namespace YongAnFrame.Features.Roles
{
    /// <summary>
    /// 玩家的自定义角色技能
    /// </summary>
    /// <param name="fPlayer">框架玩家</param>
    /// <param name="properties">技能原始属性</param>
    public class Skill(FramePlayer fPlayer, SkillProperties properties)
    {
        /// <summary>
        /// 激活开始
        /// </summary>
        /// <returns>播放音乐文件名称</returns>
        public delegate string ActiveStart(FramePlayer fPlayer);
        /// <summary>
        /// 激活结束
        /// </summary>
        /// <returns>播放音乐文件名称</returns>
        public delegate string ActiveEnd(FramePlayer fPlayer);
        /// <summary>
        /// 冷却结束
        /// </summary>
        /// <returns>播放音乐文件名称</returns>
        public delegate string BurialEnd(FramePlayer fPlayer);
        /// <summary>
        /// 获取原始属性
        /// </summary>
        public SkillProperties Properties { get; } = properties;
        /// <summary>
        /// 获取名称
        /// </summary>
        public string Name { get; set; } = properties.Name;
        /// <summary>
        /// 获取绑定物品
        /// </summary>
        public ItemType UseItem => Properties.UseItem;
        /// <summary>
        /// 获取发动描述(可null)
        /// </summary>
        public string Statement { get; set; } = properties.Statement;
        /// <summary>
        /// 获取介绍
        /// </summary>
        public string Description { get; set; } = properties.Description;
        /// <summary>
        /// 获取最大作用时间
        /// </summary>
        public float ActiveMaxTime { get; set; } = properties.ActiveMaxTime;
        /// <summary>
        /// 获取最大冷却时间
        /// </summary>
        public float BurialMaxTime { get; set; } = properties.BurialMaxTime;
        /// <summary>
        /// 获取激活开始委托
        /// </summary>
        public ActiveStart ActiveStartAction { get; set; } = properties.ActiveStartAction;
        /// <summary>
        /// 获取激活结束委托
        /// </summary>
        public ActiveEnd ActiveEndAction { get; set; } = properties.ActiveEndAction;
        /// <summary>
        /// 获取冷却结束委托
        /// </summary>
        public BurialEnd BurialEndAction { get; set; } = properties.BurialEndAction;
        /// <summary>
        /// 获取是否激活
        /// </summary>
        public bool IsActive { get => ActiveRemainingTime > 0; }
        /// <summary>
        /// 获取是否冷却
        /// </summary>
        public bool IsBurial { get => BurialRemainingTime > 0; }
        /// <summary>
        /// 获取行动时间
        /// </summary>
        public float ActiveRemainingTime { get; private set; }
        /// <summary>
        /// 获取冷却时间
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

            ActiveRemainingTime = ActiveMaxTime;
            BurialRemainingTime = BurialMaxTime;

            coroutineHandle = Timing.RunCoroutine(Timer());
            fPlayer.UI.MessageList.Add(new MessageText($"{(string.IsNullOrEmpty(Statement) ? $"技能[{Name}]已经发动" : $"{Name}:{Statement}")}（持续时间：{ActiveMaxTime}）", ActiveMaxTime, MessageType.System));
        }

        /// <summary>
        /// 还原技能
        /// </summary>
        public void Restore()
        {
            Name = Properties.Name;
            Statement = Properties.Statement;
            Description = Properties.Description;
            ActiveMaxTime = Properties.ActiveMaxTime;
            BurialMaxTime = Properties.BurialMaxTime;
            ActiveStartAction = Properties.ActiveStartAction;
            ActiveEndAction = Properties.ActiveEndAction;
            BurialEndAction = Properties.BurialEndAction;
            if (coroutineHandle.IsValid)
            {
                Timing.KillCoroutines(coroutineHandle);
            }
            fPlayer.UI.MessageList.Add(new MessageText($"技能[{Name}]被其他人影响，技能信息全部重置", 10, MessageType.System));
        }

        private IEnumerator<float> Timer()
        {
            string? musicNameName = ActiveStartAction?.Invoke(fPlayer);
            if (musicNameName is not null) MusicManager.Play(musicNameName, $"技能发动语音", fPlayer, 10);
            while (IsActive)
            {
                ActiveRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicNameName = ActiveEndAction?.Invoke(fPlayer);
            if (musicNameName is not null) MusicManager.Play(musicNameName, $"技能结束语音", fPlayer, 10);
            while (IsBurial)
            {
                BurialRemainingTime--;
                yield return Timing.WaitForSeconds(1f);
            }
            musicNameName = BurialEndAction?.Invoke(fPlayer);
            if (musicNameName is not null) MusicManager.Play(musicNameName, $"技能准备好语音", fPlayer, 10);
        }
    }
}
