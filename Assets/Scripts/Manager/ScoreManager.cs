using System;
using UnityEngine;

namespace CyberSpeed.CardsMatchGame
{
    public class ScoreManager :  MonoBehaviour, IScoreData
    { 
        public static ScoreManager Instance { get; private set; }
        private int baseScorePerMatch = 1;
        public int Turns { get; private set; }
        public int Matches { get; private set; }
        public int Combo { get; private set; }
        public int Score { get; private set; }
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
            
            ResetAll();
        }

        private void OnEnable()
        {
            GameManager.Instance.eventDispatcher.Subscribe<GameStartedPayload>(GameEvents.GAME_STARTED,OnGameStarted);
            GameManager.Instance.eventDispatcher.Subscribe(GameEvents.GAME_CARD_MATCH,OnMatch);
            GameManager.Instance.eventDispatcher.Subscribe(GameEvents.GAME_CARD_MISSMATCH,OnMismatch);
        }
        
        private void OnDisable()
        {
            GameManager.Instance.eventDispatcher.UnsubscribeAll(this);
        }
        
        private void OnGameStarted(GameStartedPayload obj)
        {
            ResetAll();
        }

        public void ResetAll()
        {
            Turns = 0;
            Matches = 0;
            Combo = 0;
            Score = 0;
            Matches = 0;
            baseScorePerMatch = 1;
        }
        
        private void OnMatch()
        {
            Turns++;
            Matches++;
            // increase combo each successful continuous match
            Combo = Mathf.Max(0, Combo + 1);
            // +1 per match, multiply by combo only when combo > 1
            int increment = Combo > 1 ? (1 * Combo) : 1;
            Score += increment;
            AudioManager.Instance.PlayMatchFound();
            GameManager.Instance?.eventDispatcher.Dispatch(GameEvents.GAME_SCORE_UPDATE, this);
        }

        private void OnMismatch()
        {
            // break streak; set combo back to 1
            Turns++;
            Combo = 0;
            baseScorePerMatch = 1;
            AudioManager.Instance.PlayMismatch();
            GameManager.Instance?.eventDispatcher.Dispatch(GameEvents.GAME_SCORE_UPDATE, this);
        }
        
        public void RestoreState(int turns, int matches, int combo, int score)
        {
            Turns = turns;
            Matches = matches;
            Combo = combo;
            Score = score;
            GameManager.Instance?.eventDispatcher.Dispatch(GameEvents.GAME_SCORE_UPDATE, this);
        }
    }
}
