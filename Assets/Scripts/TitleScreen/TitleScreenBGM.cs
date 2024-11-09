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

        AudioSystem.S.Play(args);
    }

    
}
