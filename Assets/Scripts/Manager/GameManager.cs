using System;
using System.Collections.Generic;
using UnityEngine;

namespace CyberSpeed.CardsMatchGame
{
    [DefaultExecutionOrder(-100)] 
    public class GameManager : MonoBehaviour
    {
        [field: Header("Game Settings")]
        public int rows { get; private set; }
        public int cols { get; private set; }
        public static GameManager Instance { get; private set; }
        public CardManager cardManager;
        public ISaveManager saveManager;
        public EventDispatcher eventDispatcher;

        public GameLayoutScriptableObject gameLayoutSOAsset;
        public AnimalSpritesScriptable animalSpritesSo;
        
        [SerializeField] private UIScreenBase mainMenuUIHandler, gamePlayUIHandler, gameEndUIHandler;

        
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
            saveManager = new SaveLoadManager();
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
            eventDispatcher?.Dispatch(GameEvents.GAME_STARTED, new GameStartedPayload { rows = rows, cols = cols });
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
            saveManager.Delete(key);
            AudioManager.Instance.PlayGameOver();
            eventDispatcher?.Dispatch(GameEvents.GAME_OVER, ScoreManager.Instance );
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
            
            saveManager.Save(key, data);
            eventDispatcher?.Dispatch(GameEvents.GAME_SAVED, new GameSavedPayload { key = key });
        }
        
        public void LoadGame(string key)
        {
            SaveData data = saveManager.Load(key);
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
                allCards[i].Initialize(data.cardValues[i], animalSpritesSo.GetSprite(data.cardValues[i]));

                if (data.cardMatched[i])
                    allCards[i].SetMatched();
                else if (data.cardFaceDown[i])
                    allCards[i].ShowCardFront();
                else
                    allCards[i].ShowCardBack();
            }

            ScoreManager.Instance.RestoreState(data.turns, data.matches, data.combo, data.score);
            
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

