using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryManager : MonoBehaviour
{
    public EnemyFactoryA enemyFactoryA;
    public EnemyFactoryB enemyFactoryB;

    private Dictionary<string, IEnemyFactory> factoryDictionary;

    void Awake()
    {
        factoryDictionary = new Dictionary<string, IEnemyFactory>();
        factoryDictionary.Add("NormalEnemy", enemyFactoryA);
        factoryDictionary.Add("FastEnemy", enemyFactoryB);
    }

    public Enemy CreateEnemy(string type)
    {
        if (factoryDictionary.ContainsKey(type))
        {
            return factoryDictionary[type].CreateEnemy();
        }
        return null;
    }
}
