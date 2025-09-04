using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CyberSpeed.CardsMatchGame
{
    public class MainMenuUIHandler : UIScreenBase
    {
        [SerializeField] Button playButton;
        [SerializeField] TMP_Dropdown gameLayoutDropDown;
        [SerializeField] SaveGameConfirmationPopup saveGameConfirmationPopup;
        private int rows, columns;
        
        private void Awake()
        {
            PopulateGameLayoutDropdown();
            Debug.Log(Application.persistentDataPath );
        }
        
        private void OnEnable()
        {
            gameLayoutDropDown.onValueChanged.AddListener(OnDropdownValueChange);
            playButton.onClick.AddListener(OnPlayButtonClick);
        }
        
        private void OnDisable()
        {
            gameLayoutDropDown.onValueChanged.RemoveListener(OnDropdownValueChange);
            playButton.onClick.RemoveListener(OnPlayButtonClick);
        }
        
        void PopulateGameLayoutDropdown()
        {
            gameLayoutDropDown.ClearOptions();

            List<string> options = new List<string>();
            var gridLevels = GameManager.Instance?.gameLayoutSOAsset.gridLevels;
            foreach (var grid in gridLevels)
            {
                options.Add(grid.ToString());
            }
            
            gameLayoutDropDown.AddOptions(options);

            gameLayoutDropDown.value = 0;
            gameLayoutDropDown.RefreshShownValue();
            OnDropdownValueChange(0);
        }
        
        public void OnDropdownValueChange(int value)
        {
            ( rows,  columns) = GetRowColumns(gameLayoutDropDown.options[gameLayoutDropDown.value].text);
            Debug.Log($"OnDropdownValueChange: rows {rows} columns {columns} for value {value}");
            AudioManager.Instance.PlayButtonClick();
        }

        private (int, int) GetRowColumns(string input)
        {
            string[] parts = input.Split('x');
            if (parts.Length == 2 && int.TryParse(parts[0], out int firstNumber) && int.TryParse(parts[1], out int secondNumber))
                return (firstNumber, secondNumber);
            else
                return (2, 2);
        }
        
        private void OnPlayButtonClick()
        {   
            string key = $"{rows}x{columns}";
            AudioManager.Instance.PlayButtonClick();
            if (GameManager.Instance.saveManager.HasSave(key))
            {
                saveGameConfirmationPopup.Init(rows, columns);
            }
            else
            {
                GameManager.Instance.StartGame(rows, columns);
            }
        }
    }
}