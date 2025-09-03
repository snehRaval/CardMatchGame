using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberSpeed.CardsMatchGame
{
    public class GamePlayUIHandler : UIScreenBase
    {

        [SerializeField] private CardManager cardManager;
        private void OnEnable()
        {
            GameManager.Instance.OnGameStarted += SpawnGrid;
        }
        
        private void OnDisable()
        {
            GameManager.Instance.OnGameStarted -= SpawnGrid;
        }
        
        private void SpawnGrid(int rows, int cols)
        {
            Debug.Log($"Spawning {rows}x{cols} board...");
            cardManager.CreateCardGrid(rows, cols);
        }
    }
}
