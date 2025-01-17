using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    // Singleton pattern
    public static BulletPool instance;

    // Pool of bullets
    public Queue<Bullet> bulletPool = new Queue<Bullet>();

    public int poolSize = 10;
    public Bullet bulletPrefab;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    void Start()
    {
        for (int i = 0; i < poolSize; i++)
        {
            Bullet bullet = Instantiate(bulletPrefab);
            bullet.gameObject.SetActive(false); // Deactivate all bullets initially
            bulletPool.Enqueue(bullet);
        }
    }

    public Bullet GetBullet()
    {
        if (bulletPool.Count > 0)
        {
            Bullet bullet = bulletPool.Dequeue();
            return bullet;
        }
        else
        {
            Debug.LogWarning("Bullet pool is empty! Consider increasing pool size.");
            return null; 
        }
    }

    public void ReturnToPool(Bullet bullet)
    {
        bullet.gameObject.SetActive(false); 
        bulletPool.Enqueue(bullet); 
    }
}
