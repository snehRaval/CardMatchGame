using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace CyberSpeed.CardsMatchGame
{
    public class SaveGameConfirmationPopup : MonoBehaviour
    {
        [SerializeField] Button yes_Button, no_Button;

        private int row, column;
        private string key;

        public void Init(int rows, int columns)
        {

            Show(true);
            row = rows;
            column = columns;
            key = $"{rows}x{columns}";
        }

        public void Show(bool value)
        {
            gameObject.SetActive(value);
        }

        private void OnEnable()
        {
            yes_Button.onClick.AddListener(LoadSavedGame);
            no_Button.onClick.AddListener(LoadNewGame);
        }

        private void OnDisable()
        {
            yes_Button.onClick.RemoveListener(LoadSavedGame);
            no_Button.onClick.RemoveListener(LoadNewGame);
        }

        private void LoadSavedGame()
        {
            Show(false);
            GameManager.Instance.LoadGame(key);
        }

        private void LoadNewGame()
        {
            Show(false);
            GameManager.Instance.saveManager.Delete(key); // wipe old save if starting new
            GameManager.Instance.StartGame(row, column);
        }

    }
}
