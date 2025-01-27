using UnityEngine;
using UnityEngine.UI;

public class PlayScreen : BaseScreen
{
    [SerializeField] Button waveBtn;
    private bool hasStartedSpawning = false;  

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
        if (!hasStartedSpawning)
        {
            GameLoopManager.Instance.StartSpawning();
            hasStartedSpawning = true;
            waveBtn.interactable = false; 
        }
    }

    public void ResetSpawningState()
    {
        hasStartedSpawning = false;
        waveBtn.interactable = true;
    }
}