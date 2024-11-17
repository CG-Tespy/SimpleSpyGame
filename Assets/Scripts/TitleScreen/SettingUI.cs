using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using CGT.Myceliaudio;

public class SettingUI : MonoBehaviour
{
	[SerializeField] float popDuration;
	[SerializeField] MainMenu mainMenu;
	[SerializeField] Button closeButton; 

	[Header("Sliders")]
	[SerializeField] Slider masterSlider;
	[SerializeField] Slider bgmSlider; 
	[SerializeField] Slider sfxSlider;

	[Header("Text")]
	[SerializeField] TMP_Text masterValue;
	[SerializeField] TMP_Text bgmValue;
	[SerializeField] TMP_Text sfxValue;

    private void Awake()
    {
		transform.localScale = Vector3.zero;
    }

    private void Start()
	{
		masterSlider.value = AudioSystem.S.GetTrackGroupVol(TrackGroup.Master) / AudioMath.MaxVol;
		bgmSlider.value = AudioSystem.S.GetTrackGroupVol(TrackGroup.BGMusic) / AudioMath.MaxVol;
		sfxSlider.value = AudioSystem.S.GetTrackGroupVol(TrackGroup.SoundFX) / AudioMath.MaxVol;
		Debug.Log("Mater Vol: " + masterSlider.value);
		Debug.Log("BGMusic Vol: " + bgmSlider.value);
		Debug.Log("SoundFX Vol: " + sfxSlider.value);
	}

	private void Update()
	{
		AudioSystem.S.SetTrackGroupVol(TrackGroup.Master, masterSlider.value * AudioMath.MaxVol);
		AudioSystem.S.SetTrackGroupVol(TrackGroup.BGMusic, bgmSlider.value * AudioMath.MaxVol);
		AudioSystem.S.SetTrackGroupVol(TrackGroup.SoundFX, sfxSlider.value * AudioMath.MaxVol);
		masterValue.text = ((int)(masterSlider.value * AudioMath.MaxVol)).ToString() + " / 100";
		bgmValue.text = ((int)(bgmSlider.value * AudioMath.MaxVol)).ToString() + " / 100";
		sfxValue.text = ((int)(sfxSlider.value * AudioMath.MaxVol)).ToString() + " / 100";
	}

	private void OnEnable()
	{
		transform.DOScale(1, popDuration);
	}

	public void CloseWindow()
	{
		transform.DOScale(0, popDuration).OnComplete(()=>
		{
			gameObject.SetActive(false);
            mainMenu.buttonParent.
                DOLocalMoveX(200, mainMenu.buttonMoveDuration).
                SetEase(Ease.OutBack).
				OnComplete(mainMenu.EnableButtons);
        });
	}
}
