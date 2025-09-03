using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CardMatching.UI
{
    public class MainMenuUIHandler : MonoBehaviour
    {
        [SerializeField] Button playButton;
        [SerializeField] TMP_Dropdown gameLayoutDropDown;
        [SerializeField] GameLayoutScriptableObject gameLayoutSOAsset;

        private int rows, columns;
        private void Awake()
        {
            PopulateGameLayoutDropdown();
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
            foreach (var grid in gameLayoutSOAsset.gridLevels)
            {
                options.Add(grid.ToString());
            }
            
            gameLayoutDropDown.AddOptions(options);

            gameLayoutDropDown.value = 0;
            gameLayoutDropDown.RefreshShownValue();
        }
        
        public void OnDropdownValueChange(int value)
        {
            Debug.Log($"OnDropdownValueChange {value}");
            ( rows,  columns) = GetRowColumns(gameLayoutDropDown.options[gameLayoutDropDown.value].text);
            Debug.Log($"OnDropdownValueChange: rows {rows} columns {columns}");
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
            gameObject.SetActive(false);
            //Initialise grid of rows and columns
        }
    }
}