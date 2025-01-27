using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerBehavior : MonoBehaviour
{
    public LayerMask EnemiesLayer;

    public Enemy target;
    public Transform towerPivote;

    public int SummonCost = 100; 
    public float damage;
    public float fireRate;
    public float range;

    private float delay;

    [SerializeField] private ParticleSystem placementParticle;
    private bool hasPlayedPlacementEffect = false;

    private IDamageMethod currentDamageMethodClass;

    void Start()
    {
        currentDamageMethodClass = GetComponent<IDamageMethod>();
        if(currentDamageMethodClass == null)
        {
            Debug.LogError("TOWER: No damage class attached to given tower!");
        }
        else
        {
            currentDamageMethodClass.Init(damage, fireRate);
        }
               
        delay = 1 / fireRate;
    }
  

    public void Tick()
    {
        currentDamageMethodClass.DamageTick(target);

        if (target != null)
        {
            towerPivote.transform.rotation = Quaternion.LookRotation(target.rootPart.position - transform.position);
        }
    }


    private void OnDrawGizmos()
    {
        if(target != null)
        {
            Gizmos.DrawWireSphere(transform.position, range);
            Gizmos.DrawLine(towerPivote.position, target.rootPart.position);
        }
    }

    public void PlayPlacementEffect()
    {
        if (placementParticle != null && !hasPlayedPlacementEffect)
        {
            placementParticle.Play();
            hasPlayedPlacementEffect = true;
        }
    }
}
