using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberSpeed.CardsMatchGame
{
    public class GamePlayUIHandler : UIScreenBase
    {

        [SerializeField] private CardManager cardManager;
        [SerializeField] private TextMeshProUGUI turnText,matchesText,scoreText,comboText;
      
        [SerializeField] Button homeButton;
        [SerializeField] ExitPopupUI gameEndPopup;
        private void OnEnable()
        {
            GameManager.Instance.OnGameStarted += SpawnGrid;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnScoreUpdate += OnScoreUpdate;
            }
            
            homeButton.onClick.AddListener(OnHomeClick);
            gameEndPopup.Show(false);
        }
        
        private void OnDisable()
        {
            GameManager.Instance.OnGameStarted -= SpawnGrid;
            if (ScoreManager.Instance != null)
            {
                ScoreManager.Instance.OnScoreUpdate -= OnScoreUpdate;
            }
            
            homeButton.onClick.RemoveListener(OnHomeClick);
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

        private void OnScoreUpdate(IScoreData scoreData)
        {
            turnText.text = $"Turn: {scoreData.Turns}";
            matchesText.text = $"Matches: {scoreData.Matches}";
            scoreText.text = $"Score: {scoreData.Score}";
            comboText.text = $"Combo: {scoreData.Combo}";
        }

        private void OnHomeClick()
        {
            gameEndPopup.Show(true);
        }
    }
}
