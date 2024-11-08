using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public string NextSceneName;
    public float rotatePeriod;
    public Transform buttonParent;
    public Button[] buttons;
    public float hoverButtonRatio;
    public float buttonMoveDuration;
    public float buttonMoveDelay;
    [SerializeField] Transform cube;

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
        cube.DORotate(new Vector3(360, 360, 0), rotatePeriod, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        buttonParent.DOLocalMoveX(200, buttonMoveDuration).SetEase(Ease.OutBack).SetDelay(buttonMoveDelay).OnComplete(()=>
        {
            EnableButtons();
        });

        originalButtonScale = buttons[0].transform.localScale;
    }

    public void NewGame()
    {
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
	}

    public void ExitGame()
    {
        Application.Quit();
	}

    private void EnableButtons()
    {
        isButtonInteractable = true;
        foreach (Button btn in buttons)
        {
            btn.enabled = true;
        }
    }
}
