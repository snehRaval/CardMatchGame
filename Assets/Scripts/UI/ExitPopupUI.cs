using UnityEngine;
using UnityEngine.UI;
namespace CyberSpeed.CardsMatchGame
{
    public class ExitPopupUI : MonoBehaviour
    {
        [SerializeField] Button homeButton, saveAndHomeButton, cancelButton;

        public void Show(bool value)
        {
            gameObject.SetActive(value);
        }
        
        private void OnEnable()
        {
            homeButton.onClick.AddListener(OnHomeClick);
            saveAndHomeButton.onClick.AddListener(OnSaveAndHomeClick);
            cancelButton.onClick.AddListener(OnCancelClick);
        }

        private void OnDisable()
        {
            homeButton.onClick.RemoveListener(OnHomeClick);
            saveAndHomeButton.onClick.RemoveListener(OnSaveAndHomeClick);
            cancelButton.onClick.RemoveListener(OnCancelClick);
        }

        private void OnHomeClick()
        {
            if (GameManager.Instance.cardManager  != null)
                GameManager.Instance.cardManager .ReleaseAllCards();
            
            GameManager.Instance.InitialiseGame();
        }

        private void OnSaveAndHomeClick()
        {
            GameManager.Instance.GameSave();
            OnHomeClick();
        }

        private void OnCancelClick()
        {
            Show(false);
        }
    }
}