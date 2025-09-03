using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CyberSpeed.CardsMatchGame
{
    public class CardUI : MonoBehaviour
    {
        public int CardValue { get; private set; }
        public bool IsFlipped { get; private set; } // means open card
        public bool IsMatched { get; private set; } // already matched with other card

        [SerializeField] private Image imgFrontCard = null;
        [SerializeField] private Image imgBackCard = null;

        public void Initialize(int index, Sprite frontSideImg)
        {
            this.CardValue = index;
            imgFrontCard.sprite = frontSideImg;
        }

        public void Reset()
        {
            CardValue = 0;
            IsFlipped = true;
            IsMatched = false;
            imgFrontCard.enabled = false;
            imgBackCard.enabled = true;
            //return to pool 
        }

        public void OnClicked()
        {
            //to avoid accidental double click
            if (imgFrontCard.enabled || IsFlipped == false)
                return;
            imgFrontCard.enabled = true;
            IsFlipped = false;
            //play audio 
        }

        void ShowCardBack()
        {
            imgBackCard.enabled = true;
            imgFrontCard.enabled = false;
        }

        void ShowCardFront()
        {
            imgBackCard.enabled = false;
            imgFrontCard.enabled = true;
        }

        public void SetMatched()
        {
            IsMatched = true;
            Color color = imgFrontCard.color;
            color.a = 0.7f;
            imgFrontCard.color = color;
        }
    }
}
