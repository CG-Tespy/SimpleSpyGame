using CGT.Myceliaudio;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
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

        Sequence _buttonAnimSeq; // For button growing and shrinking

        protected virtual void Awake()
        {
            foreach (Button btn in buttons)
            {
                btn.enabled = false;
            }
        }

        protected virtual void Start()
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
                OnComplete(() => {
                    settingUI.gameObject.SetActive(true);
                    settingButton.transform.DOScale(originalButtonScale, 0.2f);
                });
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

    }
}