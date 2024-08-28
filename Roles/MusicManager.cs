using Exiled.API.Features;
using Exiled.API.Features.Components;
using Mirror;
using SCPSLAudioApi.AudioCore;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SCPSLAudioApi.AudioCore.AudioPlayerBase;

namespace YongAnFrame.Players
{
    public class MusicManager
    {
        private static readonly MusicManager instance = new();

        private int num = 1;
        public static MusicManager Instance => instance;
        public Dictionary<string, ReferenceHub> MusicNpc { get; set; } = [];
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
        public AudioPlayerBase Play(string musicFile, string npcName, TrackEvent trackEvent, Player source, float distance, bool isSole = false, float volume = 80, bool isLoop = false)
        {
            return Play(musicFile, npcName, trackEvent, false, 80, false, source, distance);
        }
        public AudioPlayerBase Play(string musicFile, string npcName, TrackEvent trackEvent, bool isSole = false, float volume = 80, bool isLoop = false, Player source = null, float distance = 0)
        {
            AudioPlayerBase audioPlayerBase = null;
            try
            {
                OnTrackLoaded += trackEvent.TrackLoaded ?? trackEvent.TrackLoaded;
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

                if (distance > 0)
                {
                    audioPlayerBase.AudioToPlay = [source.UserId];
                }
                else if (distance != 0)
                {
                    List<string> playerListId = [];
                    foreach (var player in Player.List.Where(p => Vector3.Distance(p.Position, source.Position) <= distance))
                    {
                        playerListId.Add(player.UserId);
                    }
                    audioPlayerBase.AudioToPlay = playerListId;
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

        public readonly struct TrackEvent
        {
            public TrackEvent(TrackLoaded trackLoaded)
            {
                TrackLoaded = trackLoaded;
            }
            public TrackLoaded TrackLoaded { get; }
        }
    }
}
