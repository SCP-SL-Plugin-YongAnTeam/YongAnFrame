using AudioApi.AudioCore;
using AudioApi.AudioCore.EventArgs.Voice;
using Exiled.API.Features;
using Exiled.API.Features.Components;
using Mirror;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YongAnFrame.Features.Players;
using static YongAnFrame.Features.TrackEvent;

namespace YongAnFrame.Features
{
    /// <summary>
    /// 音乐管理器
    /// </summary>
    public static class MusicManager
    {
        private static uint num = 0;
        /// <summary>
        /// 获取放音频的玩家(NPC)
        /// </summary>
        public static Dictionary<string, ReferenceHub> MusicNpc { get; } = [];
        private static readonly Dictionary<VoicePlayerBase, TrackEvent> trackEventDic = [];

        static MusicManager()
        {
            VoicePlayerBase.OnTrackLoaded += TrackLoaded;
            VoicePlayerBase.OnFinishedTrack += FinishedTrack;
        }

        private static void TrackLoaded(TrackLoadedEventArgs args)
        {
            if (trackEventDic.TryGetValue(args.VoicePlayerBase, out TrackEvent trackEvent))
            {
                 trackEvent.PlayMusicAction?.Invoke(args);
            }
        }

        private static void FinishedTrack(TrackFinishedEventArgs args)
        {
            if (trackEventDic.TryGetValue(args.VoicePlayerBase, out TrackEvent trackEvent))
            {
                trackEvent.StopMusicAction?.Invoke(args);
            }
            
        }

        private static ReferenceHub CreateMusicNpc(string name)
        {
            var newNpc = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            ReferenceHub hubNpc = newNpc.GetComponent<ReferenceHub>();
            NetworkServer.AddPlayerForConnection(new FakeConnection(0), newNpc);
            hubNpc.nicknameSync.Network_myNickSync = name;
            hubNpc.authManager.NetworkSyncedUserId = null;
            MusicNpc.Add($"{num++}:{name}", hubNpc);
            return hubNpc;
        }

        private static void KillMusicNpc(VoicePlayerBase playerBase)
        {
            if (playerBase is null) return;
            ReferenceHub npc = playerBase.Owner;
            MusicNpc.Remove(npc.nicknameSync.Network_myNickSync);
            CustomNetworkManager.TypedSingleton.OnServerDisconnect(npc.connectionToClient);
            Player.Dictionary.Remove(npc.gameObject);
            UnityEngine.Object.Destroy(npc.gameObject);
        }

        /// <summary>
        /// 立刻停止播放音频
        /// </summary>
        /// <param name="playerBase">voicePlayerBase</param>
        public static void Stop(VoicePlayerBase playerBase)
        {
            if (playerBase is null) return;
            playerBase.Stoptrack(true);
            KillMusicNpc(playerBase);
        }
        /// <summary>
        /// 向所有玩家播放音频
        /// </summary>
        /// <param name="musicName">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <returns></returns>
        public static VoicePlayerBase Play(string musicName, string npcName) => Play(musicName, npcName, -1);
        /// <summary>
        /// 向玩家(<paramref name="source"/>)播放音频
        /// </summary>
        /// <param name="musicName">音频文件/Url</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="source">传播距离检测源头玩家(可null，null时是NPC)</param>
        /// <returns></returns>
        public static VoicePlayerBase Play(string musicName, string npcName, FramePlayer source) => Play(musicName, npcName, source, 0);
        /// <summary>
        /// NPC在<paramref name="distance"/>米内向玩家播放音频
        /// </summary>
        /// <param name="musicName">音频文件/Url</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="distance">传播距离(-1时是全部玩家，0时是源头玩家)</param>
        /// <returns></returns>
        public static VoicePlayerBase Play(string musicName, string npcName, float distance) => Play(musicName, npcName, null, distance);
        /// <summary>
        /// 在<paramref name="distance"/>米内向玩家播放音频
        /// </summary>
        /// <param name="musicName">音频文件/Url</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="source">传播距离检测源头玩家(可null，null时是NPC)</param>
        /// <param name="distance">传播距离(-1时是全部玩家，0时是源头玩家)</param>
        /// <returns></returns>
        public static VoicePlayerBase Play(string musicName, string npcName, FramePlayer? source, float distance) => Play(musicName, npcName, null, source, distance, null, 80, false);
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="musicName">音频文件/Url</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="trackEvent">播放事件(可null)</param> 
        /// <param name="source">传播距离检测源头玩家(可null，null时是NPC)</param>
        /// <param name="distance">传播距离(-1时是全部玩家，0时是源头玩家)</param>
        /// <param name="extraPlay">额外可接收音频的玩家(可null)</param>
        /// <param name="volume">音量大小</param>
        /// <param name="isLoop">是否循环</param>
        /// <returns></returns>
        public static VoicePlayerBase Play(string musicName, string npcName, TrackEvent? trackEvent, FramePlayer? source, float distance, FramePlayer[]? extraPlay, float volume = 80, bool isLoop = false)
        {
            VoicePlayerBase? voicePlayerBase = null;
            ReferenceHub npc = CreateMusicNpc(npcName);
            voicePlayerBase = VoicePlayerBase.Get(npc);

            try
            {
                if (trackEvent is not null)
                {
                    if (trackEventDic.ContainsKey(voicePlayerBase))
                    {
                        trackEventDic[voicePlayerBase] = (TrackEvent)trackEvent;
                    }
                    else
                    {
                        trackEventDic.Add(voicePlayerBase, (TrackEvent)trackEvent);
                    }
                }

                if (distance != -1)
                {
                    if (source is not null)
                    {
                        if (distance == 0)
                        {
                            voicePlayerBase.BroadcastTo.Add(npc.PlayerId);
                        }
                        else
                        {
                            voicePlayerBase.BroadcastTo = [.. FramePlayer.List.Where(p => Vector3.Distance(p.ExPlayer!.Position, source.ExPlayer!.Position) <= distance).Select((s) => s.ExPlayer!.Id)];
                        }
                    }

                    if (extraPlay is not null)
                    {
                        foreach (var player in extraPlay)
                        {
                            if (!voicePlayerBase.BroadcastTo.Contains(player.ExPlayer.Id))
                            {
                                voicePlayerBase.BroadcastTo.Add(player.ExPlayer.Id);
                            }
                        }
                    }
                }

                voicePlayerBase.CurrentPlay = $"{PathManager.Music}/{musicName}.ogg";
                voicePlayerBase.Volume = volume;
                voicePlayerBase.Loop = isLoop;
                voicePlayerBase.Play(-1);
            }
            catch (Exception)
            {
                Stop(voicePlayerBase);
            }
            return voicePlayerBase;
        }
    }
    /// <summary>
    /// 音轨事件
    /// </summary>
    /// <param name="playMusic">播放音频委托</param>
    /// <param name="stopMusic">停止音频委托</param>
    public readonly struct TrackEvent(PlayMusic? playMusic, StopMusic? stopMusic)
    {
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="args"><seealso cref="TrackFinishedEventArgs"/></param>
        public delegate void PlayMusic(TrackLoadedEventArgs args);
        /// <summary>
        /// 停止音频
        /// </summary>
        /// <param name="args"><seealso cref="TrackFinishedEventArgs"/></param>
        public delegate void StopMusic(TrackFinishedEventArgs args);
        /// <summary>
        /// 获取播放音频委托
        /// </summary>
        public PlayMusic? PlayMusicAction { get; } = playMusic;
        /// <summary>
        /// 获取停止音频委托
        /// </summary>
        public StopMusic? StopMusicAction { get; } = stopMusic;
    }
}