using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : BaseScreen
{
    //[SerializeField] Button retryButton;
    [SerializeField] Button homeButton;



    private void Start()
    {
        //retryButton.onClick.AddListener(OnRetry);
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

    //void OnRetry()
    //{
    //    //SoundManager.inst.PlaySound(SoundName.BtnClick);
    //    UIManager.instance.SwitchScreen(GameScreens.Play);
    //}

    void OnHome()
    {
        //SoundManager.inst.PlaySound(SoundName.BtnClick);
        SceneManager.LoadScene(0);
        UIManager.instance.SwitchScreen(GameScreens.Play);
    }

}