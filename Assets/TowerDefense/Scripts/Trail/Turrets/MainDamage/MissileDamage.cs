using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileDamage : MonoBehaviour, IDamageMethod
{
    public LayerMask enemiesLayer;
    [SerializeField] private ParticleSystem missileSystem;
    [SerializeField] private ParticleSystem shotParticle;
    [SerializeField] private Transform towerHead;

    private ParticleSystem.MainModule missileSystemMain;
    public float damage;
    private float fireRate;
    private float delay;

    public void Init(float damage, float fireRate)
    {
        missileSystemMain = missileSystem.main;
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

            missileSystemMain.startRotationX = towerHead.forward.x;
            missileSystemMain.startRotationY = towerHead.forward.y;
            missileSystemMain.startRotationZ = towerHead.forward.z;

            missileSystem.Play();
            shotParticle.Play();

            float slowDownDuration = 0.5f;
            float slowDownFactor = 0.3f;
            target.ApplySlowdown(slowDownDuration, slowDownFactor);

            delay = 1f / fireRate;
        }
    }
}
