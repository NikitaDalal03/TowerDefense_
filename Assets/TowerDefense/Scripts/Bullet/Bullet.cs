using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int bulletForce;
    private int damage;

    private string returnToPoolMethodName = "ReturnBulletToPool";

    public void SetData(int damage)
    {
        this.damage = damage;
    }

    public void ShootBullet()
    {
        rb.AddForce(transform.forward * bulletForce);
        DeactivateBullet();
    }

    public void OnTriggerEnter(Collider other)
    {
        //EnemyHealth enemy = other.GetComponent<EnemyHealth>();
        //if (enemy != null)
        //{
        //    enemy.TakeDamage(damage);
        //    ReturnBulletToPool();
        //}
        //else
        //{
        //    Debug.Log("Enemy is null");
        //}
    }

    public void DeactivateBullet()
    {
        Invoke(returnToPoolMethodName, 20);
    }


    public void ReturnBulletToPool()
    {
        rb.velocity = Vector3.zero;
        BulletPool.instance.RetunToPool(this);
    }

}
