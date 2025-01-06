using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class TowersTargetting
{
    public enum TargetType
    {
        First,
        Last,   
        Close,
        Strong,
        Weak
    }

    public static Enemy GetTarget(TowerBehavior currenTower, TargetType TargetMethod)
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(currenTower.transform.position, currenTower.range, currenTower.EnemiesLayer);
        NativeArray<EnemyData> enemiesToCalculate = new NativeArray<EnemyData>(enemiesInRange.Length, Allocator.TempJob);
        NativeArray<Vector3> NodePositions = new NativeArray<Vector3>(GameLoopManager.nodePositions, Allocator.TempJob);
        NativeArray<float> NodeDistances = new NativeArray<float>(GameLoopManager.nodeDistances, Allocator.TempJob);
        NativeArray<int> enemyToIndex = new NativeArray<int>(new int[] { -1}, Allocator.TempJob);
        int EnemyIndexToReturn = -1;

        for (int i = 0; i < enemiesToCalculate.Length; i++)
        {
            Enemy currentEnemy = enemiesInRange[i].transform.parent.GetComponent<Enemy>();
            int EnemyIndexInList = EntitySummoners.enemiesInGame.FindIndex(x => x == currentEnemy);

            enemiesToCalculate[i] = new EnemyData(currentEnemy.transform.position, currentEnemy.nodeIndex, currentEnemy.health, EnemyIndexInList);
        }

        SearchForEnemy EnemySearchJob = new SearchForEnemy
        {
            _EnemiesToCalculate = enemiesToCalculate,
            _NodePositions = NodePositions,
            _NodeDistances = NodeDistances,
            _EnemyToIndex = enemyToIndex,
            CompareValue = Mathf.Infinity,
            TargetingType = (int)TargetMethod,
            TowerPosition = currenTower.transform.position
        };

        switch((int)TargetMethod)
        {
            case 0: //First
                EnemySearchJob.CompareValue = Mathf.Infinity;
                break;

            case 1: //Last
                EnemySearchJob.CompareValue = Mathf.NegativeInfinity;
                break;

            case 2: //Close
                goto case 0;

            case 3: //Strong
                goto case 1;

            case 4: //Weak
                goto case 0;
        }

        JobHandle dependency = new JobHandle();
        JobHandle SearchJobHandle = EnemySearchJob.Schedule(enemiesToCalculate.Length, dependency);
        SearchJobHandle.Complete();

        if(enemyToIndex[0] != -1)
        {
            EnemyIndexToReturn = enemiesToCalculate[enemyToIndex[0]].EnemyIndex;
            
            enemiesToCalculate.Dispose();
            NodePositions.Dispose();
            NodeDistances.Dispose();
            enemyToIndex.Dispose();
            return EntitySummoners.enemiesInGame[EnemyIndexToReturn];
        }

        enemiesToCalculate.Dispose();
        NodePositions.Dispose();
        NodeDistances.Dispose();
        enemyToIndex.Dispose();
        return null;
    }


    struct EnemyData
    {
        public EnemyData(Vector3 position, int nodeIndex, float hp, int enemyIndex)
        {
            EnemyPosition = position;
            EnemyIndex = enemyIndex;
            NodeIndex = nodeIndex;
            Health = hp;
        }

        public Vector3 EnemyPosition;
        public int EnemyIndex;
        public int NodeIndex;
        public float Health;
    }

    struct SearchForEnemy : IJobFor
    {
        public NativeArray<EnemyData> _EnemiesToCalculate;
        public NativeArray<Vector3> _NodePositions;
        public NativeArray<float> _NodeDistances;
        public NativeArray<int> _EnemyToIndex;

        public Vector3 TowerPosition;
        public float CompareValue;
        public int TargetingType;

        public void Execute(int index)
        {
            float currentEnemyDistanceToEnd = 0;
            float DistanceToEnemy = 0;
            switch (TargetingType)
            {
                case 0: //First
                    currentEnemyDistanceToEnd = GetDistanceToEnd(_EnemiesToCalculate[index]);
                    if (currentEnemyDistanceToEnd < CompareValue)
                    {
                        _EnemyToIndex[0] = index;
                        CompareValue = currentEnemyDistanceToEnd;
                        Debug.Log("CurrentEnemyDistanceToEnd:" + currentEnemyDistanceToEnd);
                    }
                    break;

                case 1: //Last
                    currentEnemyDistanceToEnd = GetDistanceToEnd(_EnemiesToCalculate[index]);
                    if (currentEnemyDistanceToEnd > CompareValue)
                    {
                        _EnemyToIndex[0] = index;
                        CompareValue = currentEnemyDistanceToEnd;
                    }
                    break;

                case 2: //Close
                    DistanceToEnemy = Vector3.Distance(TowerPosition, _EnemiesToCalculate[index].EnemyPosition);
                    if (DistanceToEnemy < CompareValue)
                    {
                        _EnemyToIndex[0] = index;
                        CompareValue = DistanceToEnemy;
                    }
                    break;

                case 3: //Strong                   
                    if (_EnemiesToCalculate[index].Health > CompareValue)
                    {
                        _EnemyToIndex[0] = index;
                        CompareValue = _EnemiesToCalculate[index].Health;
                    }
                    break;

                case 4: //Weak                  
                    if (_EnemiesToCalculate[index].Health < CompareValue)
                    {
                        _EnemyToIndex[0] = index;
                        CompareValue = _EnemiesToCalculate[index].Health;
                    }
                    break;
            }
        }

        private float GetDistanceToEnd(EnemyData EnemyToEvaluate)
        {
            float finalDistance = Vector3.Distance(EnemyToEvaluate.EnemyPosition, _NodePositions[EnemyToEvaluate.NodeIndex]);

            for(int i = EnemyToEvaluate.NodeIndex; i < _NodeDistances.Length; i++)
            {
                finalDistance += _NodeDistances[i];
            }

            return finalDistance;
        }
    }

}
