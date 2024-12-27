using UnityEngine;

public class ArcherTower : Tower
{
    public GameObject arrowPrefab;  // Prefab for the arrow projectile

    public override void Attack(Enemy enemy)
    {
        if (IsEnemyInRange(enemy) && attackTimer <= 0f)
        {
            // Fire an arrow towards the enemy
            FireArrow(enemy);
            attackTimer = attackCooldown;  // Reset attack cooldown
        }
    }

    private void FireArrow(Enemy enemy)
    {
        GameObject arrow = Instantiate(arrowPrefab, transform.position, Quaternion.identity);
        Arrow arrowScript = arrow.GetComponent<Arrow>();
        arrowScript.Initialize(enemy, 10f);  // Pass the target enemy and damage
    }
}
