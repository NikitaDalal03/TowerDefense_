using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance { get; private set; }

    public int maxEnemiesAllowed = 20;  
    //private int remainingEnemies;  
    public TextMeshProUGUI countText;  
    public GameObject gameOverScreen;  

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        //remainingEnemies = maxEnemiesAllowed; 
    }

    void OnEnable()
    {
        Enemy.OnEnemyReachedEnd += OnEnemyReachedEnd;
    }

    void OnDisable()
    {
        Enemy.OnEnemyReachedEnd -= OnEnemyReachedEnd;
    }

    private void Start()
    {
        gameOverScreen.SetActive(false);
        countText.text = $"{maxEnemiesAllowed}";
    }

    private void OnEnemyReachedEnd()
    {
        //remainingEnemies--;
        maxEnemiesAllowed--;

        if (countText != null)
        {
            //countText.text = $"{remainingEnemies}/{maxEnemiesAllowed}";
            countText.text = $"{maxEnemiesAllowed}";
        }


        if (maxEnemiesAllowed <= 0)
        {
            GameOverLogic();
        }
    }

    private void GameOverLogic()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(true);
        }

        Time.timeScale = 0f;  
    }
}
