using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class SettingUI : MonoBehaviour
{
	[SerializeField] float popDuration;
	[SerializeField] Button closeButton; 
	[SerializeField] Slider bgmSlider; 
	[SerializeField] Slider sfxSlider;

	[SerializeField] AudioSource bgmSource;
	[SerializeField] AudioSource sfxSource;

	[SerializeField] TMP_Text bgmValue;
	[SerializeField] TMP_Text sfxValue;

	private void Start()
	{
		bgmSlider.value = bgmSource.volume;
		sfxSlider.value = sfxSource.volume;
	}

	private void Update()
	{
		bgmSource.volume = bgmSlider.value;
		sfxSource.volume = sfxSlider.value;
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
		});
	}
}
