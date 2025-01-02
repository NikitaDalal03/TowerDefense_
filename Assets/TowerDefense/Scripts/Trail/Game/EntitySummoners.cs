using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySummoners : MonoBehaviour
{
    public static List<Transform> enemiesInGameTransform;
    public static List<Enemy> enemiesInGame;
    public static Dictionary<int, GameObject> enemyPrefabs;
    public static Dictionary<int, Queue<Enemy>> enemyObjectPools;

    private static bool isInitialized;

    public static void Init()
    {
        if(!isInitialized)
        {
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

    public static Enemy SummonEnemy(int enemyId)
    {
        Enemy SummonedEnemy = null;

        if(enemyPrefabs.ContainsKey(enemyId))
        {
            Queue<Enemy> referencedQueue = enemyObjectPools[enemyId];
            if(referencedQueue.Count > 0)
            {
                //dequeue enemy and initialized
                SummonedEnemy = referencedQueue.Dequeue();
                SummonedEnemy.Init();

                SummonedEnemy.gameObject.SetActive(true);
            }
            else
            {
                //instantiate new instance of enemy and initialize
                GameObject newEnemy = Instantiate(enemyPrefabs[enemyId], GameLoopManager.nodePositions[0], Quaternion.identity);
                SummonedEnemy = newEnemy.GetComponent<Enemy>();
                SummonedEnemy.Init();
            }
        }
        else
        {
            Debug.Log($"EntitySummoner : Enemy with ID of {enemyId} does not exists!");
            return null;
        }

        enemiesInGameTransform.Add(SummonedEnemy.transform);
        enemiesInGame.Add(SummonedEnemy);
        SummonedEnemy.ID = enemyId;
        return SummonedEnemy;
    }

    public static void RemoveEnemy(Enemy enemyToRemove)
    {
        enemyObjectPools[enemyToRemove.ID].Enqueue(enemyToRemove);
        enemyToRemove.gameObject.SetActive(false);
        enemiesInGameTransform.Remove(enemyToRemove.transform);
        enemiesInGame.Remove(enemyToRemove);      
    }
}
