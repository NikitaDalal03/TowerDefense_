using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask placementCheckMask;
    [SerializeField] private LayerMask placementColliderMask;
    [SerializeField] private Camera playerCamera;
    private GameObject currentPlacingTower;

    void Start()
    {
        
    }

    void Update()
    {
        if(currentPlacingTower != null)
        {
            Ray camray = playerCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if(Physics.Raycast(camray, out hitInfo, 100f, placementColliderMask))
            {
                currentPlacingTower.transform.position = hitInfo.point;
            }

            if(Input.GetKeyDown(KeyCode.Q))
            {
                Destroy(currentPlacingTower);
                currentPlacingTower = null;
                return;
            }

            if(Input.GetMouseButtonDown(0) && hitInfo.collider.gameObject != null)
            {
                if(!hitInfo.collider.gameObject.CompareTag("CantPlace"))
                {
                    BoxCollider towerCollider = currentPlacingTower.gameObject.GetComponent<BoxCollider>();
                    towerCollider.isTrigger = true;

                    Vector3 boxCenter = currentPlacingTower.gameObject.transform.position + towerCollider.center;
                    Vector3 halfExtents = towerCollider.size / 2;

                    if (!Physics.CheckBox(boxCenter, halfExtents, Quaternion.identity, placementCheckMask, QueryTriggerInteraction.Ignore)) //ignore all trigger colliders
                    {
                        GameLoopManager.towersInGame.Add(currentPlacingTower.GetComponent<TowerBehavior>());
                        
                        towerCollider.isTrigger = false;
                        currentPlacingTower = null;
                    }                    
                }               
            }                   
        }
    }

    public void SetTowerToPlace(GameObject tower)
    {
        currentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
    }
}
