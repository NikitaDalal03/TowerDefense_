using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RocketPlatformDamage : MonoBehaviour, IDamageMethod
{
    [SerializeField] private ParticleSystem shotParticle;
    private float damage;
    private float fireRate;
    private float delay;

    public Transform bulletAnchor;

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

            shotParticle.Play();
            Shoot(target.rootPart.position);
            target.PlayBloodEffect();

            GameLoopManager.EnqueueDamageData(new EnemyDamageData(target, damage, target.damageResistance));

            float slowDownDuration = 0.5f;
            float slowDownFactor = 0.3f;
            target.ApplySlowdown(slowDownDuration, slowDownFactor);

            delay = 1f / fireRate;
        }
    }

    private void Shoot(Vector3 targetPosition)
    {
        Bullet bullet = BulletPool.instance.GetBullet();
        if (bullet != null)
        {
            bullet.transform.SetPositionAndRotation(bulletAnchor.transform.position, bulletAnchor.transform.rotation);
            bullet.gameObject.SetActive(true);

            bullet.ShootBullet(targetPosition);
        }
    }
}


