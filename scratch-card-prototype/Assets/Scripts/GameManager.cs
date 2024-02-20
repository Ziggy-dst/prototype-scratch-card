using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    [HideInInspector] public GameState currentGameState = GameState.Start;

    // public Action OnGameEnds;

    public enum GameState
    {
        Menu,
        Start,
        Pause,
        End
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    void Start()
    {

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }
    }

    private void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void EndGame()
    {
        // UIManager.Instance.losePanel.SetActive(true);
        // OnGameEnds();
        // Restart();
    }
}