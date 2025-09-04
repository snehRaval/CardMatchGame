using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CyberSpeed.CardsMatchGame
{
    public class CardUI : MonoBehaviour
    {
        public int CardValue { get; private set; }
        public bool IsFaceDown { get; private set; } // true when face-down
        public bool IsMatched { get; private set; } // already matched with other card
        public bool IsAnimating => flipCoroutine != null;

        [SerializeField] private Image imgFrontCard = null;
        [SerializeField] private Image imgBackCard = null;

        [SerializeField] private Button onClickCard = null;

        public Action<CardUI> Clicked;

        [SerializeField] private float flipDuration = 0.25f;
        [SerializeField] private AnimationCurve flipEase = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        private Coroutine flipCoroutine;

        private void Awake()
        {
            onClickCard = this.GetComponent<Button>();
        }

        private void OnEnable()
        {
            onClickCard.onClick.AddListener(OnClicked);
        }
        
        private void OnDisable()
        {
            onClickCard.onClick.RemoveListener(OnClicked);
        }

        public void Initialize(int index, Sprite frontSideImg)
        {
            this.CardValue = index;
            imgFrontCard.sprite = frontSideImg;
            // default state: face-down
            IsMatched = false;
            IsFaceDown = true;
            imgFrontCard.enabled = false;
            imgBackCard.enabled = true;
            // reset visual alpha in case it was marked matched before
            Color color = imgFrontCard.color;
            color.a = 1f;
            imgFrontCard.color = color;
            // reset transform scale for predictable flips
            var t = transform as RectTransform;
            if (t != null)
            {
                var s = t.localScale;
                s.x = 1f;
                t.localScale = s;
            }
        }
        
        public void Reset()
        {
            CardValue = 0;
            IsFaceDown = true;
            IsMatched = false;
            imgFrontCard.enabled = false;
            imgBackCard.enabled = true;
            //return to pool 
        }

        public void OnClicked()
        {
            // Ignore if already matched or already face-up
            if (IsMatched || IsFaceDown == false || IsAnimating)
                return;
            AudioManager.Instance.PlayCardFlip();
            Clicked?.Invoke(this);
        }

        public void ShowCardBack()
        {
            if (IsFaceDown && imgBackCard.enabled && !imgFrontCard.enabled)
                return;
            StartFlip(false);
        }

        public void ShowCardFront()
        {
            if (!IsFaceDown && imgFrontCard.enabled && !imgBackCard.enabled)
                return;
            StartFlip(true);
        }

        public void SetMatched()
        {
            IsMatched = true;
            Color color = imgFrontCard.color;
            color.a = 0.7f;
            imgFrontCard.color = color;
        }

        private void StartFlip(bool toFront)
        {
            if (!gameObject.activeInHierarchy)
            {
                ApplyInstantState(toFront);
                return;
            }

            if (flipCoroutine != null)
            {
                StopCoroutine(flipCoroutine);
                flipCoroutine = null;
            }
            flipCoroutine = StartCoroutine(FlipRoutine(toFront));
        }

        private void ApplyInstantState(bool toFront)
        {
            if (toFront)
            {
                imgBackCard.enabled = false;
                imgFrontCard.enabled = true;
                IsFaceDown = false;
            }
            else
            {
                imgBackCard.enabled = true;
                imgFrontCard.enabled = false;
                IsFaceDown = true;
            }
            var t = transform as RectTransform;
            if (t != null)
            {
                var s = t.localScale;
                s.x = 1f;
                t.localScale = s;
            }
        }

        private IEnumerator FlipRoutine(bool toFront)
        {
            var t = transform as RectTransform;
            if (t == null)
            {
                ApplyInstantState(toFront);
                yield break;
            }

            float elapsed = 0f;
            float half = Mathf.Max(0.0001f, flipDuration * 0.5f);

            // phase 1: scale X from 1 -> 0
            while (elapsed < half)
            {
                elapsed += Time.unscaledDeltaTime;
                float p = Mathf.Clamp01(elapsed / half);
                float eased = flipEase != null ? flipEase.Evaluate(p) : p;
                SetLocalXScale(Mathf.Lerp(1f, 0f, eased));
                yield return null;
            }
            SetLocalXScale(0f);

            // swap faces at mid flip
            if (toFront)
            {
                imgBackCard.enabled = false;
                imgFrontCard.enabled = true;
                IsFaceDown = false;
            }
            else
            {
                imgBackCard.enabled = true;
                imgFrontCard.enabled = false;
                IsFaceDown = true;
            }

            // phase 2: scale X from 0 -> 1
            elapsed = 0f;
            while (elapsed < half)
            {
                elapsed += Time.unscaledDeltaTime;
                float p = Mathf.Clamp01(elapsed / half);
                float eased = flipEase != null ? flipEase.Evaluate(p) : p;
                SetLocalXScale(Mathf.Lerp(0f, 1f, eased));
                yield return null;
            }
            SetLocalXScale(1f);

            flipCoroutine = null;
        }

        private void SetLocalXScale(float x)
        {
            var t = transform as RectTransform;
            if (t == null) return;
            var s = t.localScale;
            s.x = x;
            t.localScale = s;
        }
    }
}