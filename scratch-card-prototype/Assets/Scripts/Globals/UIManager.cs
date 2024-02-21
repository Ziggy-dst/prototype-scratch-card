using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    // resource text
    public TMP_Text treasureText;
    public TMP_Text goldText;
    public TMP_Text curseText;

    // interactable UIs
    public Button buyCardButton;

    // panels
    public GameObject endingPanel;

    public void InitializeUIElements(int treasureCount, int goldCount, int curseCount)
    {
        treasureText.text = $"Treasure Count: {treasureCount}";
        goldText.text = $"Gold Count: {goldCount}";
        curseText.text = $"Curse Count: {curseCount}";

        buyCardButton.interactable = true;

        endingPanel.SetActive(false);
    }

    public void ChangeTreasureCountUI(int newValue)
    {
        treasureText.text = $"Treasure Count: {newValue}";
    }

    public void ChangeGoldCountUI(int newValue)
    {
        treasureText.text = $"Treasure Count: {newValue}";
    }

    public void ChangeCurseLevelUI(int newValue)
    {
        treasureText.text = $"Curse Level: {newValue}";
    }

    public void ChangeBuyCardButtonStates(bool ifInteractable)
    {
        buyCardButton.interactable = ifInteractable;
    }

    public void ShowEndingPanel()
    {
        endingPanel.SetActive(true);
    }
}