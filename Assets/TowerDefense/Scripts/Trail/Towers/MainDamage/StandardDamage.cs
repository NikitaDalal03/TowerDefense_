using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamageMethod
{
    public void DamageTick(Enemy target);
    public void Init(float damage, float fireRate);
}

public class StandardDamage : MonoBehaviour, IDamageMethod
{
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

            Shoot(target.transform.position);  
            GameLoopManager.EnqueueDamageData(new EnemyDamageData(target, damage, target.damageResistance));
            delay = 1f / fireRate;
        }
    }

    private void Shoot(Vector3 targetPosition)
    {
        Bullet bullet = BulletPool.instance.GetBullet();
        if (bullet != null)
        {
            bullet.transform.position = bulletAnchor.transform.position;
            bullet.transform.rotation = bulletAnchor.transform.rotation;
            bullet.gameObject.SetActive(true);

            bullet.ShootBullet(targetPosition);
        }
    }
}

