using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : BaseScreen
{
    [SerializeField] Button waveBtn;

    private void Start()
    {
       waveBtn.onClick.AddListener(OnWave);
    }

    public override void ActivateScreen()
    {
        base.ActivateScreen();
    }

    public override void DeActivateScreen()
    {
        base.DeActivateScreen();
    }

    void OnWave()
    {
        GameLoopManager.Instance.StartSpawning();
    }

}