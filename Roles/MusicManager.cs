using Exiled.API.Features;
using Exiled.API.Features.Components;
using Mirror;
using SCPSLAudioApi.AudioCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using YongAnFrame.Players;
using static SCPSLAudioApi.AudioCore.AudioPlayerBase;

namespace YongAnFrame.Roles
{
    /// <summary>
    /// 一个通用的音频控制器
    /// </summary>
    public sealed class MusicManager
    {
        private static readonly MusicManager instance = new();

        private int num = 1;
        public static MusicManager Instance => instance;
        /// <summary>
        /// 获取或设置放音频的玩家(NPC)
        /// </summary>
        public Dictionary<string, ReferenceHub> MusicNpc { get; } = [];
        private MusicManager() { }

        internal void Init()
        {
            OnFinishedTrack += TrackFinished;
            Log.Info("MusicManager----------OK");
        }

        private void TrackFinished(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos)
        {
            Stop(playerBase);
        }

        private ReferenceHub CreateMusicNpc(string name)
        {
            var newNpc = UnityEngine.Object.Instantiate(NetworkManager.singleton.playerPrefab);
            ReferenceHub hubNpc = newNpc.GetComponent<ReferenceHub>();
            NetworkServer.AddPlayerForConnection(new FakeConnection(0), newNpc);
            hubNpc.nicknameSync.Network_myNickSync = name;
            MusicNpc.Add(name, hubNpc);
            return hubNpc;
        }

        /// <summary>
        /// 立刻停止播放音频
        /// </summary>
        /// <param name="playerBase">AudioPlayerBase</param>
        public void Stop(AudioPlayerBase playerBase)
        {
            if (playerBase == null) return;
            ReferenceHub npc = playerBase.Owner;
            playerBase.Stoptrack(true);
            MusicNpc.Remove(npc.nicknameSync.Network_myNickSync);
            CustomNetworkManager.TypedSingleton.OnServerDisconnect(npc.connectionToClient);
            Player.Dictionary.Remove(npc.gameObject);
            UnityEngine.Object.Destroy(npc.gameObject);
        }
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="musicFile">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <returns></returns>
        public AudioPlayerBase Play(string musicFile, string npcName)
        {
            return Play(musicFile, npcName, new TrackEvent(), null, 0, [], false, 80, false);
        }
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="musicFile">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="source">传播距离检测源头玩家</param>
        /// <param name="distance">传播距离</param>
        /// <returns></returns>
        public AudioPlayerBase Play(string musicFile, string npcName, FramePlayer source, float distance)
        {
            return Play(musicFile, npcName, new TrackEvent(), source, distance, [], false, 80, false);
        }
        /// <summary>
        /// 单独给一个人播放音频
        /// </summary>
        /// <param name="musicFile">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="source">指定玩家</param>
        /// <returns></returns>
        public AudioPlayerBase Play(string musicFile, string npcName, Player source)
        {
            return Play(musicFile, npcName, new TrackEvent(), source, [], false, 80, false);
        }
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="musicFile">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="trackEvent">播放事件</param>
        /// <param name="source">传播距离检测源头玩家</param>
        /// <param name="distance">传播距离</param>
        /// <param name="extraPlay">额外可接收音频的玩家</param>
        /// <param name="isSole">是否覆盖播放</param>
        /// <param name="volume">音量大小</param>
        /// <param name="isLoop">是否循环</param>
        /// <returns></returns>
        public AudioPlayerBase Play(string musicFile, string npcName, TrackEvent trackEvent, FramePlayer source, float distance, FramePlayer[] extraPlay, bool isSole = false, float volume = 80, bool isLoop = false)
        {
            AudioPlayerBase audioPlayerBase = null;
            try
            {
                OnTrackLoaded += trackEvent.TrackLoaded;
                if (!MusicNpc.TryGetValue(npcName, out ReferenceHub npc))
                {
                    npc = CreateMusicNpc(npcName);
                    audioPlayerBase = Get(npc);
                }
                else
                {
                    if (!isSole)
                    {
                        npc = CreateMusicNpc(npcName);
                        audioPlayerBase = Get(npc);
                        MusicNpc.Add(num + npcName, npc);
                        num++;
                    }
                }

                if (extraPlay != null)
                {
                    audioPlayerBase.AudioToPlay = extraPlay.Select((s) => { return s.ExPlayer.UserId; }).ToList();
                }

                if (distance != 0)
                {
                    audioPlayerBase.AudioToPlay ??= [];
                    foreach (var player in Player.List.Where(p => Vector3.Distance(p.Position, source.ExPlayer.Position) <= distance))
                    {
                        audioPlayerBase.AudioToPlay.Add(player.UserId);
                    }
                }

                audioPlayerBase.Enqueue($"{Paths.Plugins}/{Server.Port}/YongAnPluginData/{musicFile}.ogg", 0);
                audioPlayerBase.Volume = volume;
                audioPlayerBase.Loop = isLoop;
                audioPlayerBase.Play(0);
            }
            catch (Exception)
            {
                Stop(audioPlayerBase);
            }
            return audioPlayerBase;
        }
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="musicFile">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="trackEvent">播放事件</param>
        /// <param name="source">传播距离检测源头玩家</param>
        /// <param name="extraPlay">额外可接收音频的玩家</param>
        /// <param name="isSole">是否覆盖播放</param>
        /// <param name="volume">音量大小</param>
        /// <param name="isLoop">是否循环</param>
        /// <returns></returns>
        public AudioPlayerBase Play(string musicFile, string npcName, TrackEvent trackEvent, FramePlayer source, FramePlayer[] extraPlay, bool isSole = false, float volume = 80, bool isLoop = false)
        {
            AudioPlayerBase audioPlayerBase = null;
            try
            {
                OnTrackLoaded += trackEvent.TrackLoaded;
                if (!MusicNpc.TryGetValue(npcName, out ReferenceHub npc))
                {
                    npc = CreateMusicNpc(npcName);
                    audioPlayerBase = Get(npc);
                }
                else
                {
                    if (!isSole)
                    {
                        npc = CreateMusicNpc(npcName);
                        audioPlayerBase = Get(npc);
                        MusicNpc.Add(num + npcName, npc);
                        num++;
                    }
                }

                if (extraPlay != null)
                {
                    audioPlayerBase.AudioToPlay = extraPlay.Select((s) => { return s.ExPlayer.UserId; }).ToList();
                }

                audioPlayerBase.AudioToPlay.Add(source.UserId);

                audioPlayerBase.Enqueue($"{Paths.Plugins}/{Server.Port}/YongAnPluginData/{musicFile}.ogg", 0);
                audioPlayerBase.Volume = volume;
                audioPlayerBase.Loop = isLoop;
                audioPlayerBase.Play(0);
            }
            catch (Exception)
            {
                Stop(audioPlayerBase);
            }
            return audioPlayerBase;
        }
        
        public readonly struct TrackEvent(TrackLoaded trackLoaded)
        {
            public TrackLoaded TrackLoaded { get; } = trackLoaded;
        }
    }
}
