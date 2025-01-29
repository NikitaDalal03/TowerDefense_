using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireTriggerManager : MonoBehaviour
{
    [SerializeField] private FlameThrowerDamage baseClass;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Enemy"))
        {
            Effect flameEffect = new Effect("Fire", baseClass.fireRate, baseClass.damage, 5f);
            ApplyEffectData effectData = new ApplyEffectData(EntitySummoners.enemyTransformPairs[other.transform.parent], flameEffect);
            GameLoopManager.EnqueueEffectsToApply(effectData);
        }
    }
}
