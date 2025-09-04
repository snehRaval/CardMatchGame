using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberSpeed.CardsMatchGame
{
    public class GameEndUIHandler : UIScreenBase
    {
        [SerializeField] private TextMeshProUGUI turnText,matchesText,scoreText,comboText;
        [SerializeField] Button rePlayButton, homeButton;
       
        public void OnEnable()
        {
            GameManager.Instance.eventDispatcher.Subscribe<IScoreData>(GameEvents.GAME_OVER,OnGameoverUIUpdate);
            rePlayButton.onClick.AddListener(OnReplayClick);
            homeButton.onClick.AddListener(OnHomeClick);
        }

        public void OnDisable()
        {
            GameManager.Instance.eventDispatcher.UnsubscribeAll(this);
            rePlayButton.onClick.RemoveListener(OnReplayClick);
            homeButton.onClick.RemoveListener(OnHomeClick);
        }
        
        public void Show(bool value)
        {
            gameObject.SetActive(value);
        }

        public void Reset()
        {
            turnText.text = "Turn: 0";
            matchesText.text = "Matches: 0";
            scoreText.text = "Score: 0";
            comboText.text = "Combo: 0";
        }

        private void OnGameoverUIUpdate(IScoreData scoreData)
        {
            turnText.text = $"Turns: {scoreData.Turns}";
            matchesText.text = $"Matches: {scoreData.Matches}";
            scoreText.text = $"Score: {scoreData.Score}";
            comboText.text = $"Combo: {scoreData.Combo}";
        }

        private void OnHomeClick()
        {
            if (GameManager.Instance.cardManager != null)
                GameManager.Instance.cardManager.ReleaseAllCards();
           
            AudioManager.Instance.PlayButtonClick();
            GameManager.Instance.InitialiseGame();
            Show(false);
        } 
        
        private void OnReplayClick()
        {
            if (GameManager.Instance.cardManager  != null)
                GameManager.Instance.cardManager.ReleaseAllCards();

            AudioManager.Instance.PlayButtonClick();
            GameManager.Instance.ReStartGame();
            
            Show(false);
        }
    }
}
