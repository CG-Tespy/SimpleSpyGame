using UnityEngine;
using CGT.Myceliaudio;

public class TitleScreenSFX : MonoBehaviour
{
    [SerializeField] protected AudioClip hoverBtnClip;
    public PlayAudioArgs hoverBtnArgs;

    protected virtual void Awake()
    {
        hoverBtnArgs = new PlayAudioArgs();
        hoverBtnArgs.Clip = hoverBtnClip;
        hoverBtnArgs.Loop = false;
        hoverBtnArgs.TrackGroup = TrackGroup.SoundFX;
    }
}
