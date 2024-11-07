using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour
{
    public string NextSceneName;
    public float rotatePeriod;
    [SerializeField] Transform cube;

    private void Start()
    {
        cube.DORotate(new Vector3(0, 360, 0), rotatePeriod, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart).SetEase(Ease.Linear);
    }

    public void NewGame()
    {
        SceneManager.LoadScene(NextSceneName);
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
