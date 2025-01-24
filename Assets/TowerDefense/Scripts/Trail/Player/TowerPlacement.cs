using UnityEngine;
using TMPro; 

public class TowerPlacement : MonoBehaviour
{
    [Header("Camera and Layers")]
    public Camera playerCamera;
    public LayerMask placementLayerMask;
    public LayerMask collisionLayerMask;

    [Header("Player Stats")]
    public PlayersStats playerStats;

    [Header("UI")]
    public GameObject notEnoughMoneyText; 

    private GameObject currentPlacingTower;
    private bool isPlacing = false;

    public void SetTowerToPlace(GameObject towerPrefab)
    {
        if (towerPrefab == null)
        {
            return;
        }

        if (isPlacing)
        {
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
                    return;
                }

                if (!hitCollider.CompareTag("CanPlace"))
                {
                    return;
                }

                Vector3 placementCenter = hitCollider.bounds.center;
                BoxCollider towerCollider = currentPlacingTower.GetComponent<BoxCollider>();

                if (towerCollider == null)
                {
                    return;
                }

                //float towerHalfHeight = towerCollider.size.y / 2f;
                float placementY = hitCollider.bounds.max.y;// + towerHalfHeight;

                Vector3 placementPosition = new Vector3(placementCenter.x, placementY, placementCenter.z);
                currentPlacingTower.transform.position = placementPosition;

                bool hasValidTag = hitCollider.CompareTag("CanPlace");
                bool isOverlap = false;

                bool hasEnoughMoney = playerStats != null && playerStats.HasEnoughMoney(GetSelectedTowerCost());
                bool canPlace = hasValidTag && hasEnoughMoney;

                // Visual feedback for valid placement
                Renderer renderer = currentPlacingTower.GetComponent<Renderer>();
                if (renderer != null)
                {
                    renderer.material.color = canPlace ? Color.green : Color.red;
                }

                if (Input.GetMouseButtonDown(0) && canPlace)
                {
                    PlaceTower();
                }

                // Show the "Not Enough Money" message if the player can't afford the tower
                if (Input.GetMouseButtonDown(0) && !canPlace)
                {
                    ShowNotEnoughMoneyMessage();
                }

                // Cancel placement right mouse or Escape 
                if (Input.GetMouseButtonDown(1) || Input.GetKeyDown(KeyCode.Escape))
                {
                    CancelPlacement();
                }
            }
            else
            {
                Debug.Log("HandlePlacement: Raycast did not hit a valid placement area.");
            }
        }
    }

    private void PlaceTower()
    {
        if (currentPlacingTower == null)
        {
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

                // Deduct the cost from the player's money only if the tower is successfully placed
                if (playerStats.SubtractMoney(towerBehavior.SummonCost))
                {
                    Debug.Log($"Placed {towerBehavior.gameObject.name} for {towerBehavior.SummonCost} coins.");
                }
                else
                {
                    Debug.LogWarning("Not enough money.");
                    CancelPlacement();
                }
            }
            else
            {
                Debug.LogError("GameLoopManager instance is null.");
            }
        }
        else
        {
            Debug.LogWarning("TowerBehavior component is missing on the tower prefab.");
        }

        currentPlacingTower = null;
        isPlacing = false;
    }

    private void CancelPlacement()
    {
        if (currentPlacingTower != null)
        {
            Destroy(currentPlacingTower);
            currentPlacingTower = null;
        }

        isPlacing = false;
        HideNotEnoughMoneyMessage();
    }

    private void ShowNotEnoughMoneyMessage()
    {
        if (notEnoughMoneyText != null)
        {
            notEnoughMoneyText.gameObject.SetActive(true); 
            Invoke("HideNotEnoughMoneyMessage", 2f); 
        }
    }

    private void HideNotEnoughMoneyMessage()
    {
        if (notEnoughMoneyText != null)
        {
            notEnoughMoneyText.gameObject.SetActive(false); 
        }
    }

    private float GetSelectedTowerCost()
    {
        if (currentPlacingTower == null)
            return 0f;

        TowerBehavior tempTower = currentPlacingTower.GetComponent<TowerBehavior>();
        if (tempTower != null)
        {
            return tempTower.SummonCost;
        }

        return 0f;
    }
}
