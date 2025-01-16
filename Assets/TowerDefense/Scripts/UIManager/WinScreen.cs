using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WinScreen : BaseScreen
{
    [SerializeField] Button retryButton;
    [SerializeField] Button homeButton;
    [SerializeField] Button nextButton;


    private void Start()
    {
        retryButton.onClick.AddListener(OnRetry);
        homeButton.onClick.AddListener(OnHome);
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

    void OnRetry()
    {
        //SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Play);
    }

    void OnHome()
    {
        //SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Home);
    }

    void OnNext()
    {
        //SoundManager.inst.PlaySound(SoundName.BtnClick);
        UIManager.instance.SwitchScreen(GameScreens.Play);

    }

}