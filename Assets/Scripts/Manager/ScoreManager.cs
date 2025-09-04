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
        public event Action<IScoreData> OnScoreUpdate; //combo
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
            ResetAll();
        }
        
        public void ResetAll()
        {
            Turns = 0;
            Matches = 0;
            Combo = 0;
            Score = 0;
            Matches = 0;
            baseScorePerMatch = 0;
        }

        public void IncrementTurn()
        {
            Turns++;
            OnScoreUpdate?.Invoke(this);
        }

        public void OnMatch()
        {
            Matches++;
            baseScorePerMatch++;
            // increase combo each successful continuous match
            Combo = Mathf.Max(0, Combo + 1);
            Score += baseScorePerMatch * Combo;
            OnScoreUpdate?.Invoke(this);
            AudioManager.Instance.PlayMatchFound();
        }

        public void OnMismatch()
        {
            // break streak; set combo back to 1
            Combo = 0;
            baseScorePerMatch = 1;
            OnScoreUpdate?.Invoke(this);
            AudioManager.Instance.PlayMismatch();
        }
        
        public void RestoreState(int turns, int matches, int combo, int score)
        {
            Turns = turns;
            Matches = matches;
            Combo = combo;
            Score = score;
            OnScoreUpdate?.Invoke(this);
        }
    }
}
