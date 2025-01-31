using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : BaseScreen
{
    [SerializeField] Button homeButton;
    [SerializeField] Button playAgainButton;

    private void Start()
    {
        homeButton.onClick.AddListener(OnHome);
        playAgainButton.onClick.AddListener(OnPlayAgain);
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
        UIManager.instance.SwitchScreen(GameScreens.Home);
    }

    void OnPlayAgain()
    {

    }
}