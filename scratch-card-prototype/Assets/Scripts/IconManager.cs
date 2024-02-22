using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    [Header("Gold")]
    public int totalGold = 0;
    public int revealedGold = 0;
    
    [Header("Curse")]
    public int totalCurse = 0;
    public int revealedCurse = 0;
    
    [Header("Treasure")]
    public int totalTreasure = 0;
    public int revealedTreasure = 0;

    private IconBase[] goldArray;
    private IconBase[] curseArray;
    private IconBase[] treasureArray;

    private UIManager uiManager;

    private void Awake()
    {
        uiManager = GameManager.Instance.UIManager;
    }

    void Start()
    {
        goldArray = transform.Find("Scratch Surface Sprite/Gold").GetComponentsInChildren<IconBase>();
        curseArray = transform.Find("Scratch Surface Sprite/Curse").GetComponentsInChildren<IconBase>();
        treasureArray = transform.Find("Scratch Surface Sprite/Treasure").GetComponentsInChildren<IconBase>();
        
        totalGold = transform.Find("Scratch Surface Sprite/Gold").childCount;
        totalCurse = transform.Find("Scratch Surface Sprite/Curse").childCount;
        totalTreasure = transform.Find("Scratch Surface Sprite/Treasure").childCount;
    }


    void Update()
    {
        revealedGold = GetNumberOfRevealedIcon(goldArray);
        revealedCurse = GetNumberOfRevealedIcon(curseArray);
        revealedTreasure = GetNumberOfRevealedIcon(treasureArray);
    }

    int GetNumberOfRevealedIcon(IconBase[] iconArray)
    {
        int number = 0;
        foreach (var icon in iconArray)
        {
            if (icon.isRevealed) number++;
        }
        return number;
    }

    private void OnEnable()
    {
        uiManager.showCurse += ShowCurse;
        uiManager.showGold += ShowGold;
    }
    
    private void OnDisable()
    {
        uiManager.showCurse -= ShowCurse;
        uiManager.showGold -= ShowGold;
    }

    void ShowCurse()
    {
        // Image answerPanelImage = uiManager.answerPanel.GetComponent<Image>();
        TextMeshProUGUI answerText = uiManager.answerPanel.GetComponentInChildren<TextMeshProUGUI>();
        answerText.text = "There are " + (totalCurse - revealedCurse) + " Unrevealed Curse Icon";

        // Sequence fadeAnswer = DOTween.Sequence();
        // fadeAnswer
        //     .AppendInterval(1)
        //     .Append(answerText.DOFade(0, 1f))
        //     .Insert(1, answerPanelImage.DOFade(0, 1f))
        //     .OnComplete((() => { answerPanelImage.gameObject.SetActive(false); }))
        //     .Rewind();
        // fadeAnswer.Play();
    }
    
    void ShowGold()
    {
        // Image answerPanelImage = uiManager.answerPanel.GetComponent<Image>();
        TextMeshProUGUI answerText = uiManager.answerPanel.GetComponentInChildren<TextMeshProUGUI>();
        answerText.text = "There are " + (totalGold - revealedGold) + " Unrevealed Gold Icon";

        // Sequence fadeAnswer = DOTween.Sequence();
        // fadeAnswer
        //     .AppendInterval(1)
        //     .Append(answerText.DOFade(0, 1f))
        //     .Insert(1, answerPanelImage.DOFade(0, 1f))
        //     .OnComplete((() => { answerPanelImage.gameObject.SetActive(false); }))
        //     .Rewind();
        // fadeAnswer.Play();
    }
}
