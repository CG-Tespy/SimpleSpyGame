using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using NaughtyAttributes;
using UnityEngine.UI;
using UnityEngine.Serialization;

namespace SimpleSpyGame
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] [Scene] protected string _titleScene = "TitleScreen_CGT";
        [SerializeField] protected Transform _holdsTitleButtons;
        [SerializeField] protected TextMeshProUGUI _victoryText, _defeatText;
        [SerializeField] protected float _fadeDur = 2f;

        [SerializeField] protected Image _fadeScreen;

        protected virtual void Awake()
        {
            _titleButtons = _holdsTitleButtons.GetComponentsInChildren<Button>();
            SetLevelEndTextVisibility(_hideIt);
            Color screenCol = _fadeScreen.color;
            screenCol.a = 0;
            _fadeScreen.color = screenCol;
        }

        protected IList<Button> _titleButtons;

        protected virtual void SetLevelEndTextVisibility(float visibility)
        {
            Color victoryColor = _victoryText.color;
            Color defeatColor = _defeatText.color;
            victoryColor.a = defeatColor.a = 0;

            _victoryText.color = victoryColor;
            _defeatText.color = defeatColor;
        }

        protected static int _hideIt = 0;

        protected virtual void OnEnable()
        {
            StageEvents.PlayerWon += OnPlayerWon;
            StageEvents.PlayerLost += OnPlayerLost;
            SystemEvents.MoveToNextLevelStart += OnMoveToNextLevelStart;
            SystemEvents.ExitGameSeqStart += OnExitGameSeqStart;
        }

        protected virtual void OnPlayerWon()
        {
            KillLevelEndTextTweens();
            _victoryText.DOFade(_showIt, _fadeDur);
        }

        protected virtual void KillLevelEndTextTweens()
        {
            _victoryText.DOKill();
            _defeatText.DOKill();
        }

        protected static int _showIt = 1;

        protected virtual void OnPlayerLost()
        {
            KillLevelEndTextTweens();
            _defeatText.DOFade(_showIt, _fadeDur);
        }

        protected virtual void OnMoveToNextLevelStart()
        {
            KillLevelEndTextTweens();

            _victoryText.DOFade(_hideIt, _fadeDur);
            _defeatText.DOFade(_hideIt, _fadeDur);
            _fadeScreen.DOFade(_showIt, _fadeDur)
                .OnComplete(OnBlackScreenDoneFadingOut);

        }

        protected virtual void OnBlackScreenDoneFadingOut()
        {
            SystemEvents.ScreenFadeOutDone();
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            _fadeScreen.DOKill();
            _fadeScreen.DOFade(0, _fadeDur);

            if (scene.name == _titleScene)
            {
                SetLevelEndTextVisibility(_hideIt);
            }
        }

        protected virtual void OnExitGameSeqStart()
        {
            foreach (var buttonEl in _titleButtons)
            {
                buttonEl.interactable = false;
            }

            _fadeScreen.DOFade(1, _fadeDur)
                .OnComplete(ExitGameFadeDone);
        }

        protected virtual void ExitGameFadeDone()
        {
            SystemEvents.FadeOutForGameExitDone();
        }

        protected virtual void OnDisable()
        {
            StageEvents.PlayerWon -= OnPlayerWon;
            StageEvents.PlayerLost -= OnPlayerLost;
            SystemEvents.MoveToNextLevelStart -= OnMoveToNextLevelStart;
            SystemEvents.ExitGameSeqStart -= OnExitGameSeqStart;
        }
    }
}