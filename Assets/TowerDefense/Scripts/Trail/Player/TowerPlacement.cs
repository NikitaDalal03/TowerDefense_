using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [Header("Camera and Layers")]
    public Camera playerCamera;
    public LayerMask placementLayerMask;  // Layers where towers can be placed
    public LayerMask collisionLayerMask;  // Layers to check for overlapping towers

    [Header("Player Stats")]
    public PlayersStats playerStats;

    private GameObject currentPlacingTower;
    private bool isPlacing = false;


    public void SetTowerToPlace(GameObject towerPrefab)
    {
        if (towerPrefab == null)
        {
            Debug.LogWarning("SetTowerToPlace failed: Tower prefab is null.");
            return;
        }

        if (isPlacing)
        {
            Debug.Log("SetTowerToPlace: Canceling previous placement.");
            CancelPlacement();
        }

        currentPlacingTower = Instantiate(towerPrefab);
        isPlacing = true;

        // Change tower color to indicate placement mode
        Renderer towerRenderer = currentPlacingTower.GetComponent<Renderer>();
        if (towerRenderer != null)
        {
            towerRenderer.material.color = Color.yellow;
        }

        Debug.Log("SetTowerToPlace: Placing " + towerPrefab.name);
    }

    void Update()
    {
        HandlePlacement();
    }

    private void HandlePlacement()
    {
        if (isPlacing && currentPlacingTower != null)
        {
            Ray camRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(camRay, out hitInfo, 500f, placementLayerMask))
            {
                Collider hitCollider = hitInfo.collider;

                if (hitCollider == null)
                {
                    Debug.LogWarning("HandlePlacement: Hit collider is null.");
                    return;
                }

                if (!hitCollider.CompareTag("CanPlace"))
                {
                    Debug.LogWarning("HandlePlacement: Hit object is not tagged with 'CanPlace'.");
                    return;
                }

                Vector3 placementCenter = hitCollider.bounds.center;
                BoxCollider towerCollider = currentPlacingTower.GetComponent<BoxCollider>();

                if (towerCollider == null)
                {
                    Debug.LogWarning("HandlePlacement: Tower prefab is missing a BoxCollider.");
                    return;
                }

                float towerHalfHeight = towerCollider.size.y / 2f;
                float placementY = hitCollider.bounds.max.y + towerHalfHeight;

                Vector3 placementPosition = new Vector3(placementCenter.x, placementY, placementCenter.z);
                currentPlacingTower.transform.position = placementPosition;

                bool hasValidTag = hitCollider.CompareTag("CanPlace");
                bool isOverlap = false;

                bool hasEnoughMoney = playerStats != null && playerStats.HasEnoughMoney(GetSelectedTowerCost());
                bool canPlace = hasValidTag;

                // Visual feedback
                Renderer renderer = currentPlacingTower.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = canPlace ? Color.green : Color.red;
                }

                // Debug Logs for Placement Validation
                Debug.Log($"Placement Validation - HasValidTag: {hasValidTag}, IsOverlap: {isOverlap}, HasEnoughMoney: {hasEnoughMoney}, CanPlace: {canPlace}");

                // Place tower on left mouse click
                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    PlaceTower();
                }

                // Cancel placement with right mouse click or Escape key
                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                {
                    CancelPlacement();
                }
            }
            else
            {
                // Debug Log for Failed Raycast
                Debug.Log("HandlePlacement: Raycast did not hit a valid placement area.");
            }
        }
    }

    private void PlaceTower()
    {
        if (currentPlacingTower == null)
        {
            Debug.LogWarning("PlaceTower failed: No tower is currently being placed.");
            return;
        }

        BoxCollider towerCollider = currentPlacingTower.GetComponent<BoxCollider>();
        if (towerCollider != null)
        {
            towerCollider.isTrigger = false;
        }

        TowerBehavior towerBehavior = currentPlacingTower.GetComponent<TowerBehavior>();
        if (towerBehavior != null)
        {
            // Ensure GameLoopManager is properly referenced
            if (GameLoopManager.Instance != null)
            {
                GameLoopManager.Instance.towersInGame.Add(towerBehavior);
                playerStats.AddMoney(-towerBehavior.SummonCost);
                Debug.Log($"PlaceTower: Placed {towerBehavior.gameObject.name} for {towerBehavior.SummonCost} coins.");
            }
            else
            {
                Debug.LogError("PlaceTower failed: GameLoopManager instance is null.");
            }
        }
        else
        {
            Debug.LogWarning("PlaceTower: TowerBehavior component is missing on the tower prefab.");
        }

        // Reset placement state
        currentPlacingTower = null;
        isPlacing = false;
    }

    private void CancelPlacement()
    {
        if (currentPlacingTower != null)
        {
            Debug.Log("CancelPlacement: Destroying currentPlacingTower.");
            Destroy(currentPlacingTower);
            currentPlacingTower = null;
        }
        else
        {
            Debug.LogWarning("CancelPlacement called, but no tower was being placed.");
        }

        isPlacing = false;
    }

    private float GetSelectedTowerCost()
    {
        if (currentPlacingTower == null)
            return 0f;

        TowerBehavior tempTower = currentPlacingTower.GetComponent<TowerBehavior>();
        if (tempTower != null)
        {
            Debug.Log($"GetSelectedTowerCost: {tempTower.gameObject.name} costs {tempTower.SummonCost}.");
            return tempTower.SummonCost;
        }

        Debug.LogWarning("GetSelectedTowerCost: TowerBehavior component is missing.");
        return 0f;
    }
}