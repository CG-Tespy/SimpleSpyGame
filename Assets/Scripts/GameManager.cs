using UnityEngine;

namespace SimpleSpyGame
{
    public class GameManager : MonoBehaviour
    {
        protected virtual void Awake()
        {
            if (S != null & S != this)
            {
                Destroy(this.gameObject);
                return;
            }

            S = this;
        }

        public static GameManager S { get; protected set; }

        protected virtual void OnEnable()
        {
            StageEvents.DocRetrieved += OnDocRetrieved;
            StageEvents.PlayerCaught += OnPlayerCaught;
        }

        protected virtual void OnDocRetrieved()
        {
            StageEvents.PlayerWon();
            Debug.Log("The player won!");
            LevelOver = true;
        }

        protected virtual void OnPlayerCaught()
        {
            StageEvents.PlayerLost();
            Debug.Log("The player was caught...");
            LevelOver = true;
        }

        protected virtual void OnDisable()
        {
            StageEvents.DocRetrieved -= OnDocRetrieved;
            StageEvents.PlayerCaught -= OnPlayerCaught;
        }

        public virtual bool LevelOver { get; protected set; }
    }
}