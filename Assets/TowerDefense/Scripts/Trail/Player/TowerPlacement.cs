using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacement : MonoBehaviour
{
    [SerializeField] private LayerMask placementCheckMask;
    [SerializeField] private LayerMask placementColliderMask;
    [SerializeField] private Camera playerCamera;
    [SerializeField] private PlayersStats playerStatics;
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
                    BoxCollider towerCollider = currentPlacingTower.GetComponent<BoxCollider>();
                    towerCollider.isTrigger = true;

                    Vector3 Center = currentPlacingTower.transform.position + towerCollider.center;
                    Vector3 halfExtents = towerCollider.size / 2f;

                    if (!Physics.CheckBox(Center, halfExtents, Quaternion.identity, placementCheckMask, QueryTriggerInteraction.Ignore)) //ignore all trigger colliders
                    {
                        TowerBehavior currentTowerBehavior = currentPlacingTower.GetComponent<TowerBehavior>();
                        GameLoopManager.towersInGame.Add(currentTowerBehavior);

                        playerStatics.AddMoney(-currentTowerBehavior.SummonCost);

                        towerCollider.isTrigger = false;
                        currentPlacingTower = null;
                    }                    
                }               
            }                   
        }
    }

    public void SetTowerToPlace(GameObject tower)
    {
        int towerSummonCost =  tower.GetComponent<TowerBehavior>().SummonCost;
        if (playerStatics.GetMoney() >= towerSummonCost)
        {
             currentPlacingTower = Instantiate(tower, Vector3.zero, Quaternion.identity);
        }
        else
        {
            Debug.Log("You need more money to perchase a" +tower.name);
        }
    }
}
