using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    public Transform enemySpawnPoint;
    public Transform[] waypoints;  
    private EnemyFactory enemyFactory;

    public float enemySpawnInterval = 2f;
    private float enemySpawnTimer = 0f;

    private int enemyACount = 0;
    private int enemyBCount = 0;
    private const int enemiesPerType = 5;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        GameController.Instance.waypoints = waypoints; 
        enemyFactory = GetComponent<EnemyFactory>();
    }

    private void Update()
    {
        enemySpawnTimer -= Time.deltaTime;
        if (enemySpawnTimer <= 0f)
        {
            SpawnEnemy();
            enemySpawnTimer = enemySpawnInterval;
        }
    }

    private void SpawnEnemy()
    {
        string enemyType = "";

        if (enemyACount < enemiesPerType)
        {
            enemyType = "NormalEnemy";
            enemyACount++;
        }
        else if (enemyBCount < enemiesPerType)
        {
            enemyType = "FastEnemy";
            enemyBCount++;
        }

        if (enemyACount >= enemiesPerType && enemyBCount >= enemiesPerType)
        {
            enemyACount = 0;
            enemyBCount = 0;
        }

        Enemy newEnemy = enemyFactory.CreateEnemy(enemyType);

        if (newEnemy != null)
        {
            // Pass the waypoints (static reference) to the new enemy
            newEnemy.SetWaypoints();
            newEnemy.transform.position = enemySpawnPoint.position;
        }
    }
}
