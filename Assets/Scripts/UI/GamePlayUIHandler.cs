using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace CyberSpeed.CardsMatchGame
{
    public class GamePlayUIHandler : UIScreenBase
    {

        [SerializeField] private CardManager cardManager;
        [SerializeField] private TextMeshProUGUI turnText,matchesText,scoreText,comboText;
        private void OnEnable()
        {
            GameManager.Instance.OnGameStarted += SpawnGrid;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.TurnChanged += OnTurnChanged;
                ScoreManager.Instance.MatchStatsChanged += OnMatchStatsChanged; 
                ScoreManager.Instance.ComboChanged += OnComboChanged;
            }
        }
        
        private void OnDisable()
        {
            GameManager.Instance.OnGameStarted -= SpawnGrid;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.TurnChanged -= OnTurnChanged;
                ScoreManager.Instance.MatchStatsChanged -= OnMatchStatsChanged;
                ScoreManager.Instance.ComboChanged -= OnComboChanged;
            }
        }
        
        private void SpawnGrid(int rows, int cols)
        {
            Debug.Log($"Spawning {rows}x{cols} board...");
            cardManager.CreateCardGrid(rows, cols);
            turnText.text = "Turn: 0";
            matchesText.text = "Matches: 0";
            scoreText.text = "Score: 0";
            comboText.text = "Combo: 0";
        }

        private void OnTurnChanged(int turns)
        {
            turnText.text = $"Turn: {turns}";
        }

        private void OnMatchStatsChanged(int matches, int score, int combo)
        {
            matchesText.text = $"Matches: {matches}";
            scoreText.text = $"Score: {score}";
            comboText.text = $"Combo: {combo}";
        }
        
        private void OnComboChanged(int combo)
        {
            comboText.text = $"Combo: {combo}";
        }
    }
}
