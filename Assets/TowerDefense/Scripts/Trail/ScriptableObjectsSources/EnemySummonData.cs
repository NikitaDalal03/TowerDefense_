using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Create EnemySummonData")]
public class EnemySummonData : ScriptableObject
{
    public GameObject enemyPrefab;  
    public int enemyId;             
    public EnemyStats enemyStats;  
}

[System.Serializable]
public class EnemyStats
{
    public EnemyType type;   
    public float health;     
    public float speed;    
}

public enum EnemyType
{
    Basic,
    Fast,
    Tank,
    Boss
}
