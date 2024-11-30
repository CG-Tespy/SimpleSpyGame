using CGT.CharacterControls;
using UnityEngine;

namespace SimpleSpyGame
{
    public class HidingSpot : MonoBehaviour
    {
        [Tooltip("This will show up while Third Eye is active")]
        [SerializeField] protected GameObject _thirdEyeVfx;
        [SerializeField] protected Color _debugColor = Color.blue;

        protected virtual void Awake()
        {
            _inputReader = FindObjectOfType<AltInputReader>();
            _thirdEyeVfx.SetActive(false);
        }
        
        protected AltInputReader _inputReader;


        protected virtual void OnEnable()
        {
            _inputReader.ThirdEyeToggleStart += OnThirdEyeToggle;
            StageEvents.PlayerWon += OnPlayerWonOrLost;
            StageEvents.PlayerLost += OnPlayerWonOrLost;
        }

        protected virtual void OnThirdEyeToggle()
        {
            _thirdEyeVfx.SetActive(!_thirdEyeVfx.activeSelf);
        }

        protected virtual void OnPlayerWonOrLost()
        {
            this.gameObject.SetActive(false);
        }

        protected virtual void OnDisable()
        {
            _inputReader.ThirdEyeToggleStart -= OnThirdEyeToggle;
            StageEvents.PlayerWon -= OnPlayerWonOrLost;
            StageEvents.PlayerLost -= OnPlayerWonOrLost;
        }

    }
}