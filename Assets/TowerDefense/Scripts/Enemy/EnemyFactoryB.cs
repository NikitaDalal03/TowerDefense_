using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryB : MonoBehaviour, IEnemyFactory
{
    public Enemy CreateEnemy()
    {
        GameObject enemyObject = new GameObject("FastEnemy");
        FastEnemy enemy = enemyObject.AddComponent<FastEnemy>();


        enemy.speed = 5f;
        enemy.health = 100;
        enemy.damage = 10;

        return enemy;
    }
}

