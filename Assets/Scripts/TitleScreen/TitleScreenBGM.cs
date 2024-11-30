using CGT.Myceliaudio;
using UnityEngine;

public class TitleScreenBGM : MonoBehaviour
{
    [SerializeField] protected AudioClip _clip;

    protected virtual void Awake()
    {
        PlayAudioArgs args = new PlayAudioArgs();
        args.Clip = _clip;
        args.Loop = true;
        args.TrackGroup = TrackGroup.BGMusic;

        AudioClip currentlyPlaying = AudioSystem.S.GetClipPlayingAt(TrackGroup.BGMusic, 0);
        if (currentlyPlaying != _clip)
        {
            AudioSystem.S.StopPlaying(TrackGroup.BGMusic, 0);
            AudioSystem.S.Play(args);
        }
    }

    
}
