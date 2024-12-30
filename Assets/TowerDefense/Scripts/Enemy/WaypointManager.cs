using UnityEngine;

public class WaypointManager : MonoBehaviour
{
    public Transform[] waypoints;  

    // Method to return the waypoints
    public Transform[] GetWaypoints()
    {
        return waypoints;
    }
}
