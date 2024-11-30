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
            DontDestroyOnLoad(this.gameObject);

            _levelChangeWait = new WaitForSeconds(_levelChangeDelay);
            
        }

        public static GameManager S { get; protected set; }
        protected int _currentLevelIndex = 0;
        protected WaitForSeconds _levelChangeWait;
        protected AltInputReader _inputReader;

        protected virtual void OnEnable()
        {
            _inputReader = FindObjectOfType<AltInputReader>();

            ListenForEvents();
        }

        protected virtual void ListenForEvents()
        {
            StageEvents.DocRetrieved += OnDocRetrieved;
            StageEvents.PlayerCaught += OnPlayerCaught;

            if (_inputReader != null) // Since the player might not be in the current scene
            {
                _inputReader.InteractStart += OnPlayerInteractInput;
            }
            SystemEvents.ScreenFadeOutDone += OnScreenFadeOutDone;
            SceneManager.sceneLoaded += OnSceneLoaded;
            SystemEvents.FadeOutForGameExitDone += OnFadeOutForGameExitDone;
        }

        protected virtual void OnDocRetrieved()
        {
            StageEvents.PlayerWon();
            Debug.Log("The player won!");
            LevelOver = true;
            StartCoroutine(PlayerVictorySequence());
        }

        protected virtual IEnumerator PlayerVictorySequence()
        {
            _playerLost = false;
            _playerWon = true;
            yield return WaitToAllowPlayerInput();
        }

        protected bool _playerWon, _playerLost;

        protected virtual IEnumerator WaitToAllowPlayerInput()
        {
            yield return _levelChangeWait;
            _playerMayFinishScene = true;
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
            _playerLost = true;
            _playerWon = false;
            yield return WaitToAllowPlayerInput();
        }

        protected bool _playerMayFinishScene;

        protected virtual void OnPlayerInteractInput()
        {
            if (!_playerMayFinishScene || !LevelOver)
            {
                return;
            }

            _playerMayFinishScene = false; // <- To avoid the potential issue with button-mashing
            bool thereIsAnotherLevel = _currentLevelIndex < _stages.Count - 1;
            if (thereIsAnotherLevel && _playerWon)
            {
                _shouldMoveToNextLevel = true;
                SystemEvents.MoveToNextLevelStart();
            }
            else
            {
                _shouldMoveToTitleScreen = true;
                SystemEvents.MoveToTitleScreenStart();
            }
        }

        protected bool _shouldMoveToNextLevel, _shouldMoveToTitleScreen;
        public virtual void MoveToLevel(int levelIndex)
        {
            _currentLevelIndex = levelIndex;
            string levelToLoad = _stages[_currentLevelIndex];
            SceneManager.LoadScene(levelToLoad);
        }

        protected bool _movingToNextLevel;

        protected virtual void OnScreenFadeOutDone()
        {
            if (_shouldMoveToNextLevel)
            {
                MoveToLevel(_currentLevelIndex + 1);
            }
            else if (_shouldMoveToTitleScreen)
            {
                SceneManager.LoadScene(_titleScreenScene);
            }
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            _playerMayFinishScene = _shouldMoveToTitleScreen =
                _shouldMoveToNextLevel = _movingToNextLevel = 
                _playerWon = _playerLost = false;

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

                _inputReader = FindObjectOfType<AltInputReader>();

                UNlistenForEvents();
                ListenForEvents();

                SystemEvents.LevelLoaded();
            }
        }

        protected virtual void OnDisable()
        {
            UNlistenForEvents();
        }

        protected virtual void UNlistenForEvents()
        {
            StageEvents.DocRetrieved -= OnDocRetrieved;
            StageEvents.PlayerCaught -= OnPlayerCaught;

            if (_inputReader != null)
            {
                _inputReader.InteractStart -= OnPlayerInteractInput;
            }

            SystemEvents.ScreenFadeOutDone -= OnScreenFadeOutDone;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            SystemEvents.FadeOutForGameExitDone -= OnFadeOutForGameExitDone;
        }

        public virtual bool LevelOver { get; protected set; }

        public virtual void ExitGame()
        {
            SystemEvents.ExitGameSeqStart();
        }

        protected virtual void OnFadeOutForGameExitDone()
        {
            Application.Quit();
        }
    
        public virtual void NewGame()
        {
            _currentLevelIndex = -1; // <- Should be set to 0 when the fade's done
            _shouldMoveToNextLevel = true;
            SystemEvents.MoveToNextLevelStart();
        }
    }
}