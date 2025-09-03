using UnityEngine;
// using DG.Tweening;
using System;

namespace CyberSpeed.CardsMatchGame
{
    [RequireComponent(typeof(CanvasGroup))]
    public class UIScreenBase : MonoBehaviour
    { 
        private CanvasGroup uiCanvasGroup;
       private void Awake()
        {
            Debug.Log("UIScreenBase: Awake() called");

            uiCanvasGroup = GetComponent<CanvasGroup>();

            if (uiCanvasGroup == null)
            {
                Debug.LogError("CanvasGroup component not found!");
                enabled = false;
                return;
            }

            // Initially hide the UI
            //          Hide();
        }

        public void ShowWithDuration(float duration, Action onComplete = null)
        {
            // uiCanvasGroup.DOFade(1f, duration)
            //     .SetUpdate(true)
            //     .OnStart(() =>
            //     {
            //         uiCanvasGroup.interactable = true;
            //         uiCanvasGroup.blocksRaycasts = true;
            //     })
            //     .OnComplete(() =>
            //     {
            //         onComplete?.Invoke(); 
            //     });
        }

        public void HideWithDuration(float duration, Action onComplete = null)
        {
            // uiCanvasGroup.DOFade(0f, duration)
            //     .SetUpdate(true)
            //     .OnStart(() =>
            //     {
            //         uiCanvasGroup.interactable = false;
            //         uiCanvasGroup.blocksRaycasts = false;
            //     })
            //     .OnComplete(() =>
            //     {
            //         onComplete?.Invoke(); 
            //     });
        }

        // Show the UI
        public void Show()
        {
            Debug.Log("UIScreenBase: Show() called");

            // uiCanvasGroup.alpha = 1f;
            // uiCanvasGroup.interactable = true;
            // uiCanvasGroup.blocksRaycasts = true;
            
            gameObject.SetActive(true);
        }

        // Hide the UI
        public void Hide()
        {
            // uiCanvasGroup.alpha = 0f;
            // uiCanvasGroup.interactable = false;
            // uiCanvasGroup.blocksRaycasts = false;
            
            gameObject.SetActive(false);
        }

        public bool IsUiVisible()
        {
            return uiCanvasGroup.alpha == 1;
        }
    }
}
