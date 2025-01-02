using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    public LayerMask EnemiesLayer;

    public Enemy target;
    public Transform towerPivote;

    public float damage;
    public float fireRate;
    public float range;

    private float delay;

    void Start()
    {
        delay = 1 / fireRate;
    }

    void Update()
    {
        
    }

    public void Tick()
    {
        if(target != null)
        {
            towerPivote.transform.rotation = Quaternion.LookRotation(target.transform.position - transform.position);
        }
    }
}
