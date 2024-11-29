using CGT.Myceliaudio;
using UnityEngine;

namespace SimpleSpyGame
{
    public class StageAudio : MonoBehaviour
    {
        [SerializeField] protected AudioClip _bgm, _victoryJingle, _failureJingle;

        protected virtual void Awake()
        {
            PrepAudioArgs();
            AudioSystem.S.Play(_bgmArgs);
        }

        protected virtual void PrepAudioArgs()
        {
            _bgmArgs = new PlayAudioArgs()
            {
                Clip = _bgm,
                TrackGroup = TrackGroup.BGMusic,
                Loop = true,
            };

            _victoryJingleArgs = new PlayAudioArgs()
            {
                Clip = _victoryJingle,
                TrackGroup = TrackGroup.SoundFX,
            };

            _failureJingleArgs = new PlayAudioArgs()
            {
                Clip = _failureJingle,
                TrackGroup = TrackGroup.SoundFX,
            };
        }

        PlayAudioArgs _bgmArgs, _victoryJingleArgs, _failureJingleArgs;

        protected virtual void OnEnable()
        {
            StageEvents.PlayerWon += OnPlayerWon;
            StageEvents.PlayerLost += OnPlayerLost;
        }

        protected virtual void OnPlayerWon()
        {
            AudioSystem.S.Play(_victoryJingleArgs);
        }

        protected virtual void OnPlayerLost()
        {
            AudioSystem.S.Play(_failureJingleArgs);
        }

        protected virtual void OnDisable()
        {
            StageEvents.PlayerWon -= OnPlayerWon;
            StageEvents.PlayerLost -= OnPlayerLost;
        }
    }
}