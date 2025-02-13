using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    public int CurrentLevel { get; private set; } = 1; 
    public event System.Action OnLevelChanged;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void NextLevel()
    {
        CurrentLevel++;
        OnLevelChanged?.Invoke(); 
    }

    public void SetLevel(int level)
    {
        CurrentLevel = level;
        OnLevelChanged?.Invoke();
    }

}
