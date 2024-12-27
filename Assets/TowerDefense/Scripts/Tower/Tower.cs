using UnityEngine;

public abstract class Tower : MonoBehaviour
{
    public float attackRange = 5f;  
    public float attackCooldown = 1f;  
    protected float attackTimer = 0f;  

    public abstract void Attack(Enemy enemy);  

    public virtual void Update()
    {
        // Decrease the attack timer
        attackTimer -= Time.deltaTime;
    }

    // Check if an enemy is within range
    protected bool IsEnemyInRange(Enemy enemy)
    {
        return Vector3.Distance(transform.position, enemy.transform.position) <= attackRange;
    }
}
