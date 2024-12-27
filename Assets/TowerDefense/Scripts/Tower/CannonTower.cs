using UnityEngine;

public class CannonTower : Tower
{
    public GameObject cannonballPrefab;  

    public override void Attack(Enemy enemy)
    {
        if (IsEnemyInRange(enemy) && attackTimer <= 0f)
        {

            FireCannonball(enemy);
            attackTimer = attackCooldown; 
        }
    }

    private void FireCannonball(Enemy enemy)
    {

        GameObject cannonball = Instantiate(cannonballPrefab, transform.position, Quaternion.identity);
        Projectile cannonballScript = cannonball.GetComponent<Projectile>();
        cannonballScript.Initialize(enemy, 20f);  
    }
}
