using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    public float health;
    public float speed;
    private int currentWaypointIndex = 0;  

    public virtual void Start()
    {
       
    }

    public void SetWaypoints()
    {
        currentWaypointIndex = 0;  
    }

    protected virtual void MoveAlongWaypoints()
    {
        if (GameController.Instance.waypoints == null || GameController.Instance.waypoints.Length == 0)
        return;

        Transform targetWaypoint = GameController.Instance.waypoints[currentWaypointIndex];
        transform.position = Vector3.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        if (transform.position == targetWaypoint.position)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= GameController.Instance.waypoints.Length)
            {
                Die();
            }
        }
    }

    public virtual void TakeDamage(float amount)
    {
        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }

    protected virtual void Update()
    {
        MoveAlongWaypoints();
    }
}
