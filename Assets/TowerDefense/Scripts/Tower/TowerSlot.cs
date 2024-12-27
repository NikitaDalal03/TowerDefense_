using UnityEngine;

public class TowerSlot : MonoBehaviour
{
    public Tower currentTower;  // The current tower in the slot

    // Method to assign a tower to the slot
    public void SetTower(Tower tower)
    {
        if (currentTower != null)
        {
            Destroy(currentTower.gameObject);  // Destroy any existing tower
        }
        currentTower = Instantiate(tower, transform.position, Quaternion.identity);  // Place new tower at the slot
    }
}
