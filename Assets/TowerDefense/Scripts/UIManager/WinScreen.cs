using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : BaseScreen
{
    [SerializeField] Button playAgainButton;
    [SerializeField] Button nextButton;

    private void Start()
    {
        playAgainButton.onClick.AddListener(OnPlayAgain);
        nextButton.onClick.AddListener(OnNext);
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
    }

    public override void DeActivateScreen()
    {
        base.DeActivateScreen();
    }

    void OnPlayAgain()
    {
        //SoundManager.inst.PlaySound(SoundName.BtnClick);
    }

    void OnNext()
    {
        LevelManager.Instance.NextLevel();
        UIManager.instance.SwitchScreen(GameScreens.Play);
    }
}