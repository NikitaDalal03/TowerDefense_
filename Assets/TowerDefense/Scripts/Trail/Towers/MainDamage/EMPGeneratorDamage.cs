using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EMPGeneratorDamage : MonoBehaviour, IDamageMethod
{
    private float damage;
    private float fireRate;
    private float delay;

    public void Init(float damage, float fireRate)
    {
        this.damage = damage;
        this.fireRate = fireRate;
        delay = 1f / fireRate;
    }

    public void DamageTick(Enemy target)
    {
        if (target)
        {
            if (delay > 0f)
            {
                delay -= Time.deltaTime;
                return;
            }


            GameLoopManager.EnqueueDamageData(new EnemyDamageData(target, damage, target.damageResistance));

            float slowDownDuration = 0.5f;
            float slowDownFactor = 0.3f;
            target.ApplySlowdown(slowDownDuration, slowDownFactor);

            delay = 1f / fireRate;
        }
    }
}