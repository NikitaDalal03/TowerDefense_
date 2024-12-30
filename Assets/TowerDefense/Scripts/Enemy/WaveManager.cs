using UnityEngine;
using System.Collections;

public class WaveManager : MonoBehaviour
{
    public EnemyPool enemyPool;
    public EnemyFactoryManager enemyFactoryManager;
    public Transform spawnPoint;
    public float timeBetweenSpawns = 1f;

    private int waveNumber = 1;

    void Start()
    {
        StartCoroutine(SpawnWave());
    }

    IEnumerator SpawnWave()
    {
        int enemyCount = waveNumber * 2;  
        for (int i = 0; i < enemyCount; i++)
        {
            string enemyType = (i % 2 == 0) ? "NormalEnemy" : "FastEnemy";
            SpawnEnemy(enemyType);
            yield return new WaitForSeconds(timeBetweenSpawns);
        }

        waveNumber++;
        yield return new WaitForSeconds(3f);
        StartCoroutine(SpawnWave());
    }

    void SpawnEnemy(string enemyType)
    {
        Enemy enemy = enemyFactoryManager.CreateEnemy(enemyType);
        enemy.transform.position = spawnPoint.position;

        enemy.gameObject.SetActive(true);

        Debug.Log($"Spawned {enemyType} at position {enemy.transform.position}");
    }

}
