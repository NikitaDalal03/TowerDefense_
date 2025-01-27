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

    public void OnTriggerEnter(Collider other)
    {
        ReturnBulletToPool();        
    }


    public void DeactivateBullet()
    {
        Invoke(returnToPoolMethodName, 5f); 
    }


    public void ReturnBulletToPool()
    {
        rb.velocity = Vector3.zero;
        gameObject.transform.rotation = Quaternion.identity;
        gameObject.transform.position = Vector3.zero;
        BulletPool.instance.ReturnToPool(this);
    }
}
