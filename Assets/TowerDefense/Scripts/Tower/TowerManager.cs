using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public TowerSlot[] towerSlots;  // Array of all the tower slots
    public CannonTower cannonTowerPrefab;  // Prefab for the cannon tower
    public ArcherTower archerTowerPrefab;  // Prefab for the archer tower

    void Start()
    {
        // Example of assigning a tower to a specific slot:
        towerSlots[0].SetTower(cannonTowerPrefab);  // Place a cannon tower at slot 0
        towerSlots[1].SetTower(archerTowerPrefab);  // Place an archer tower at slot 1
    }
}
