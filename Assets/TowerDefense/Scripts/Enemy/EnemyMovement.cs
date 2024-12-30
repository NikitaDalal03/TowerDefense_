using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    public float speed = 5f; 
    private int currentPointIndex = 0;  
    private Transform[] pathPoints;  

    private WaypointManager waypointManager;

    void Start()
    {

        waypointManager = FindObjectOfType<WaypointManager>();

        //If the WaypointManager is found, get the waypoints
        if (waypointManager != null)
        {
            pathPoints = waypointManager.GetWaypoints(); 
        }
    }

    void Update()
    {
        //If there are no waypoints, return early
        if (pathPoints == null || pathPoints.Length == 0) return;

        //Move the enemy towards the current waypoint
        Transform targetPoint = pathPoints[currentPointIndex];
        Vector3 direction = (targetPoint.position - transform.position).normalized;
        transform.position = Vector3.MoveTowards(transform.position, targetPoint.position, speed * Time.deltaTime);

        //If the enemy reaches the current waypoint, move to the next
        if (transform.position == targetPoint.position)
        {
            currentPointIndex = (currentPointIndex + 1) % pathPoints.Length;
        }
    }
}
