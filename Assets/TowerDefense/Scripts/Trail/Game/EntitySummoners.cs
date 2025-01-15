using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoners : MonoBehaviour
{
    public static List<Transform> enemiesInGameTransform;
    public static List<Enemy> enemiesInGame;

    public static Dictionary<Transform, Enemy> enemyTransformPairs;
    public static Dictionary<int, GameObject> enemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> enemyObjectPools;

    private static bool isInitialized;

    public static void Init()
    {
        if(!isInitialized)
        {
            enemyTransformPairs = new Dictionary<Transform, Enemy>();
            enemiesInGame = new List<Enemy>();
            enemyPrefabs = new Dictionary<int, GameObject>();
            enemiesInGameTransform = new List<Transform>();
            enemyObjectPools = new Dictionary<int, Queue<Enemy>>();

            EnemySummonData[] enemies = Resources.LoadAll<EnemySummonData>("Enemies");
            Debug.Log(enemies[0].name);

            foreach (EnemySummonData enemy in enemies)
            {
                enemyPrefabs.Add(enemy.enemyId, enemy.enemyPrefab);
                enemyObjectPools.Add(enemy.enemyId, new Queue<Enemy>());
            }

            isInitialized = true;
        }
        else
        {
            Debug.Log("EntitySummoner : This class is already initialized");
        }
    }

    public static Enemy SummonEnemy(EnemyType enemyType)
    {
        EnemySummonData summonData = null;

        // Find the right EnemySummonData based on the EnemyType
        foreach (EnemySummonData data in Resources.LoadAll<EnemySummonData>("Enemies"))
        {
            if (data.enemyStats.type == enemyType)
            {
                summonData = data;
                break;
            }
        }

        if (summonData != null)
        {
            GameObject enemyPrefab = summonData.enemyPrefab;
            EnemySummonData enemyStats = summonData;

            // Instantiate the enemy and initialize
            GameObject newEnemy = Instantiate(enemyPrefab, GameLoopManager.nodePositions[0], Quaternion.identity);
            Enemy summonedEnemy = newEnemy.GetComponent<Enemy>();
            summonedEnemy.Init();
            summonedEnemy.maxHealth = enemyStats.enemyStats.health;
            summonedEnemy.health = enemyStats.enemyStats.health;
            summonedEnemy.speed = enemyStats.enemyStats.speed;

            // Add to game lists
            if (!enemiesInGame.Contains(summonedEnemy)) enemiesInGame.Add(summonedEnemy);
            if (!enemiesInGameTransform.Contains(summonedEnemy.transform)) enemiesInGameTransform.Add(summonedEnemy.transform);
            if (!enemyTransformPairs.ContainsKey(summonedEnemy.transform)) enemyTransformPairs.Add(summonedEnemy.transform, summonedEnemy);

            return summonedEnemy;
        }
        else
        {
            Debug.LogError("Enemy type not found!");
            return null;
        }
    }


    public static void RemoveEnemy(Enemy enemyToRemove)
    {
        enemyObjectPools[enemyToRemove.ID].Enqueue(enemyToRemove);
        enemyToRemove.gameObject.SetActive(false);

        enemyTransformPairs.Remove(enemyToRemove.transform);
        enemiesInGameTransform.Remove(enemyToRemove.transform);
        enemiesInGame.Remove(enemyToRemove);
    }
}
