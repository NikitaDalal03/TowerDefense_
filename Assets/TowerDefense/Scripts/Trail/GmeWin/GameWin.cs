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
            OnGameWon?.Invoke();
            UIManager.instance.SwitchScreen(GameScreens.Win);
            //gameLoopManager.loopShouldEnd = true;
        }
    }
}
