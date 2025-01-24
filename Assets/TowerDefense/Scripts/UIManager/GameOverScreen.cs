using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverScreen : BaseScreen
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
        SceneManager.LoadScene(0);
       // UIManager.instance.SwitchScreen(GameScreens.Play);
    }

}