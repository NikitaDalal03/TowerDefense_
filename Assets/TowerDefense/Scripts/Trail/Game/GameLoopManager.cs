using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class GameLoopManager : MonoBehaviour
{
    public static List<TowerBehavior> towersInGame;

    public static Vector3[] nodePositions;
    public static float[] nodeDistances;
    private static Queue<Enemy> enemiesToRemove;
    private static Queue<int> enemyIdsToSummon;

    public Transform nodeParent;
    public bool loopShouldEnd;

    void Start()
    {
        towersInGame = new List<TowerBehavior>();
        enemyIdsToSummon = new Queue<int>();
        enemiesToRemove = new Queue<Enemy>();
        EntitySummoners.Init();

        nodePositions = new Vector3[nodeParent.childCount];
        for(int i = 0; i < nodePositions.Length; i++)
        {
            nodePositions[i] = nodeParent.GetChild(i).position;
        }

        nodeDistances = new float[nodePositions.Length - 1];
        for (int i = 0; i < nodeDistances.Length; i++)
        {
            nodeDistances[i] = Vector3.Distance(nodePositions[i], nodePositions[i+1]);
        }

        StartCoroutine(GameLoop());
        InvokeRepeating("SummonTest", 0f, 1f);
        //InvokeRepeating("RemoveTest", 0f, 1.5f);

    }

    void SummonTest()
    {
        EnqueueEnemyIDToSummon(1);
    }

    //void RemoveTest()
    //{
    //    if(EntitySummoners.enemiesInGame.Count > 0)
    //    {
    //        EntitySummoners.RemoveEnemy(EntitySummoners.enemiesInGame[Random.Range(0, EntitySummoners.enemiesInGame.Count)]);
    //    }
    //}

    IEnumerator GameLoop()
    {
        while(loopShouldEnd == false)
        {
            //spawn enemies
            if(enemyIdsToSummon.Count > 0)
            {
                for(int i = 0; i < enemyIdsToSummon.Count; i++)
                {
                    EntitySummoners.SummonEnemy(enemyIdsToSummon.Dequeue()); 
                }
            }
            //spawn towers

            //move enemies
            NativeArray<Vector3> nodesToUse = new NativeArray<Vector3>(nodePositions, Allocator.TempJob);
            NativeArray<float> enemySpeeds = new NativeArray<float>(EntitySummoners.enemiesInGame.Count, Allocator.TempJob);
            NativeArray<int> nodeIndices = new NativeArray<int>(EntitySummoners.enemiesInGame.Count, Allocator.TempJob);           
            TransformAccessArray EnemyAccess = new TransformAccessArray(EntitySummoners.enemiesInGameTransform.ToArray(), 2);

            for(int i = 0; i< EntitySummoners.enemiesInGame.Count; i++)
            {
                enemySpeeds[i] = EntitySummoners.enemiesInGame[i].speed;
                nodeIndices[i] = EntitySummoners.enemiesInGame[i].nodeIndex;
            }

            MovesEnemiesJob MoveJob = new MovesEnemiesJob
            {
                nodepositions = nodesToUse,
                EnemySpeed = enemySpeeds,
                nodeIndex = nodeIndices,
                deltaTime = Time.deltaTime
            };

            JobHandle MoveJobHandle = MoveJob.Schedule(EnemyAccess);
            MoveJobHandle.Complete();

            for(int i = 0; i < EntitySummoners.enemiesInGame.Count; i++)
            {
                EntitySummoners.enemiesInGame[i].nodeIndex = nodeIndices[i];

                if(EntitySummoners.enemiesInGame[i].nodeIndex == nodePositions.Length)
                {
                    EnqueueEnemyToRemove(EntitySummoners.enemiesInGame[i]);
                }
            }

            enemySpeeds.Dispose();
            nodeIndices.Dispose();
            EnemyAccess.Dispose();
            nodesToUse.Dispose();

            //tick towers
            foreach(TowerBehavior tower in towersInGame)
            {
                tower.target = TowersTargetting.GetTarget(tower, TowersTargetting.TargetType.First);
                tower.Tick();
            }

            //apply effects


            //damage enemies


            //remove enemies
            if (enemiesToRemove.Count > 0)
            {
                for (int i = 0; i < enemiesToRemove.Count; i++)
                {
                    EntitySummoners.RemoveEnemy(enemiesToRemove.Dequeue());
                }
            }

            //remove towers

            yield return null;
        }
    }

    public static void EnqueueEnemyIDToSummon(int id)
    {
        enemyIdsToSummon.Enqueue(id);
    }

    public static void EnqueueEnemyToRemove(Enemy enemyToRemove)
    {
        enemiesToRemove.Enqueue(enemyToRemove);
    }
}


public struct MovesEnemiesJob : IJobParallelForTransform
{
    [NativeDisableParallelForRestriction]
    public NativeArray<Vector3> nodepositions;

    [NativeDisableParallelForRestriction]
    public NativeArray<int> nodeIndex;

    [NativeDisableParallelForRestriction]
    public NativeArray<float> EnemySpeed;

    public float deltaTime;


    public void Execute(int index, TransformAccess transform)
    {
        if(nodeIndex[index] < nodepositions.Length)
        {
           
        }

        Vector3 PositionToMoveTo = nodepositions[nodeIndex[index]];
        transform.position = Vector3.MoveTowards(transform.position, PositionToMoveTo, EnemySpeed[index] * deltaTime);

        if(transform.position == PositionToMoveTo)
        {
            nodeIndex[index]++;
        }                            
    }
}
