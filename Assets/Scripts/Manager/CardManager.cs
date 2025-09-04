using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace CyberSpeed.CardsMatchGame
{
    public class CardManager : MonoBehaviour
    {
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
            int totalCards = rows * cols;
            if (totalCards % 2 != 0)
            {
                Debug.LogError($"Grid must have even number of cells. Received {rows}x{cols} = {totalCards}.");
                return;
            }

            totalPairs = totalCards / 2;

            gridObject.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            gridObject.constraintCount = cols;
          
            // apply cell size from SO if configured
            if (GameManager.Instance?.gameLayoutSOAsset != null)
            {
                var gridLevels = GameManager.Instance.gameLayoutSOAsset.gridLevels;
                foreach (var layout in gridLevels)
                {
                    if (layout.rows == rows && layout.cols == cols)
                    {
                        gridObject.cellSize = new Vector2(layout.cellSize, layout.cellSize);
                        break;
                    }
                }
            }

            Debug.Log($"CreateCardGrid: totalPairs {totalPairs} | Grid {rows}x{cols}");

            // Generate and shuffle card values
            List<int> cardValues = GenerateCardPairs(totalPairs);
            Shuffle(cardValues);
            
            // Clear previous cards if needed
            foreach (Transform child in gridObject.transform)
            {
                child.gameObject.SetActive(false);
            }
            
            if (CardInstantiate(rows, cols, cardValues)) return;

            matchController?.Initialise(totalPairs);
            matchController?.RunInitialReveal();
        }

        private bool CardInstantiate(int rows, int cols, List<int> cardValues)
        {
            // Instantiate cards
            int cardIndex = 0;
            for (int row = 0; row < rows; row++)
            {
                for (int col = 0; col < cols; col++)
                {
                    GameObject cardObj = objectPool.GetObjectFromPool();
                    if (cardObj == null)
                    {
                        Debug.LogError("Failed to get card object from pool. Pool may not be initialized properly.");
                        return true;
                    }

                    cardObj.SetActive(true);
                    CardUI card = cardObj.GetComponent<CardUI>();
                    int value = cardValues[cardIndex];
                    card.Initialize(value, GameManager.Instance?.animalSpritesSo.GetSprite(value));

                    matchController?.RegisterCard(card);

                    cardIndex++;
                }
            }

            return false;
        }
        private List<int> GenerateCardPairs(int totalPairs)
        {
            List<int> pairs = new List<int>();
            int count = (int) GameManager.Instance?.animalSpritesSo.Count;
            for (int i = 0; i < totalPairs; i++)
            {
                int index = i % count;
                pairs.Add(index);
                pairs.Add(index);
            }
            return pairs;
        }
        private void Shuffle(List<int> list)
        {
            for (int i = list.Count - 1; i > 0; i--)
            {
                int j = Random.Range(0, i + 1);
                (list[i], list[j]) = (list[j], list[i]); // tuple swap
            }
        }
    }
}