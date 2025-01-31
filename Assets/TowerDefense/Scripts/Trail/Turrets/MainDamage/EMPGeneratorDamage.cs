using System.Collections.Generic;
using UnityEngine;

public class EMPGeneratorDamage : MonoBehaviour, IDamageMethod
{
    [SerializeField] float rotationSpeed = 200f;
    [SerializeField] Transform turretPivot;
    void Update()
    {
        if(turretPivot)
        {
            turretPivot.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
        }
    }

    public void Init(float damage, float fireRate)
    {

    }

    public void DamageTick(Enemy target)
    {
        if (target)
        {
            float slowDownDuration = 1f;
            float slowDownFactor = 0.1f;
            target.ApplySlowdown(slowDownDuration, slowDownFactor); 
        }
    }   
}