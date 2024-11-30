using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using DG.Tweening;
using CGT.Myceliaudio;

public class MainMenu : MonoBehaviour
{
	public string NextSceneName;
	public float cubeRotateDuration;


	[Header("Buttons")]
	public Transform buttonParent;
	public GameObject startButton;
	public GameObject settingButton;
	public GameObject exitButton;
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

	private GameObject currentSelectedObject;
	private Vector3 originalButtonScale;
	private bool isButtonInteractable;

	Sequence onButtonSequence; // For button growing and shrinking

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
			.DOLocalMoveX(640, buttonMoveDuration)
			.SetEase(Ease.OutBack)
			.SetDelay(buttonMoveDelay)
			.OnComplete(EnableButtons);

		originalButtonScale = buttons[0].transform.localScale;
		settingUI.transform.localScale = Vector3.zero;

		EventSystem.current.SetSelectedGameObject(startButton);
		startButton.GetComponent<Button>().Select();
		currentSelectedObject = EventSystem.current.currentSelectedGameObject;
	}

	public void NewGame()
	{
		DOTween.Clear();
		SceneManager.LoadScene(NextSceneName);
	}

	public void MouseOnButton(Button aButton)
	{
		if (!isButtonInteractable) return;
		currentSelectedObject = aButton.gameObject;
		AudioSystem.S.Play(titleScreenSFX.hoverBtnArgs);

		onButtonSequence.Kill(); // For avoiding issue when using both mouse and gamepad
		// Setup growing and shrinking Tween
		onButtonSequence = DOTween.Sequence();
		onButtonSequence.Append(currentSelectedObject.transform.DOScale(originalButtonScale * hoverButtonRatio, buttonScaleDuration)
								.OnComplete(() =>
								{
									currentSelectedObject.transform.DOScale(originalButtonScale, buttonScaleDuration);
								}));
		onButtonSequence.SetLoops(-1, LoopType.Yoyo);
	}

	public void MouseExitButton(Button aButton)
	{
		if (!isButtonInteractable) return;
		onButtonSequence.Kill();
		aButton.transform.DOScale(originalButtonScale, 0.2f);
		currentSelectedObject = null;
	}

	public void Setting()
	{
		Debug.Log("Open Setting Page");
		onButtonSequence.Kill();
		DisenableButtons();
		MoveOutButton();
	}

	public void MoveInButton()
	{ 
		buttonParent
			.DOLocalMoveX(640, buttonMoveDuration)
			.SetEase(Ease.OutBack)
			.OnComplete(EnableButtons);
	}

	public void MoveOutButton()
	{ 
		buttonParent.
			DOLocalMoveX(1340, buttonMoveDuration).
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
		EventSystem.current.SetSelectedGameObject(startButton);
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
