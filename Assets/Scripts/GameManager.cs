using CGT.CharacterControls;
using DG.Tweening;
using NaughtyAttributes;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SimpleSpyGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] protected Image _blackScreen;
        [SerializeField] protected float _blackScreenFadeDuration = 1f;

        [Tooltip("How long the player has to wait (after losing in or beating a level) before they can apply input to move on")]
        [SerializeField] protected float _levelChangeDelay = 2f;

        [Tooltip("Make sure they're set in order here")]
        [SerializeField][Scene] protected List<string> _stages = new List<string>();

        [SerializeField] [Scene] protected string _titleScreenScene = "TitleScreen";

        protected virtual void Awake()
        {
            if (S != null & S != this)
            {
                Destroy(this.gameObject);
                return;
            }

            S = this;

            _levelChangeWait = new WaitForSeconds(_levelChangeDelay);
            _inputReader = FindObjectOfType<AltInputReader>();
        }

        public static GameManager S { get; protected set; }
        protected int _currentLevelIndex = 0;
        protected WaitForSeconds _levelChangeWait;
        protected AltInputReader _inputReader;

        protected virtual void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            StageEvents.DocRetrieved += OnDocRetrieved;
            StageEvents.PlayerCaught += OnPlayerCaught;
            _inputReader.InteractStart += OnPlayerInteractInput;
        }

        protected virtual void OnDocRetrieved()
        {
            StageEvents.PlayerWon();
            Debug.Log("The player won!");
            LevelOver = true;
        }

        protected virtual IEnumerator PlayerVictorySequence()
        {
            yield return WaitToAllowPlayerInput();
        }

        protected virtual void ShowVictoryUI()
        {

        }

        protected virtual IEnumerator WaitToAllowPlayerInput()
        {
            yield return _levelChangeWait;
            _playerMayMoveToNextStage = true;
        }

        protected virtual void OnPlayerCaught()
        {
            StageEvents.PlayerLost();
            Debug.Log("The player was caught...");
            LevelOver = true;
            StartCoroutine(PlayerLossSequence());
        }

        protected virtual IEnumerator PlayerLossSequence()
        {
            ShowDefeatUI();
            yield return _levelChangeWait;
            _playerMayMoveToNextStage = true;
        }

        protected virtual void ShowDefeatUI()
        {

        }

        protected bool _playerMayMoveToNextStage;

        protected virtual void OnPlayerInteractInput()
        {
            if (!_playerMayMoveToNextStage || !LevelOver)
            {
                return;
            }

            bool thereIsAnotherLevel = _currentLevelIndex < _stages.Count - 1;
            if (thereIsAnotherLevel)
            {
                MoveToLevel(_currentLevelIndex + 1);
            }
            else
            {
                _playerMayMoveToNextStage = false; // So avoid the potential issue with button-mashing
                SceneManager.LoadScene(_titleScreenScene);
            }
        }

        public virtual void MoveToLevel(int levelIndex)
        {
            _currentLevelIndex = levelIndex;
            _movingToNextLevel = true;
            _blackScreen.DOFade(0, _blackScreenFadeDuration)
                .OnComplete(LoadLevelOnScreenFadeOut);
        }

        protected bool _movingToNextLevel;

        protected virtual void LoadLevelOnScreenFadeOut()
        {
            string levelToLoad = _stages[_currentLevelIndex];
            SceneManager.LoadScene(levelToLoad);
            _movingToNextLevel = false;
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _playerMayMoveToNextStage = false;
            _movingToNextLevel = false;

            if (scene.name == _titleScreenScene)
            {
                _currentLevelIndex = 0;
            }
            else
            {
                int stageIndex = _stages.IndexOf(scene.name);
                bool loadedAStage = stageIndex >= 0;
                if (loadedAStage)
                {
                    _currentLevelIndex = stageIndex;
                }
            }
        }

        protected virtual void OnDisable()
        {
            StageEvents.DocRetrieved -= OnDocRetrieved;
            StageEvents.PlayerCaught -= OnPlayerCaught;
            _inputReader.InteractStart -= OnPlayerInteractInput;
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        public virtual bool LevelOver { get; protected set; }
    }
}