using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float speed = 5f;
    public int health = 100;
    public int damage = 10;

    public abstract void MoveAlongPath();


    public void TakeDamage(int amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}


