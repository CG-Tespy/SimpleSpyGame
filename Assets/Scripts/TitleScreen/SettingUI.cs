using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using CGT.Myceliaudio;

public class SettingUI : MonoBehaviour
{
	[SerializeField] float popDuration;
	[SerializeField] Button closeButton; 
	[SerializeField] Slider bgmSlider; 
	[SerializeField] Slider sfxSlider;
	[SerializeField] MainMenu mainMenu;

	[SerializeField] TMP_Text bgmValue;
	[SerializeField] TMP_Text sfxValue;

    private void Awake()
    {
		transform.localScale = Vector3.zero;
    }

    private void Start()
	{
		bgmSlider.value = AudioSystem.S.GetTrackVol(TrackGroup.BGMusic);
		sfxSlider.value = AudioSystem.S.GetTrackVol(TrackGroup.SoundFX);
		Debug.Log(bgmSlider.value);
		Debug.Log(sfxSlider.value);
	}

	private void Update()
	{
		AudioSystem.S.SetTrackVol(TrackGroup.BGMusic, 0, bgmSlider.value * 100);
		AudioSystem.S.SetTrackVol(TrackGroup.SoundFX, 0, sfxSlider.value * 100);
		bgmValue.text = ((int)(bgmSlider.value * 100)).ToString() + " / 100";
		sfxValue.text = ((int)(sfxSlider.value * 100)).ToString() + " / 100";
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
