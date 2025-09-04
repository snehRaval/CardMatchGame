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
            AudioManager.Instance.PlayButtonClick();

            if (GameManager.Instance.cardManager  != null)
                GameManager.Instance.cardManager.ReleaseAllCards();
            
            GameManager.Instance.InitialiseGame();
        }

        private void OnSaveAndHomeClick()
        {
            AudioManager.Instance.PlayButtonClick();
            GameManager.Instance.GameSave();
            OnHomeClick();
        }

        private void OnCancelClick()
        {
            AudioManager.Instance.PlayButtonClick();
            Show(false);
        }
    }
}