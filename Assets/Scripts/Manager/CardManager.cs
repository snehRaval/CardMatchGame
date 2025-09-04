using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CyberSpeed.CardsMatchGame
{
    public class CardManager : MonoBehaviour
    {
        [SerializeField] public AnimalSpritesScriptable animalSpritesSo;
        [SerializeField] private GridLayoutGroup gridObject;
        [SerializeField] private MatchController matchController;
        [SerializeField] private GameObject cardPrefab;

        private int totalPairs;
        private ObjectPool objectPool;

        private void Awake()
        {
            objectPool = gridObject.gameObject.AddComponent<ObjectPool>();
            objectPool.InitializePool(cardPrefab);
        }
        
        public void ReleaseAllCards()
        {
            if (objectPool != null)
            {
                objectPool.ReleaseAll();
            }
        }

        //  private CardUI[,] cardGrid;
        public void CreateCardGrid(int rows, int cols)
        {
            totalPairs = (rows * cols) / 2;
            //    cardGrid = new CardUI[rows, cols];
            gridObject.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridObject.constraintCount = cols;

            Debug.Log($"CreateCardGrid: totalPairs {totalPairs} {rows} {cols}");
            // Create card pairs
            List<int> cardValues = new List<int>();
            for (int i = 0; i < totalPairs; i++)
            {
                cardValues.Add(i);
                cardValues.Add(i);
            }

            // Shuffle card values
            for (int i = 0; i < cardValues.Count; i++)
            {
                int temp = cardValues[i];
                int randomIndex = Random.Range(i, cardValues.Count);
                cardValues[i] = cardValues[randomIndex];
                cardValues[randomIndex] = temp;
            }

            // Validate sprite capacity
            if (totalPairs > animalSpritesSo.Count)
            {
                Debug.LogError($"Not enough sprites. Needed pairs: {totalPairs}, available unique sprites: {animalSpritesSo.Count}");
                return;
            }

            // Create cards
            int cardIndex = 0;
            for (int y = 0; y < cols; y++)
            {
                for (int x = 0; x < rows; x++)
                {
                    GameObject cardObj = objectPool.GetObjectFromPool();
                    if (cardObj == null)
                    {
                        Debug.LogError("Failed to get card object from pool. Pool may not be initialized properly.");
                        return;
                    }
                    
                    cardObj.SetActive(true);
                    CardUI card = cardObj.GetComponent<CardUI>();
                    //Debug.Log($"CreateCardGrid: Initialize cardValues {cardValues[cardIndex]} cardIndex : {cardIndex}");
                    card.Initialize(cardValues[cardIndex], animalSpritesSo.GetSprite(cardValues[cardIndex]));
                    //       cardGrid[x, y] = card;
                    if (matchController != null)
                    {
                        matchController.RegisterCard(card);
                    }

                    cardIndex++;
                }
            }

            if (matchController != null)
            {
                matchController.Initialise(totalPairs);
                matchController.RunInitialReveal();
            }
        }
    }
}