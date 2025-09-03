using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberSpeed.CardsMatchGame
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private UIScreenBase mainMenuUIHandler, gamePlayUIHandler, gameEndUIHandler;

        [field: Header("Game Settings")]
        public int rows { get; private set; }
        public int cols { get; private set; }
      
        public static GameManager Instance { get; private set; }
       
        public Action<int, int> OnGameStarted;   // rows, cols
        public Action OnGameOver;
        public Action OnGamePaused;
        public Action OnGameResumed;
        public Action OnGameSave;
        
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
        
        private void InitialiseGame()
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
            gamePlayUIHandler.gameObject.SetActive(true);
            OnGameStarted?.Invoke(rows, cols);
        }

        public void GameOver()
        {
            Debug.Log("Game Over!");
            OnGameOver?.Invoke();
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
            OnGameSave?.Invoke();
        }
    }
}

