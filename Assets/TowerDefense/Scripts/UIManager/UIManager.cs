using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameScreens
{
    Home,
    Play,
    GameOver,
    Win,
    LevelSelection
}
public enum GamePopUp
{
    SoundSetting,
}

public class UIManager : MonoBehaviour
{
    [SerializeField] BaseScreen currentScreen;
    public static UIManager instance;
    [SerializeField] List<BaseScreen> screens;

    private void Start()
    {
        instance = this;
        currentScreen.ActivateScreen();
    }

    //private void Update()
    //{
    //    currentScreen.TakeInput();
    //}

    public void SwitchScreen(GameScreens screen)
    {
        if (screen == currentScreen.screen) return;
        foreach (BaseScreen baseScreen in screens)
        {
            if (baseScreen.screen == screen)
            {
                baseScreen.ActivateScreen();
                currentScreen.DeActivateScreen();
                currentScreen = baseScreen;
            }
        }
    }
}