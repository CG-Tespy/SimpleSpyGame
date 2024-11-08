using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public string NextSceneName;
    public float rotatePeriod;
    public Button startButton; 
    public Button settingButton; 
    public Button exitButton;
    public float hoverButtonRatio;
    [SerializeField] Transform cube;

    private Vector3 originalButtonScale;
    
    private void Start()
    {
        cube.DORotate(new Vector3(360, 360, 0), rotatePeriod, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
        originalButtonScale = startButton.transform.localScale;
    }

    public void NewGame()
    {
        SceneManager.LoadScene(NextSceneName);
	}

    public void MouseOnButton(Button aButton)
    {
        aButton.transform.DOScale(aButton.transform.localScale * hoverButtonRatio, 0.2f);
	}

    public void MouseExitButton(Button aButton)
    {
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
}
