using UnityEngine;

namespace SimpleSpyGame
{
    public class GameManager : MonoBehaviour
    {
        protected virtual void OnEnable()
        {
            StageEvents.DocRetrieved += OnDocRetrieved;
            StageEvents.PlayerCaught += OnPlayerCaught;
        }

        protected virtual void OnDocRetrieved()
        {
            StageEvents.PlayerWon();
            Debug.Log("The player won!");
        }

        protected virtual void OnPlayerCaught()
        {
            StageEvents.PlayerLost();
            Debug.Log("The player was caught...");
        }

        protected virtual void OnDisable()
        {
            StageEvents.DocRetrieved -= OnDocRetrieved;
            StageEvents.PlayerCaught -= OnPlayerCaught;
        }
    }
}