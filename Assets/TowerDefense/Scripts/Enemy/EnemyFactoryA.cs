using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFactoryA : MonoBehaviour, IEnemyFactory
{
    public Enemy CreateEnemy()
    {
        GameObject enemyObject = new GameObject("NormalEnemy");
        NormalEnemy enemy = enemyObject.AddComponent<NormalEnemy>();

        enemy.speed = 7f;
        enemy.health = 150;
        enemy.damage = 15;

        return enemy;
    }
}
