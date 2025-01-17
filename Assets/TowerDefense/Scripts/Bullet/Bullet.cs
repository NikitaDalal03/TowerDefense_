using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private int bulletForce;

    private string returnToPoolMethodName = "ReturnBulletToPool";


    public void ShootBullet(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        rb.velocity = direction * bulletForce; 
        DeactivateBullet();
    }


    public void DeactivateBullet()
    {
        Invoke(returnToPoolMethodName, 2f); 
    }


    public void ReturnBulletToPool()
    {
        rb.velocity = Vector3.zero;
        BulletPool.instance.ReturnToPool(this);
    }
}
