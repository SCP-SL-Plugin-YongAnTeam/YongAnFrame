using Exiled.API.Features;
using Exiled.API.Features.Components;
using Mirror;
using SCPSLAudioApi.AudioCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using YongAnFrame.Features.Players;
using static SCPSLAudioApi.AudioCore.AudioPlayerBase;

namespace YongAnFrame.Features
{
    /// <summary>
    /// 一个通用的音乐管理器
    /// </summary>
    public static class MusicManager
    {
        private static uint num = 0;
        /// <summary>
        /// 获取或设置放音频的玩家(NPC)
        /// </summary>
        public static Dictionary<string, ReferenceHub> MusicNpc { get; } = [];
        static MusicManager()
        {
            OnFinishedTrack += TrackFinished;
        }

        private static void TrackFinished(AudioPlayerBase playerBase, string track, bool directPlay, ref int nextQueuePos) => Stop(playerBase);

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

        /// <summary>
        /// 立刻停止播放音频
        /// </summary>
        /// <param name="playerBase">AudioPlayerBase</param>
        public static void Stop(AudioPlayerBase playerBase)
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
        /// 向所有玩家播放音频
        /// </summary>
        /// <param name="musicFile">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <returns></returns>
        public static AudioPlayerBase Play(string musicFile, string npcName) => Play(musicFile, npcName, -1);
        /// <summary>
        /// 向一名玩家播放音频
        /// </summary>
        /// <param name="musicFile">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="source">传播距离检测源头玩家(可null，null时是NPC)</param>
        /// <returns></returns>
        public static AudioPlayerBase Play(string musicFile, string npcName, FramePlayer source) => Play(musicFile, npcName, source, 0);
        /// <summary>
        /// NPC向玩家播放音频
        /// </summary>
        /// <param name="musicFile">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="distance">传播距离(-1时是全部玩家，0时是源头玩家)</param>
        /// <returns></returns>
        public static AudioPlayerBase Play(string musicFile, string npcName, float distance) => Play(musicFile, npcName, null, distance);
        /// <summary>
        /// 在多少米内向玩家播放音频
        /// </summary>
        /// <param name="musicFile">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="source">传播距离检测源头玩家(可null，null时是NPC)</param>
        /// <param name="distance">传播距离(-1时是全部玩家，0时是源头玩家)</param>
        /// <returns></returns>
        public static AudioPlayerBase Play(string musicFile, string npcName, FramePlayer source, float distance) => Play(musicFile, npcName, null, source, distance, null, false, 80, false);
        /// <summary>
        /// 播放音频
        /// </summary>
        /// <param name="musicFile">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="trackEvent">播放事件(可null)</param>
        /// <param name="source">传播距离检测源头玩家(可null，null时是NPC)</param>
        /// <param name="distance">传播距离(-1时是全部玩家，0时是源头玩家)</param>
        /// <param name="extraPlay">额外可接收音频的玩家(可null)</param>
        /// <param name="isSole">[弃用]是否覆盖播放</param>
        /// <param name="volume">音量大小</param>
        /// <param name="isLoop">是否循环</param>
        /// <returns></returns>
        public static AudioPlayerBase Play(string musicFile, string npcName, TrackEvent? trackEvent, FramePlayer source, float distance, FramePlayer[] extraPlay, bool isSole = false, float volume = 80, bool isLoop = false)
        {
            AudioPlayerBase audioPlayerBase = null;
            try
            {
                if (trackEvent.HasValue)
                {
                    OnTrackLoaded += trackEvent.Value.TrackLoaded;
                }

                ReferenceHub npc = CreateMusicNpc(npcName);
                audioPlayerBase = Get(npc);

                if (distance != -1)
                {
                    if (source != null)
                    {
                        if (distance == 0)
                        {
                            audioPlayerBase.BroadcastTo.Add(npc.PlayerId);
                        }
                        else
                        {
                            audioPlayerBase.BroadcastTo = [.. FramePlayer.List.Where(p => Vector3.Distance(p.ExPlayer.Position, source.ExPlayer.Position) <= distance).Select((s) => s.ExPlayer.Id)];
                        }
                    }

                    if (extraPlay != null)
                    {
                        foreach (var player in extraPlay)
                        {
                            if (!audioPlayerBase.BroadcastTo.Contains(player.ExPlayer.Id))
                            {
                                audioPlayerBase.BroadcastTo.Add(player.ExPlayer.Id);
                            }
                        }
                    }
                }

                audioPlayerBase.CurrentPlay = $"{PathManager.Music}/{musicFile}.ogg";
                audioPlayerBase.Volume = volume;
                audioPlayerBase.Loop = isLoop;
                audioPlayerBase.Play(-1);
            }
            catch (Exception)
            {
                Stop(audioPlayerBase);
            }
            return audioPlayerBase;
        }

        /// <summary>
        /// 播放音频(Url)
        /// </summary>
        /// <param name="musicUrl">音频文件</param>
        /// <param name="npcName">NPC名称</param>
        /// <param name="trackEvent">播放事件(可null)</param>
        /// <param name="source">传播距离检测源头玩家(可null，null时是NPC)</param>
        /// <param name="distance">传播距离(-1时是全部玩家，0时是源头玩家)</param>
        /// <param name="extraPlay">额外可接收音频的玩家(可null)</param>
        /// <param name="isSole">[弃用]是否覆盖播放</param>
        /// <param name="volume">音量大小</param>
        /// <param name="isLoop">是否循环</param>
        /// <returns></returns>
        public static AudioPlayerBase PlayUrl(string musicUrl, string npcName, TrackEvent? trackEvent, FramePlayer source, float distance, FramePlayer[] extraPlay, bool isSole = false, float volume = 80, bool isLoop = false)
        {
            AudioPlayerBase audioPlayerBase = null;
            try
            {
                if (trackEvent.HasValue)
                {
                    OnTrackLoaded += trackEvent.Value.TrackLoaded;
                }

                ReferenceHub npc = CreateMusicNpc(npcName);
                audioPlayerBase = Get(npc);

                if (distance != -1)
                {
                    if (source != null)
                    {
                        if (distance == 0)
                        {
                            audioPlayerBase.BroadcastTo.Add(npc.PlayerId);
                        }
                        else
                        {
                            audioPlayerBase.BroadcastTo = FramePlayer.List.Where(p => Vector3.Distance(p.ExPlayer.Position, source.ExPlayer.Position) <= distance).Select((s) => s.ExPlayer.Id).ToList();
                        }
                    }

                    if (extraPlay != null)
                    {
                        foreach (var player in extraPlay)
                        {
                            if (!audioPlayerBase.BroadcastTo.Contains(player.ExPlayer.Id))
                            {
                                audioPlayerBase.BroadcastTo.Add(player.ExPlayer.Id);
                            }
                        }
                    }
                }

                audioPlayerBase.CurrentPlay = musicUrl;
                audioPlayerBase.Volume = volume;
                audioPlayerBase.Loop = isLoop;
                audioPlayerBase.AllowUrl = true;
                audioPlayerBase.Play(-1);
            }
            catch (Exception)
            {
                Stop(audioPlayerBase);
            }
            return audioPlayerBase;
        }
    }
    public readonly struct TrackEvent(TrackLoaded trackLoaded)
    {
        public TrackLoaded TrackLoaded { get; } = trackLoaded;
    }
}