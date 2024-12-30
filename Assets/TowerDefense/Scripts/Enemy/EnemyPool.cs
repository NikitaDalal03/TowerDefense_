using System.Collections.Generic;
using UnityEngine;

public class EnemyPool : MonoBehaviour
{
    public GameObject normalEnemyPrefab;
    public GameObject fastEnemyPrefab;
    public int poolSize = 10;

    private Dictionary<string, Queue<GameObject>> poolDictionary;

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        InitializePool("NormalEnemy", normalEnemyPrefab);
        InitializePool("FastEnemy", fastEnemyPrefab);
    }

    private void InitializePool(string enemyType, GameObject prefab)
    {
        Queue<GameObject> pool = new Queue<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            Debug.Log("Enqueue");
            GameObject enemy = Instantiate(prefab);
            enemy.SetActive(false);
            pool.Enqueue(enemy);
        }

        poolDictionary[enemyType] = pool;
    }

    public GameObject GetEnemy(string enemyType)
    {
        if (poolDictionary.ContainsKey(enemyType) && poolDictionary[enemyType].Count > 0)
        {
            GameObject enemy = poolDictionary[enemyType].Dequeue();
            Debug.Log("Dequeue");
            enemy.SetActive(true);
            return enemy;
        }
        else
        {
            //Handle pool exhaustion by instantiating a new enemy
            Debug.Log("Spawning enemies");
            return Instantiate(enemyType == "NormalEnemy" ? normalEnemyPrefab : fastEnemyPrefab);
        }
    }

    public void ReturnToPool(GameObject enemy, string enemyType)
    {
        enemy.SetActive(false);
        poolDictionary[enemyType].Enqueue(enemy);
    }
}
