using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
	public string NextSceneName;
	public float rotatePeriod;
	public Transform buttonParent;
	public Button startButton;
	public Button settingButton;
	public Button exitButton;
	public Button[] buttons;

	public float hoverButtonRatio;
	public float buttonMoveDuration;
	public float buttonMoveDelay;
	[SerializeField] Transform cube;
	[SerializeField] SettingUI settingUI;


	private Vector3 originalButtonScale;
	private bool isButtonInteractable;

	private void Awake()
	{
		foreach (Button btn in buttons)
		{
			btn.enabled = false;
		}
	}

	private void Start()
	{
		cube.DORotate(new Vector3(360, 360, 0), rotatePeriod, RotateMode.FastBeyond360)
			.SetLoops(-1, LoopType.Restart)
			.SetEase(Ease.Linear);

		buttonParent
			.DOLocalMoveX(200, buttonMoveDuration)
			.SetEase(Ease.OutBack)
			.SetDelay(buttonMoveDelay)
			.OnComplete(EnableButtons);

		originalButtonScale = buttons[0].transform.localScale;
	}

	public void NewGame()
	{
		DOTween.Clear();
		SceneManager.LoadScene(NextSceneName);
	}

	public void MouseOnButton(Button aButton)
	{
		if (!isButtonInteractable) return;
		aButton.transform.DOScale(aButton.transform.localScale * hoverButtonRatio, 0.2f);
	}

	public void MouseExitButton(Button aButton)
	{
		if (!isButtonInteractable) return;
		aButton.transform.DOScale(originalButtonScale, 0.2f);
	}

	public void Setting()
	{
		Debug.Log("Open Setting Page");
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
