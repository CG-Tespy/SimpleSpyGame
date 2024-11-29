using CGT.Myceliaudio;
using UnityEngine;

namespace SimpleSpyGame
{
    public class StageMusic : MonoBehaviour
    {
        [SerializeField] protected AudioClip _bgm;

        protected virtual void Awake()
        {
            PlayAudioArgs args = new PlayAudioArgs()
            {
                Clip = _bgm,
                TrackGroup = TrackGroup.BGMusic,
                Loop = true,
            };

            AudioSystem.S.Play(args);
        }
    }
}