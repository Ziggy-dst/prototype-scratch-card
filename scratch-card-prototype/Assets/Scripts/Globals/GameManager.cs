using System;
using System.Collections;
using System.Collections.Generic;
using Managers;
using ScratchCardAsset;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = System.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public UIManager UIManager;
    [HideInInspector] public AudioManager AudioManager;
    private ScratchCardManager cardManager;

    public bool AllowPlayerInput { get; private set; }

    // game objects
    public List<GameObject> scratchCards;

    [Header("Threshold Value")]
    public int maxCurseLevel = 5;
    public float failProgressThreshold = 0.95f;

    [Header("Static Value")]
    // public List<int> scratchCardPrices;
    public int baseCardPrice = 3;
    public int cardPriceIncrement = 0;
    public Vector2 scratchCardSpawnPosition;
    public int curseToRemoveEachBuy = 1;

    [Header("Default Value")]
    public int defaultCurseLevel = 0;
    public int defaultGoldCount = 0;
    public int defaultTreasureCount = 0;

    // dynamic values
    public int currentCurseLevel { get; private set; }
    public int CurrentGoldCount { get; private set; }
    public int CurrentTreasureCount { get; private set; }
    [HideInInspector] public bool allGoldRevealed = false;

    private int numOfScratchCardBought = 0;
    private int nextScratchCardPrice;
    private GameObject currentScratchCardAsset;
    private GameObject currentScratchCard;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            UIManager = GetComponentInChildren<UIManager>();
            AudioManager = GetComponentInChildren<AudioManager>();
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
        UIManager.InitializeUIElements(CurrentTreasureCount, CurrentGoldCount, currentCurseLevel, nextScratchCardPrice);

        if (scratchCards.Count <= 1) UIManager.ChangeBuyCardButtonStates(false, nextScratchCardPrice);
        else
        {
            if (CurrentGoldCount < nextScratchCardPrice) UIManager.ChangeBuyCardButtonStates(false, nextScratchCardPrice);
        }
    }

    private void InitializeGameValues()
    {
        AllowPlayerInput = true;

        currentCurseLevel = defaultCurseLevel;
        CurrentGoldCount = defaultGoldCount;
        CurrentTreasureCount = defaultTreasureCount;

        numOfScratchCardBought = 0;
        nextScratchCardPrice = baseCardPrice;
        // nextScratchCardPrice = scratchCardPrices[numOfScratchCardBought];
        currentScratchCardAsset = scratchCards[numOfScratchCardBought];
    }

    /// <summary>
    /// end the game if the player could not win
    /// </summary>
    private void CheckIfPlayerCouldWin(float progress)
    {
        // check if the gold is enough
        if (CurrentGoldCount < nextScratchCardPrice)
        {
            // print("progress " + progress);
            // check if all gold are revealed on this card
            if (progress >= failProgressThreshold && allGoldRevealed)
            {
                if (CurrentGoldCount < nextScratchCardPrice) OnGameEnds();
            }
        }
    }

    /// <summary>
    /// randomly generate new cards
    /// </summary>
    /// <param name="spawnPosition"></param>
    private void GenerateScratchCards(Vector2 spawnPosition)
    {
        // randomly show new scratch card, destroy the old one
        int randomCardIndex = UnityEngine.Random.Range(0, scratchCards.Count);
        while (scratchCards.IndexOf(currentScratchCard) == randomCardIndex)
        {
            randomCardIndex = UnityEngine.Random.Range(0, scratchCards.Count);
        }
        Destroy(currentScratchCard);
        currentScratchCardAsset = scratchCards[randomCardIndex];

        if (cardManager != null) cardManager.Progress.OnProgress -= CheckIfPlayerCouldWin;
        currentScratchCard = Instantiate(currentScratchCardAsset, spawnPosition, Quaternion.identity);
        cardManager = currentScratchCard.GetComponent<ScratchCardManager>();
        cardManager.Progress.OnProgress += CheckIfPlayerCouldWin;
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
        if (CurrentGoldCount >= nextScratchCardPrice)
            UIManager.ChangeBuyCardButtonStates(true, nextScratchCardPrice);
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

    public void RemoveCurse(int amount)
    {
        currentCurseLevel -= amount;
        UIManager.ChangeCurseLevelUI(currentCurseLevel);
    }

    /// <summary>
    ///  triggered when buy card button is pressed
    /// </summary>
    public void BuyScratchCard()
    {
        // reset bool
        allGoldRevealed = false;

        numOfScratchCardBought++;

        // cost
        CurrentGoldCount -= nextScratchCardPrice;
        UIManager.ChangeGoldCountUI(CurrentGoldCount);

        // remove curse
        if (currentCurseLevel > 0) RemoveCurse(curseToRemoveEachBuy);

        // check if the button should be greyed out (if have enough money to buy a new card)
        if (CurrentGoldCount < nextScratchCardPrice) UIManager.ChangeBuyCardButtonStates(false, nextScratchCardPrice);

        // randomly generate new card
        GenerateScratchCards(scratchCardSpawnPosition);

        // check if the card runs out
        if (numOfScratchCardBought >= scratchCards.Count - 1) UIManager.ChangeBuyCardButtonStates(false, nextScratchCardPrice);
        else
        {
            // nextScratchCardPrice = scratchCardPrices[numOfScratchCardBought];
            nextScratchCardPrice += cardPriceIncrement;
        }
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