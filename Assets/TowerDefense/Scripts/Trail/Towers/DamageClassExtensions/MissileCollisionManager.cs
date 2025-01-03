using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileCollisionManager : MonoBehaviour
{
    [SerializeField] private MissileDamage baseClass; 
    [SerializeField] private ParticleSystem explosionSystem;
    [SerializeField] private ParticleSystem missileSystem;
    [SerializeField] private float explosionRadius;

    private List<ParticleCollisionEvent> missileCollision;

    public void Start()
    {
        missileCollision = new List<ParticleCollisionEvent>();

    }

    private void OnParticleCollision(GameObject other)
    {
        missileSystem.GetCollisionEvents(other, missileCollision);

        for(int collisionevents = 0; collisionevents < missileCollision.Count; collisionevents++)
        {
            explosionSystem.transform.position = missileCollision[collisionevents].intersection;
            explosionSystem.Play();

            Collider[] enemiesInRadius = Physics.OverlapSphere(missileCollision[collisionevents].intersection, explosionRadius, baseClass.enemiesLayer);

            for(int i = 0; i < enemiesInRadius.Length; i++)
            {
                Enemy EnemyToDamage = EntitySummoners.enemyTransformPairs[enemiesInRadius[i].transform.parent];
                EnemyDamageData damageToApply = new EnemyDamageData(EnemyToDamage, baseClass.damage, EnemyToDamage.damageResistance);
                GameLoopManager.EnqueueDamageData(damageToApply);
            }
        }
    }
}
