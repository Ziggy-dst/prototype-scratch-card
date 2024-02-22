using System;
using System.Collections;
using System.Collections.Generic;
using ScratchCardAsset;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public UIManager UIManager;

    public bool AllowPlayerInput { get; private set; }

    // game objects
    public List<GameObject> scratchCards;

    [Header("Threshold Value")]
    public int maxCurseLevel = 5;
    public float failProgressThreshold = 0.95f;

    [Header("Static Value")]
    public List<int> scratchCardPrices;
    public Vector2 scratchCardSpawnPosition;

    [Header("Default Value")]
    public int defaultCurseLevel = 0;
    public int defaultGoldCount = 0;
    public int defaultTreasureCount = 0;

    // dynamic values
    private int currentCurseLevel;
    public int CurrentGoldCount { get; private set; }
    public int CurrentTreasureCount { get; private set; }

    private int numOfScratchCardBought = 0;
    private int currentScratchCardPrice;
    private GameObject currentScratchCardAsset;
    private GameObject currentScratchCard;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            UIManager = GetComponentInChildren<UIManager>();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        InitializeGame();
    }

    private void InitializeGame()
    {
        InitializeGameValues();
        GenerateScratchCards(scratchCardSpawnPosition);
        // print(currentScratchCard);

        // initialize UI
        UIManager.InitializeUIElements(CurrentTreasureCount, CurrentGoldCount, currentCurseLevel);
        if (scratchCards.Count <= 1) UIManager.ChangeBuyCardButtonStates(false);
        else UIManager.ChangeBuyCardButtonStates(true);
    }

    private void InitializeGameValues()
    {
        AllowPlayerInput = true;

        currentCurseLevel = defaultCurseLevel;
        CurrentGoldCount = defaultGoldCount;
        CurrentTreasureCount = defaultTreasureCount;

        numOfScratchCardBought = 0;
        currentScratchCardPrice = scratchCardPrices[numOfScratchCardBought];
        currentScratchCardAsset = scratchCards[numOfScratchCardBought];
    }

    private void GenerateScratchCards(Vector2 spawnPosition)
    {
        currentScratchCard = Instantiate(currentScratchCardAsset, spawnPosition, Quaternion.identity);
    }

    /// <summary>
    /// triggered when gold icon is revealed
    /// </summary>
    /// <param name="count"></param>
    public void OnGoldObtained(int count)
    {
        CurrentGoldCount += count;
        
        UIManager.ChangeGoldCountUI(CurrentGoldCount);

        // check if the button should be greyed out
        if (CurrentGoldCount >= currentScratchCardPrice)
            UIManager.ChangeBuyCardButtonStates(true);
    }

    /// <summary>
    /// triggered when a treasure icon is revealed
    /// </summary>
    /// <param name="count"></param>
    public void OnTreasureObtained(int count)
    {
        CurrentTreasureCount += count;

        // change the score on UI
        UIManager.ChangeTreasureCountUI(CurrentTreasureCount);
    }

    /// <summary>
    /// triggered when a curse icon is revealed
    /// </summary>
    /// <param name="count"></param>
    public void OnCursed(int count)
    {
        currentCurseLevel += count;
        UIManager.ChangeCurseLevelUI(currentCurseLevel);
        if (currentCurseLevel >= maxCurseLevel)
        {
            OnGameEnds();
        }
    }

    /// <summary>
    ///  triggered when buy card button is pressed
    /// </summary>
    public void BuyScratchCard()
    {
        // check if the gold is enough
        if (CurrentGoldCount < currentScratchCardPrice)
        {
            // TODO: check if all gold are revealed on this card
            if (currentScratchCardAsset.GetComponent<EraseProgress>().GetProgress() >= failProgressThreshold)
            {
                OnGameEnds();
                return;
            }
        }

        // check if the card runs out
        if (numOfScratchCardBought < scratchCards.Count - 1)
        {
            numOfScratchCardBought++;
        }
        else return;

        if (numOfScratchCardBought == scratchCards.Count - 1)
            UIManager.ChangeBuyCardButtonStates(false);

        // cost
        CurrentGoldCount -= currentScratchCardPrice;

        // check if the button should be greyed out
        if (CurrentGoldCount < currentScratchCardPrice) UIManager.ChangeBuyCardButtonStates(false);

        // show new scratch card, destroy the old one
        Destroy(currentScratchCard);
        currentScratchCardAsset = scratchCards[numOfScratchCardBought];
        GenerateScratchCards(scratchCardSpawnPosition);

        // change the price of next card
        currentScratchCardPrice = scratchCardPrices[numOfScratchCardBought];
    }

    private void OnGameEnds()
    {
        // lock player input
        AllowPlayerInput = false;

        // show the ending panel
        UIManager.ShowEndingPanel();
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }


}