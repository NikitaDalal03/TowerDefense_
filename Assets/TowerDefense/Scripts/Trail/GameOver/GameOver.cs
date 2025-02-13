using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameOver : MonoBehaviour
{
    public static GameOver Instance { get; private set; }

    public int playerHealth = 20;  
    //private int remainingEnemies;  
    public TextMeshProUGUI countText;   

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
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
        countText.text = $"{playerHealth}";
    }

    private void OnEnemyReachedEnd()
    {
        //remainingEnemies--;
        playerHealth--;

        if (countText != null)
        {
            //countText.text = $"{remainingEnemies}/{maxEnemiesAllowed}";
            countText.text = $"{playerHealth}";
        }


        if (playerHealth <= 0)
        {
            Debug.Log("GameOver");
            UIManager.instance.SwitchScreen(GameScreens.GameOver);
        }
    }
}
