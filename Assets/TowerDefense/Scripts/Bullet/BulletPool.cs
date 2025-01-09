using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public static BulletPool instance;

    public Bullet bulletPrefab;
    public List<Bullet> bullets;

    private int poolsize = 8;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        bullets = new List<Bullet>();
        Addbullets(poolsize);
    }

    public void Addbullets(int objectToSpawn)
    {
        for (int i = 0; i < objectToSpawn; i++)
        {
            Bullet currentBullet = Instantiate(bulletPrefab, transform);
            currentBullet.gameObject.SetActive(false);
            bullets.Add(currentBullet);
        }
    }

    public Bullet GetBullet()
    {
        if (bullets.Count <= 0)
            Addbullets(poolsize);

        Bullet getbullet = bullets[bullets.Count - 1];
        bullets.Remove(bullets[bullets.Count - 1]);
        return getbullet;
    }

    public void RetunToPool(Bullet bullet)
    {
        Bullet returnedBullet = bullet;
        returnedBullet.gameObject.SetActive(false);
        bullets.Add(returnedBullet);
    }
}