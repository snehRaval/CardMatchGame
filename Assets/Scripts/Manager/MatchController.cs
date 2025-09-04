using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CyberSpeed.CardsMatchGame
{
     public class MatchController : MonoBehaviour
    {
        private readonly List<CardUI> allCards = new List<CardUI>();
        private CardUI firstSelection;
        private CardUI secondSelection;
        private int matchedPairs;
        private int totalPairs;
        private bool inputLocked;

        [SerializeField] private float mismatchHideDelay = 0.6f;
        [SerializeField] private float initialRevealSeconds = 2f;

        public void Initialise(int totalPairsCount)
        {
            totalPairs = totalPairsCount;
            matchedPairs = 0;
            firstSelection = null;
            secondSelection = null;
            inputLocked = false;
        }

        public void RegisterCard(CardUI card)
        {
            if (card == null) return;
            allCards.Add(card);
            card.Clicked += OnCardClicked;
        }

        private void OnDestroy()
        {
            foreach (var card in allCards)
            {
                if (card != null)
                {
                    card.Clicked -= OnCardClicked;
                }
            }
            allCards.Clear();
        }

        private void OnCardClicked(CardUI card)
        {
            if (inputLocked || card == null || card.IsMatched)
                return;

            // reveal immediately for feedback
            card.ShowCardFront();

            if (firstSelection == null)
            {
                firstSelection = card;
                return;
            }

            if (secondSelection == null && card != firstSelection)
            {
                secondSelection = card;
                EvaluateSelection();
            }
        }

        private void EvaluateSelection()
        {
            if (firstSelection == null || secondSelection == null)
                return;

            // A turn is counted once two cards are selected
            if (firstSelection.CardValue == secondSelection.CardValue)
            {
                // match
                firstSelection.SetMatched();
                secondSelection.SetMatched();
                matchedPairs++;
                GameManager.Instance.eventDispatcher.Dispatch(GameEvents.GAME_CARD_MATCH);
                ClearSelection();
                CheckForGameOver();
            }
            else
            {
                // mismatch
                GameManager.Instance.eventDispatcher.Dispatch(GameEvents.GAME_CARD_MISSMATCH);
                inputLocked = true;
                StartCoroutine(HideMismatchedAfterDelay());
            }
        }

        private IEnumerator HideMismatchedAfterDelay()
        {
            yield return new WaitForSeconds(mismatchHideDelay);
            if (firstSelection != null && !firstSelection.IsMatched)
            {
                firstSelection.ShowCardBack();
            }
            if (secondSelection != null && !secondSelection.IsMatched)
            {
                secondSelection.ShowCardBack();
            }
            ClearSelection();
            inputLocked = false;
        }

        private void ClearSelection()
        {
            firstSelection = null;
            secondSelection = null;
        }

        private async void CheckForGameOver()
        {
            if (matchedPairs >= totalPairs)
            {
                await UtilsAsync.Delay(500, () => {
                    GameManager.Instance.GameOver();
                });
               
            }
        }

        public void SetMatchedPairs(int count)
        {
            matchedPairs = count;
            CheckForGameOver();
        }
        
        public void RunInitialReveal()
        {
            if (!gameObject.activeInHierarchy)
                return;
            StopCoroutineSafe(initialRevealCoroutine);
            initialRevealCoroutine = StartCoroutine(InitialRevealRoutine());
        }

        private Coroutine initialRevealCoroutine;

        private IEnumerator InitialRevealRoutine()
        {
            inputLocked = true;

            // Show all card fronts
            foreach (var card in allCards)
            {
                if (card != null && !card.IsMatched)
                {
                    card.ShowCardFront();
                }
            }

            yield return new WaitForSeconds(initialRevealSeconds);

            // Hide all non-matched cards back
            foreach (var card in allCards)
            {
                if (card != null && !card.IsMatched)
                {
                    card.ShowCardBack();
                }
            }

            inputLocked = false;
        }

        private void StopCoroutineSafe(Coroutine routine)
        {
            if (routine != null)
            {
                StopCoroutine(routine);
            }
        }
    }
}