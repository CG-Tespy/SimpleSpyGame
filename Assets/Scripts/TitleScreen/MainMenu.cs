using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;
using CGT.Myceliaudio;

public class MainMenu : MonoBehaviour
{
	public string NextSceneName;
	public float cubeRotateDuration;


	[Header("Buttons")]
	public Transform buttonParent;
	public Button startButton;
	public Button settingButton;
	public Button exitButton;
	public Button[] buttons;

	[Header("Button Variable")]
	public float hoverButtonRatio;
	public float buttonMoveDuration;
	public float buttonMoveDelay;
	public float buttonScaleDuration;

	[Header("")]
	[SerializeField] TitleScreenSFX titleScreenSFX;
	[SerializeField] Transform cube;
	[SerializeField] SettingUI settingUI;


	private Vector3 originalButtonScale;
	private bool isButtonInteractable;

    Sequence onButtonSequence;

    private void Awake()
	{
		foreach (Button btn in buttons)
		{
			btn.enabled = false;
		}
	}

	private void Start()
	{
		cube.DORotate(new Vector3(360, 360, 0), cubeRotateDuration, RotateMode.FastBeyond360)
			.SetLoops(-1, LoopType.Restart)
			.SetEase(Ease.Linear);

		buttonParent
			.DOLocalMoveX(200, buttonMoveDuration)
			.SetEase(Ease.OutBack)
			.SetDelay(buttonMoveDelay)
			.OnComplete(EnableButtons);

		originalButtonScale = buttons[0].transform.localScale;
		settingUI.transform.localScale = Vector3.zero;
	}

	public void NewGame()
	{
		DOTween.Clear();
		SceneManager.LoadScene(NextSceneName);
	}

	public void MouseOnButton(Button aButton)
	{
		if (!isButtonInteractable) return;

		AudioSystem.S.Play(titleScreenSFX.hoverBtnArgs);

		// Setup growing and shrinking Tween
		onButtonSequence = DOTween.Sequence();
		onButtonSequence.Append(aButton.transform.DOScale(originalButtonScale * hoverButtonRatio, buttonScaleDuration)
								.OnComplete(() => {
									aButton.transform.DOScale(originalButtonScale, buttonScaleDuration);
								}));
		onButtonSequence.SetLoops(-1, LoopType.Yoyo);
	}

	public void MouseExitButton(Button aButton)
	{
		if (!isButtonInteractable) return;
		onButtonSequence.Kill();
		aButton.transform.DOScale(originalButtonScale, 0.2f);
	}

	public void Setting()
	{
		Debug.Log("Open Setting Page");
		onButtonSequence.Kill();
		DisenableButtons();
		buttonParent.
			DOLocalMoveX(500, buttonMoveDuration).
			SetEase(Ease.InBack).
			OnComplete(()=> {
                settingUI.gameObject.SetActive(true);
                settingButton.transform.DOScale(originalButtonScale, 0.2f);
            });
	}

	public void ExitGame()
	{
		Application.Quit();
	}

	public void EnableButtons()
	{
		isButtonInteractable = true;
		foreach (Button btn in buttons)
		{
			btn.enabled = true;
		}
	}

	public void DisenableButtons()
	{
		isButtonInteractable = false;
		foreach (Button btn in buttons)
		{
			btn.enabled = false;
		}
	}
}
