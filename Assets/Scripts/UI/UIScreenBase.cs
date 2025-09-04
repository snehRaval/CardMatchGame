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
            
        }
       
        // Show the UI
        public void Show()
        {
            Debug.Log("UIScreenBase: Show() called");
            gameObject.SetActive(true);
        }

        // Hide the UI
        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public bool IsUiVisible()
        {
            return gameObject.activeSelf;
        }
    }
}
