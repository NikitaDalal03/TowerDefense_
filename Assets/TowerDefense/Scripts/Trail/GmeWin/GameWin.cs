using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameWin : MonoBehaviour
{
    public static GameWin Instance { get; private set; }

    private int enemiesDefeated = 0;
    private bool gameWon = false;

    public GameLoopManager gameLoopManager;

    public delegate void WinEvent();
    public static event WinEvent OnGameWon;

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
    }

    //whenever an enemy is defeated
    public void EnemyDefeated()
    {
        if (gameWon) return;

        enemiesDefeated++;

        if (enemiesDefeated >= 100)
        {
            TriggerWin();
        }
    }

    // Trigger win condition
    private void TriggerWin()
    {
        gameWon = true;

        if (gameLoopManager != null)
        {
            gameLoopManager.loopShouldEnd = true;
        }

        OnGameWon?.Invoke();
        Debug.Log("You Win! Total Enemies Defeated: " + enemiesDefeated);
        // Show the win screen or UI here
        UIManager.instance.SwitchScreen(GameScreens.Win);
    }
}
