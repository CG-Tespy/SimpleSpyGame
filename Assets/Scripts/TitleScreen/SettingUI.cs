using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using CGT.Myceliaudio;
using UnityEngine.EventSystems;

public class SettingUI : MonoBehaviour
{
	[SerializeField] float popDuration;
	[SerializeField] MainMenu mainMenu;
	[SerializeField] Button closeButton; 

	[Header("Sliders")]
	[SerializeField] GameObject masterSlider;
	[SerializeField] GameObject bgmSlider; 
	[SerializeField] GameObject sfxSlider;
	private Slider mSlider;
	private Slider bSlider;
	private Slider sSlider;

	[Header("Text")]
	[SerializeField] TMP_Text masterValue;
	[SerializeField] TMP_Text bgmValue;
	[SerializeField] TMP_Text sfxValue;

    private void Awake()
    {
		transform.localScale = Vector3.zero;
		mSlider = masterSlider.GetComponent<Slider>(); 
		bSlider = bgmSlider.GetComponent<Slider>(); 
		sSlider = sfxSlider.GetComponent<Slider>(); 
    }

	private void OnEnable()
	{
		transform.DOScale(1, popDuration);
		EventSystem.current.SetSelectedGameObject(masterSlider);
	}

    private void Start()
	{
		mSlider.value = AudioSystem.S.GetTrackGroupVol(TrackGroup.Master) / AudioMath.MaxVol;
		bSlider.value = AudioSystem.S.GetTrackGroupVol(TrackGroup.BGMusic) / AudioMath.MaxVol;
		sSlider.value = AudioSystem.S.GetTrackGroupVol(TrackGroup.SoundFX) / AudioMath.MaxVol;
		Debug.Log("Mater Vol: " + mSlider.value);
		Debug.Log("BGMusic Vol: " + bSlider.value);
		Debug.Log("SoundFX Vol: " + sSlider.value);
	}

	private void Update()
	{
		AudioSystem.S.SetTrackGroupVol(TrackGroup.Master, mSlider.value * AudioMath.MaxVol);
		AudioSystem.S.SetTrackGroupVol(TrackGroup.BGMusic, bSlider.value * AudioMath.MaxVol);
		AudioSystem.S.SetTrackGroupVol(TrackGroup.SoundFX, sSlider.value * AudioMath.MaxVol);
		masterValue.text = ((int)(mSlider.value * AudioMath.MaxVol)).ToString() + " / 100";
		bgmValue.text = ((int)(bSlider.value * AudioMath.MaxVol)).ToString() + " / 100";
		sfxValue.text = ((int)(sSlider.value * AudioMath.MaxVol)).ToString() + " / 100";
	}

	public void CloseWindow()
	{
		transform.DOScale(0, popDuration).OnComplete(()=>
		{
			gameObject.SetActive(false);
			mainMenu.MoveInButton();
			EventSystem.current.SetSelectedGameObject(mainMenu.settingButton);
        });
	}
}
