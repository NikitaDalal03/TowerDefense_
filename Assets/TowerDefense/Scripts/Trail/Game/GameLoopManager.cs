using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class GameLoopManager : MonoBehaviour
{
    public static GameLoopManager Instance { get; private set; }

    public List<TowerBehavior> towersInGame = new List<TowerBehavior>();
    public static Vector3[] nodePositions;
    public static float[] nodeDistances;

    private static Queue<ApplyEffectData> EffectsQueue;
    private static Queue<EnemyDamageData> DamageData;
    private static Queue<Enemy> enemiesToRemove;
    private static Queue<int> enemyIdsToSummon;
    private static Queue<EnemyGroup> enemyGroupsToSummon;

    private PlayersStats playerStatistics;

    public Transform nodeParent;
    public bool loopShouldEnd;

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


    void Start()
    {
        playerStatistics = FindAnyObjectByType<PlayersStats>();
        EffectsQueue = new Queue<ApplyEffectData>();
        DamageData = new Queue<EnemyDamageData>();
        towersInGame = new List<TowerBehavior>();
        enemyIdsToSummon = new Queue<int>();
        enemiesToRemove = new Queue<Enemy>();
        enemyGroupsToSummon = new Queue<EnemyGroup>();
        EntitySummoners.Init();

        nodePositions = new Vector3[nodeParent.childCount];
        for (int i = 0; i < nodePositions.Length; i++)
        {
            nodePositions[i] = nodeParent.GetChild(i).position;
        }

        nodeDistances = new float[nodePositions.Length - 1];
        for (int i = 0; i < nodeDistances.Length; i++)
        {
            nodeDistances[i] = Vector3.Distance(nodePositions[i], nodePositions[i + 1]);
        }

        //StartCoroutine(GameLoop());
        //InvokeRepeating("SummonTest", 0f, 5f);
        //InvokeRepeating("RemoveTest", 0f, 1.5f);
    }

    void SummonTest()
    {
        //EnqueueEnemyIDToSummon(1);
        Debug.Log("SummonTest");
        enemyGroupsToSummon.Enqueue(new EnemyGroup(EnemyType.Basic, 1));
        enemyGroupsToSummon.Enqueue(new EnemyGroup(EnemyType.Fast, 1));
        enemyGroupsToSummon.Enqueue(new EnemyGroup(EnemyType.Tank, 1));
        enemyGroupsToSummon.Enqueue(new EnemyGroup(EnemyType.Boss, 1));
    }

    //void RemoveTest()
    //{
    //    if(EntitySummoners.enemiesInGame.Count > 0)
    //    {
    //        EntitySummoners.RemoveEnemy(EntitySummoners.enemiesInGame[Random.Range(0, EntitySummoners.enemiesInGame.Count)]);
    //    }
    //}

  
    public void StartSpawning()
    {
        StartCoroutine(GameLoop());
        InvokeRepeating("SummonTest", 0f, 20f);
    }

    IEnumerator GameLoop()
    {
        while (loopShouldEnd == false)
        {

            //spawn enemies
            if (enemyGroupsToSummon.Count > 0)
            {
                EnemyGroup currentGroup = enemyGroupsToSummon.Dequeue();
                for (int i = 0; i < currentGroup.count; i++)
                {
                    EntitySummoners.SummonEnemy(currentGroup.enemyType);
                }
            }
            //spawn towers

            //move enemies
            NativeArray<Vector3> nodesToUse = new NativeArray<Vector3>(nodePositions, Allocator.TempJob);
            NativeArray<float> enemySpeeds = new NativeArray<float>(EntitySummoners.enemiesInGame.Count, Allocator.TempJob);
            NativeArray<int> nodeIndices = new NativeArray<int>(EntitySummoners.enemiesInGame.Count, Allocator.TempJob);
            TransformAccessArray EnemyAccess = new TransformAccessArray(EntitySummoners.enemiesInGameTransform.ToArray(), 2);

            for (int i = 0; i < EntitySummoners.enemiesInGame.Count; i++)
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

            for (int i = 0; i < EntitySummoners.enemiesInGame.Count; i++)
            {
                EntitySummoners.enemiesInGame[i].nodeIndex = nodeIndices[i];

                if (EntitySummoners.enemiesInGame[i].nodeIndex == nodePositions.Length)
                {
                    EnqueueEnemyToRemove(EntitySummoners.enemiesInGame[i]);
                }
            }

            enemySpeeds.Dispose();
            nodeIndices.Dispose();
            EnemyAccess.Dispose();
            nodesToUse.Dispose();

            //tick towers
            foreach (TowerBehavior tower in towersInGame)
            {
                tower.target = TowersTargetting.GetTarget(tower, TowersTargetting.TargetType.Close);
                tower.Tick();
            }

            //apply effects
            if (EffectsQueue.Count > 0)
            {
                for (int i = 0; i < EffectsQueue.Count; i++)
                {
                    ApplyEffectData currentDamageData = EffectsQueue.Dequeue();
                    Effect effectDuplicate = currentDamageData.EnemyToAffect.ActiveEffects.Find(x => x.EffectName == currentDamageData.EffectToApply.EffectName);

                    if (effectDuplicate == null)
                    {
                        currentDamageData.EnemyToAffect.ActiveEffects.Add(currentDamageData.EffectToApply);
                    }
                    else
                    {
                        effectDuplicate.ExpireTime = currentDamageData.EffectToApply.ExpireTime;
                    }
                }
            }

            //tick enemies
            foreach (Enemy currentEnemy in EntitySummoners.enemiesInGame)
            {
                currentEnemy.Tick();
            }

            //damage enemies
            if (DamageData.Count > 0)
            {
                for (int i = 0; i < DamageData.Count; i++)
                {
                    EnemyDamageData currentDamageData = DamageData.Dequeue();
                    currentDamageData.targetedEnemy.health -= currentDamageData.totalDamage / currentDamageData.Resistance;

                    if (currentDamageData.targetedEnemy.health <= 0f)
                    {
                        if (!enemiesToRemove.Contains(currentDamageData.targetedEnemy))
                        {
                            EnqueueEnemyToRemove(currentDamageData.targetedEnemy);
                            playerStatistics.AddMoney((int)currentDamageData.totalDamage);

                            GameWin.Instance?.EnemyDefeated();
                        }
                    }
                }
            }


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

    public static void EnqueueEffectsToApply(ApplyEffectData effectData)
    {
        EffectsQueue.Enqueue(effectData);
    }

    public static void EnqueueDamageData(EnemyDamageData damageData)
    {
        DamageData.Enqueue(damageData);
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

public struct EnemyGroup
{
    public EnemyType enemyType;  
    public int count;           

    public EnemyGroup(EnemyType type, int count)
    {
        this.enemyType = type;
        this.count = count;
    }
}

public class Effect
{
    public Effect(string effectName, float damageRate, float damage, float expireTime)
    {
        ExpireTime = expireTime;
        EffectName = effectName;
        DamageRate = damageRate;
        Damage = damage;
    }

    public string EffectName;

    public float DamageDelay;
    public float Damage;
    public float ExpireTime;
    public float DamageRate;
}

public struct ApplyEffectData
{
    public ApplyEffectData(Enemy enemyToAffect, Effect effectToApply)
    {
        EnemyToAffect = enemyToAffect;
        EffectToApply = effectToApply;
    }

    public Enemy EnemyToAffect;
    public Effect EffectToApply;
}

public struct EnemyDamageData
{
    public EnemyDamageData(Enemy target, float damage, float resistance)
    {
        targetedEnemy = target;
        totalDamage = damage;
        Resistance = resistance;
    }

    public Enemy targetedEnemy;
    public float totalDamage;
    public float Resistance;
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
        // Ensure the enemy is not at the last node and still has a next node to move to
        if (nodeIndex[index] < nodepositions.Length)
        {
            Vector3 directionToNextNode = nodepositions[nodeIndex[index]] - transform.position;

            if (directionToNextNode != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(directionToNextNode);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, deltaTime * 5f);

                transform.position = Vector3.MoveTowards(transform.position, nodepositions[nodeIndex[index]], EnemySpeed[index] * deltaTime);
            }

            //if reach current move to next
            if (Vector3.Distance(transform.position, nodepositions[nodeIndex[index]]) < 0.1f)
            {
                nodeIndex[index]++;
            }
        }
    }
}