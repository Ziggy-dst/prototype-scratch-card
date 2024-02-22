using System;
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
    public GameObject infoPanel;
    public GameObject answerPanel;
    
    //actions
    public Action showCurse;
    public Action showGold;

    public void InitializeUIElements(int treasureCount, int goldCount, int curseCount)
    {
        treasureText.text = $"Treasure Count: {treasureCount}";
        goldText.text = $"Gold Count: {goldCount}";
        curseText.text = $"Curse Count: {curseCount}";

        buyCardButton.interactable = true;

        endingPanel.SetActive(false);
        infoPanel.SetActive(false);
        answerPanel.SetActive(false);
    }

    public void ChangeTreasureCountUI(int newValue)
    {
        treasureText.text = $"Treasure Count: {newValue}";
    }

    public void ChangeGoldCountUI(int newValue)
    {
        goldText.text = $"Gold Count: {newValue}";
    }

    public void ChangeCurseLevelUI(int newValue)
    {
        curseText.text = $"Curse Level: {newValue}";
    }

    public void ChangeBuyCardButtonStates(bool ifInteractable)
    {
        buyCardButton.interactable = ifInteractable;
    }

    public void ShowEndingPanel()
    {
        endingPanel.SetActive(true);
    }

    public void ShowInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    public void ShowCurse()
    {
        answerPanel.SetActive(true);
        showCurse?.Invoke();
    }
    
    public void ShowGold()
    {
        answerPanel.SetActive(true);
        showGold?.Invoke();
    }

    public void HideInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}
