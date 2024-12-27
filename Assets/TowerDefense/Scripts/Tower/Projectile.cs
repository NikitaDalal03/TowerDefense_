using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float speed = 10f;  // Speed at which the projectile moves
    public float damage = 10f;  // Damage dealt by the projectile
    private Enemy targetEnemy;  // The enemy this projectile will hit

    private void Update()
    {
        if (targetEnemy != null)
        {
            // Move the projectile towards the target enemy
            transform.position = Vector3.MoveTowards(transform.position, targetEnemy.transform.position, speed * Time.deltaTime);

            // If the projectile reaches the enemy, deal damage
            if (transform.position == targetEnemy.transform.position)
            {
                HitEnemy();
            }
        }
        else
        {
            // If there's no target enemy, destroy the projectile (optional)
            Destroy(gameObject);
        }
    }

    // Initialize the projectile with a target enemy and damage
    public void Initialize(Enemy enemy, float projectileDamage)
    {
        targetEnemy = enemy;
        damage = projectileDamage;
    }

    // Method to handle hitting an enemy
    private void HitEnemy()
    {
        if (targetEnemy != null)
        {
            targetEnemy.TakeDamage(damage);  // Deal damage to the enemy
            Destroy(gameObject);  // Destroy the projectile after hitting the enemy
        }
    }
}
