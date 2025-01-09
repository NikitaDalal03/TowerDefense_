using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [Header("Wave Settings")]
    [SerializeField] private float timeBetweenWaves = 5f; 
    [SerializeField] private int initialEnemyCount = 5; 
    [SerializeField] private int additionalEnemiesPerWave = 5; 

    private int currentWave = 1;
    private bool isSpawningWave = false;

    private float waveTimer = 0f;

    private void Start()
    {
        StartCoroutine(WaveCoroutine());
    }

    private IEnumerator WaveCoroutine()
    {
        while (true) 
        {
            if (!isSpawningWave)
            {
                isSpawningWave = true;
                waveTimer = 0f; 
                StartCoroutine(SpawnWave(currentWave)); 
                currentWave++;
            }

            waveTimer += Time.deltaTime;
            if (waveTimer >= timeBetweenWaves)
            {
                isSpawningWave = false;
            }

            yield return null;
        }
    }

    private IEnumerator SpawnWave(int waveNumber)
    {
        int enemyCount = initialEnemyCount + (waveNumber - 1) * additionalEnemiesPerWave; 
        for (int i = 0; i < enemyCount; i++)
        {
            int enemyId = 1; 
            EntitySummoners.SummonEnemy(enemyId);
            
            yield return new WaitForSeconds(1f); 
        }
    }
}
