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
            GameManager.Instance.eventDispatcher.Subscribe<GameStartedPayload>(GameEvents.GAME_STARTED,SpawnGrid);
            GameManager.Instance.eventDispatcher.Subscribe<IScoreData>(GameEvents.GAME_SCORE_UPDATE,OnScoreUpdate);
            
            homeButton.onClick.AddListener(OnHomeButtonClick);
            gameEndPopup.Show(false);
        }
        
        private void OnDisable()
        {
            GameManager.Instance.eventDispatcher.UnsubscribeAll(this);
            homeButton.onClick.RemoveListener(OnHomeButtonClick);
        }
        
        private void SpawnGrid(GameStartedPayload payloadData)
        {
            Debug.Log($"Spawning {payloadData.rows}x{payloadData.cols} board...");
            cardManager.CreateCardGrid(payloadData.rows, payloadData.cols);

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

        private void OnHomeButtonClick()
        {
            AudioManager.Instance.PlayButtonClick();
            gameEndPopup.Show(true);
        }
    }
}
