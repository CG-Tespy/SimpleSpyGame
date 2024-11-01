#define CGT_MYCELIAUDIO_1_00_00
using UnityEngine;
using System.Collections.Generic;

namespace CGT.Myceliaudio
{
    public class AudioSystem : MonoBehaviour
    {
        public static AudioSystem S
        {
            get
            {
                EnsureExists();
                return _s;
            }
        }

        protected static void EnsureExists()
        {
            // We check here to avoid creating craploads of AudioSyses from lots of
            // AudioCommands being executed in short order
            bool alreadySetUp = _s != null;
            if (alreadySetUp)
            {
                return;
            }

            _s = AudioSystemBuilder.BuildDefault();
        }

        protected virtual void Awake()
        {
            if (_s != null && _s != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                _s = this;
            }

            RegisterTrackManagers();
            DontDestroyOnLoad(this.gameObject);
        }

        protected static AudioSystem _s;

        protected virtual void RegisterTrackManagers()
        {
            IList<TrackManager> managersFound = GetComponentsInChildren<TrackManager>();

            foreach (TrackManager manager in managersFound)
            {
                TrackManagers[manager.Group] = manager;
            }
        }

        public IDictionary<TrackGroup, TrackManager> TrackManagers = new Dictionary<TrackGroup, TrackManager>();

        public virtual float GetTrackVol(TrackGroup trackGroup, int track = 0)
        {
            TrackManager managerToUse = TrackManagers[trackGroup];
            return managerToUse.GetVolume(track);
        }

        public virtual void SetTrackVol(AlterAudioSourceArgs args)
        {
            TrackManager managerToUse = TrackManagers[args.TrackGroup];
            managerToUse.SetTrackVolume(args);
        }

        public virtual void SetTrackVol(TrackGroup trackGroup, int track, float targVol)
        {
            AlterAudioSourceArgs args = new AlterAudioSourceArgs()
            {
                TrackGroup = trackGroup,
                Track = track,
                TargetValue = targVol
            };

            SetTrackVol(args);
        }

        public virtual float GetTrackGroupVol(TrackGroup trackGroup)
        {
            TrackManager managerToUse = TrackManagers[trackGroup];
            return managerToUse.BaseVolume;
        }

        public virtual void SetTrackGroupVol(TrackGroup trackGroup, float newVol)
        {
            TrackManager managerToUse = TrackManagers[trackGroup];
            managerToUse.BaseVolume = newVol;
        }

        public virtual void Play(PlayAudioArgs args)
        {
            var managerToInvolve = TrackManagers[args.TrackGroup];
            managerToInvolve.Play(args);
        }

        public virtual void StopPlaying(TrackGroup trackGroup, int track = 0)
        {
            TrackManager managerToUse = TrackManagers[trackGroup];
            managerToUse.Stop(track);
        }
    
        public virtual void FadeTrackVol(AlterAudioSourceArgs args)
        {
            TrackManager managerToUse = TrackManagers[args.TrackGroup];
            managerToUse.FadeTrackVolume(args);
        }

        public static string SystemSettingsFileName { get; set; } = "myceliaudioSettings.json";

    }
}