using UnityEngine;
using UnityEngine.UI;

public class LecelSelectionScreen : BaseScreen
{
    [SerializeField] Button backButton;
    [SerializeField] Button level1Button;

    private void Start()
    {
        backButton.onClick.AddListener(OnBack);
        level1Button.onClick.AddListener(OnLevel1Btn);
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
    }

    public override void DeActivateScreen()
    {
        base.DeActivateScreen();
    }

    void OnBack()
    {
        UIManager.instance.SwitchScreen(GameScreens.Home);
    }

    void OnLevel1Btn()
    {
        UIManager.instance.SwitchScreen(GameScreens.Play);
    }
}