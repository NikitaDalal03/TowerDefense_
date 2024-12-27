using UnityEngine;

public class EnemyFactory : MonoBehaviour
{
    public Enemy normalEnemy;  
    public Enemy fastEnemy; 

    public Enemy CreateEnemy(string enemyType)
    {
        switch (enemyType)
        {
            case "NormalEnemy":
                return Instantiate(normalEnemy);  
            case "FastEnemy":
                return Instantiate(fastEnemy);  
            default:
                Debug.LogError("Unknown enemy type: " + enemyType);
                return null;
        }
    }
}
