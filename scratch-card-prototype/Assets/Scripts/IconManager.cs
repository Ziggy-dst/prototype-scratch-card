using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.UI;

[Serializable]
public class IconPrefab
{
    public GameObject icon;
    public int minCount;
    public int maxCount;
}

public class IconManager : MonoBehaviour
{
    [Header("Random Spawn Settings")]
    public List<IconPrefab> iconPrefabs; // Sprite的预制体
    private Bounds spawnArea; // 生成区域
    public float minDistance; // Sprite之间的最小间隔
    private List<Vector2> spawnedPositions = new List<Vector2>();
    public float boundOffset;

    [Header("Gold")]
    [HideInInspector] public int totalGold = 0;
    [HideInInspector] public int revealedGold = 0;
    
    [Header("Curse")]
    [HideInInspector] public int totalCurse = 0;
    [HideInInspector] public int revealedCurse = 0;
    
    [Header("Treasure")]
    [HideInInspector] public int totalTreasure = 0;
    [HideInInspector] public int revealedTreasure = 0;

    private List<IconBase> goldList = new List<IconBase>();
    private List<IconBase> curseList = new List<IconBase>();
    private List<IconBase> treasureList = new List<IconBase>();

    private UIManager uiManager;

    private void Awake()
    {
        uiManager = GameManager.Instance.UIManager;
    }

    void Start()
    {
        Transform scratchSprite = transform.Find("Scratch Surface Sprite");
        spawnArea = scratchSprite.GetComponent<SpriteRenderer>().sprite.bounds;
        SpawnIcons(scratchSprite);

        totalGold = goldList.Count;
        totalCurse = curseList.Count;
        totalTreasure = treasureList.Count;

        // goldArray = transform.Find("Scratch Surface Sprite/Gold").GetComponentsInChildren<IconBase>();
        // curseArray = transform.Find("Scratch Surface Sprite/Curse").GetComponentsInChildren<IconBase>();
        // treasureArray = transform.Find("Scratch Surface Sprite/Treasure").GetComponentsInChildren<IconBase>();
        //
        // totalGold = transform.Find("Scratch Surface Sprite/Gold").childCount;
        // totalCurse = transform.Find("Scratch Surface Sprite/Curse").childCount;
        // totalTreasure = transform.Find("Scratch Surface Sprite/Treasure").childCount;
    }


    void Update()
    {
        revealedGold = GetNumberOfRevealedIcon(goldList);
        revealedCurse = GetNumberOfRevealedIcon(curseList);
        revealedTreasure = GetNumberOfRevealedIcon(treasureList);

        if (totalGold == revealedGold) GameManager.Instance.allGoldRevealed = true;
    }

    private void SpawnIcons(Transform backgroundSprite)
    {
        foreach (var iconPrefab in iconPrefabs)
        {
            GameObject newIconParent = new GameObject(iconPrefab.icon.name);
            newIconParent.transform.SetParent(backgroundSprite);

            int randomIconCount = Random.Range(iconPrefab.minCount, iconPrefab.maxCount + 1);

            for (int i = 0; i < randomIconCount; i++)
            {
                Vector2 spawnPosition;
                bool positionFound;

                do
                {
                    positionFound = true;
                    // 在指定范围内随机生成位置
                    spawnPosition = new Vector2(Random.Range(spawnArea.min.x + boundOffset, spawnArea.max.x - boundOffset), Random.Range(spawnArea.min.y + boundOffset, spawnArea.max.y - boundOffset));

                    // 检查与已生成的Sprite之间的间隔
                    foreach (Vector2 pos in spawnedPositions)
                    {
                        if (Vector2.Distance(spawnPosition, pos) < minDistance)
                        {
                            positionFound = false;
                            break;
                        }
                    }
                } while (!positionFound);

                // 实例化新的Sprite
                GameObject newIcon = Instantiate(iconPrefab.icon, spawnPosition, Quaternion.identity);
                spawnedPositions.Add(spawnPosition);

                switch (newIcon.tag)
                {
                    case "Gold":
                        goldList.Add(newIcon.GetComponent<IconBase>());
                        break;
                    case "Curse":
                        curseList.Add(newIcon.GetComponent<IconBase>());
                        break;
                    case "Treasure":
                        treasureList.Add(newIcon.GetComponent<IconBase>());
                        break;
                }

                newIcon.transform.SetParent(newIconParent.transform);
            }
        }
    }

    private Vector2 ConvertToScratchCardTexturePosition(Vector2 position, SpriteRenderer cardSprite)
    {
        Vector3 scratchCardOrigin = new Vector2(cardSprite.transform.position.x - cardSprite.sprite.bounds.size.x / 2,
            cardSprite.transform.position.y - cardSprite.sprite.bounds.size.y / 2);
        Vector2 relativePos = transform.position - scratchCardOrigin;
        Vector2 uvPosition = new Vector2(relativePos.x / cardSprite.sprite.bounds.size.x, relativePos.y / cardSprite.sprite.bounds.size.y);

        Vector2 convertedPosition = new Vector2(Mathf.FloorToInt(uvPosition.x * cardSprite.sprite.texture.width),
            Mathf.FloorToInt(uvPosition.y * cardSprite.sprite.texture.height));
        // print(convertedPosition);

        return convertedPosition;
    }

    int GetNumberOfRevealedIcon(List<IconBase> iconList)
    {
        int number = 0;
        foreach (var icon in iconList)
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
