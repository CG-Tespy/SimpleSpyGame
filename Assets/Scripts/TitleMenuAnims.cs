using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SimpleSpyGame
{
    public class TitleMenuAnims : MonoBehaviour
    {
        [SerializeField] protected float cubeRotateDuration = 3f;

        [Header("Buttons")]
        public Transform buttonParent;
        public GameObject startButton;
        public GameObject settingButton;
        public GameObject exitButton;
        public Button[] buttons;
        [SerializeField] protected Image _fadeScreen;
        [SerializeField] protected float _fadeOutDur = 2f;

        [Header("Button Variable")]
        public float hoverButtonRatio = 1.2f;
        public float buttonMoveDuration = 0.7f;
        public float buttonMoveDelay = 2f;
        public float buttonScaleDuration = 0.3f;

        [Header("")]
        [SerializeField] Transform cube;
        [SerializeField] SettingUI settingUI;

        protected GameObject currentSelectedObject;
        protected Vector3 originalButtonScale;
        protected bool isButtonInteractable;

        [SerializeField] [NaughtyAttributes.Scene] protected string _titleScene;

        Sequence _buttonAnimSeq; // For button growing and shrinking

        protected virtual void Awake()
        {
            // Turns out that both Awake and Start can be called more than once for the same MonoBehaviour.
            // Every time a new scene loads, even if that MonoBehaviour instance already called Awake or Start,
            // those funcs just get called again. Hence this scene-check
            if (SceneManager.GetActiveScene().name == _titleScene)
            {
                foreach (Button btn in buttons)
                {
                    btn.enabled = false;
                }
            }
        }

        protected virtual void Start()
        {
            if (SceneManager.GetActiveScene().name == _titleScene)
            {
                InitAnims();
            }
        }

        public virtual void InitAnims()
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

        public void OnPointerEnter(Button aButton)
        {
            if (!isButtonInteractable) return;
            currentSelectedObject = aButton.gameObject;

            _buttonAnimSeq.Kill(); // For avoiding issue when using both mouse and gamepad
                                     // Setup growing and shrinking Tween
            _buttonAnimSeq = DOTween.Sequence();

            Transform selectedTrans = currentSelectedObject.transform;
            _buttonAnimSeq.Append(selectedTrans.DOScale(originalButtonScale * hoverButtonRatio,buttonScaleDuration)
                                    .OnComplete(() =>
                                    {
                                        selectedTrans.DOScale(originalButtonScale, buttonScaleDuration);
                                    }));
            _buttonAnimSeq.SetLoops(-1, LoopType.Yoyo);
        }

        public void OnPointerExit(Button aButton)
        {
            if (!isButtonInteractable) return;
            _buttonAnimSeq.Kill();
            aButton.transform.DOScale(originalButtonScale, 0.2f);
            currentSelectedObject = null;
        }

        public void Setting()
        {
            Debug.Log("Open Setting Page");
            _buttonAnimSeq.Kill();
            DISableButtons();
            MoveOutButton();
            CallSettingsScreen();
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
            _buttonAnimSeq.Kill();
            buttonParent.
                DOLocalMoveX(1340, buttonMoveDuration).
                SetEase(Ease.InBack);
        }

        public virtual void CallSettingsScreen(float delay = 1f)
        {
            CancelInvoke();
            if (delay <= 0)
            {
                ActivateSettingsScreen();
            }
            else
            {
                Invoke(nameof(ActivateSettingsScreen), delay);
            }
        }

        protected virtual void ActivateSettingsScreen()
        {
            settingUI.gameObject.SetActive(true);
            settingButton.transform.DOScale(originalButtonScale, 0.2f);
        }

        public void ExitGame()
        {
            DISableButtons();
            _fadeScreen.DOFade(1, _fadeOutDur)
                .OnComplete(OnDoneFadingOutForGameExit);
        }

        protected virtual void OnDoneFadingOutForGameExit()
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

        public void DISableButtons()
        {
            isButtonInteractable = false;
            foreach (Button btn in buttons)
            {
                btn.enabled = false;
            }
        }

        protected virtual void OnEnable()
        {
            SystemEvents.MoveToNextLevelStart += OnMoveToNextLevelStart;
        }

        protected virtual void OnMoveToNextLevelStart()
        {
            _buttonAnimSeq.Kill();

            foreach (var buttonEl in buttons)
            {
                buttonEl.transform.DOScale(originalButtonScale, 0.2f);
            }

            DISableButtons();
        }

        protected virtual void OnDisable()
        {
            SystemEvents.MoveToNextLevelStart -= OnMoveToNextLevelStart;
        }
    }
}