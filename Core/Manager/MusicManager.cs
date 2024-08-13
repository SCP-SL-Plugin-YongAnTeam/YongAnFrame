using Exiled.API.Features;
using PlayerRoles;
using SCPSLAudioApi.AudioCore;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static SCPSLAudioApi.AudioCore.AudioPlayerBase;

namespace YongAnFrame.Core.Manager
{
    public class MusicManager
    {
        private static readonly MusicManager instance = new();
        public static MusicManager Instance => instance;
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
        private int num = 1;
        public Dictionary<string, Npc> MusicNpc { get; set; } = [];

        public void Stop(AudioPlayerBase playerBase)
        {
            if (playerBase == null) return;
            Npc npc = Npc.Get(playerBase.Owner);
            if (npc == null) return;
            playerBase.Stoptrack(true);
            MusicNpc.Remove(npc.UserId);
            npc.Destroy();
        }
        public AudioPlayerBase Play(string musicFile, string npcId, string npcName, TrackEvent trackEvent, Player source, float distance, bool isClean = false, float volume = 80, bool isLoop = false)
        {
            return Play(musicFile, npcId, npcName, trackEvent, false, 80, false, source, distance);
        }
        public AudioPlayerBase Play(string musicFile, string npcId, string npcName, TrackEvent trackEvent, bool isClean = false, float volume = 80, bool isLoop = false, Player source = null, float distance = 0)
        {
            AudioPlayerBase audioPlayerBase = null;
            try
            {
                OnTrackLoaded += trackEvent.TrackLoaded ?? trackEvent.TrackLoaded;
                if (!MusicNpc.TryGetValue(npcId, out Npc npc))
                {
                    npc = Npc.Spawn(npcName, RoleTypeId.Overwatch, 0, npcId);
                    audioPlayerBase = Get(npc.ReferenceHub);
                    MusicNpc.Add(npcId, npc);
                }
                else
                {
                    if (!isClean)
                    {
                        npc = Npc.Spawn(npcName, RoleTypeId.Overwatch, 0, num + npcId);
                        audioPlayerBase = Get(npc.ReferenceHub);
                        MusicNpc.Add(num + npcId, npc);
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

                audioPlayerBase.Enqueue(@$"{Paths.Plugins}\{Server.Port}\YongAnPluginData\{musicFile}.ogg", 0);
                audioPlayerBase.Volume = volume;
                audioPlayerBase.Loop = isLoop;
                audioPlayerBase.Play(0);
                return audioPlayerBase;
            }
            finally
            {
                Stop(audioPlayerBase);
            }
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
