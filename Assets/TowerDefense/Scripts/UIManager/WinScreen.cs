using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WinScreen : BaseScreen
{
    [SerializeField] Button homeButton;


    private void Start()
    {
        homeButton.onClick.AddListener(OnHome);
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
    }

    public override void DeActivateScreen()
    {
        base.DeActivateScreen();
    }

    void OnHome()
    {
        //SoundManager.inst.PlaySound(SoundName.BtnClick);
        SceneManager.LoadScene(0);
    }

}