using System;
using System.Collections.Generic;
using UnityEngine;

namespace CyberSpeed.CardsMatchGame
{
    public class GameManager : MonoBehaviour, IGameEvents
    {
        [field: Header("Game Settings")]
        public int rows { get; private set; }
        public int cols { get; private set; }
        public static GameManager Instance { get; private set; }
       
        public event Action<int, int> OnGameStarted;   // rows, cols
        public event Action<IScoreData> OnGameOver;
        public event Action OnGamePaused;
        public event Action OnGameResumed;
        public event Action OnGameSave;
        

        [SerializeField] private UIScreenBase mainMenuUIHandler, gamePlayUIHandler, gameEndUIHandler;
        public CardManager cardManager;
        void Awake()
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
        }

        private void Start()
        {
            InitialiseGame();
        }
        
        public void InitialiseGame()
        {
            Debug.Log("GameManager: InitialiseGame() called");
            mainMenuUIHandler.gameObject.SetActive(true);
            gameEndUIHandler.gameObject.SetActive(false);
            gamePlayUIHandler.gameObject.SetActive(false);
        }
        
        public void StartGame(int rowCount, int colCount)
        {
            Debug.Log($"GameManager: StartGame rows {rowCount} cols {colCount}");
            rows = rowCount;
            cols = colCount;
            mainMenuUIHandler.gameObject.SetActive(false);
            gamePlayUIHandler.gameObject.SetActive(true);
            ScoreManager.Instance.ResetAll();
            OnGameStarted?.Invoke(rows, cols);
            
        }
        
        public void ReStartGame()
        {
            Debug.Log($"GameManager: ReStartGame rows {rows} cols {cols}");
            StartGame(rows, cols);
        }

        public void GameOver()
        {
            Debug.Log("Game Over!");
            gamePlayUIHandler.gameObject.SetActive(false);
            gameEndUIHandler.gameObject.SetActive(true);
            string key = $"{rows}x{cols}";
            SaveLoadManager.Delete(key);
            OnGameOver?.Invoke(ScoreManager.Instance);
        }

        public void PauseGame()
        {
            Debug.Log("Game Paused!");
            OnGamePaused?.Invoke();
        }

        public void ResumeGame()
        {
            Debug.Log("Game Resumed!");
            OnGameResumed?.Invoke();
        }

        public void GameSave()
        {
            Debug.Log("Game Saved!");
            string key = $"{rows}x{cols}";

            SaveData data = new SaveData
            {
                rows = rows,
                cols = cols,
                turns = ScoreManager.Instance.Turns,
                matches = ScoreManager.Instance.Matches,
                combo = ScoreManager.Instance.Combo,
                score = ScoreManager.Instance.Score,
                cardValues = new List<int>(),
                cardMatched = new List<bool>(),
                cardFaceDown = new List<bool>()
            };

            var allCards = FindObjectsOfType<CardUI>();
            foreach (var card in allCards)
            {
                data.cardValues.Add(card.CardValue);
                data.cardMatched.Add(card.IsMatched);
                data.cardFaceDown.Add(card.IsFaceDown);
            }

            SaveLoadManager.Save(key, data);
            OnGameSave?.Invoke();
        }
        
        
        public void LoadGame(string key)
        {
            
            SaveData data = SaveLoadManager.Load(key);
            if (data == null)
            {
                Debug.LogWarning($"No save found for {key}");
                return;
            }
          
            mainMenuUIHandler.gameObject.SetActive(false);
            gamePlayUIHandler.gameObject.SetActive(true);

            rows = data.rows;
            cols = data.cols;
            gamePlayUIHandler.gameObject.SetActive(true);

            cardManager.ReleaseAllCards();
            cardManager.CreateCardGrid(rows, cols);

            var allCards = FindObjectsOfType<CardUI>();
            for (int i = 0; i < allCards.Length; i++)
            {
                allCards[i].Initialize(data.cardValues[i],
                    cardManager.animalSpritesSo.GetSprite(data.cardValues[i]));

                if (data.cardMatched[i])
                    allCards[i].SetMatched();
                else if (data.cardFaceDown[i])
                    allCards[i].ShowCardFront();
                else
                    allCards[i].ShowCardBack();
            }

            ScoreManager.Instance.RestoreState(
                data.turns, data.matches, data.combo, data.score
            );
            
            //NEW: Sync MatchController with restored cards
            var matchController = FindObjectOfType<MatchController>();
            int matchedCount = 0;
            foreach (var card in allCards)
            {
                if (card.IsMatched) matchedCount++;
            }
            matchController.Initialise((rows * cols) / 2);
            matchController.SetMatchedPairs(matchedCount / 2); // each pair has 2 cards
            
        }
    }
}

