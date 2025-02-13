using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretData", menuName = "Turret/TurretData", order = 0)]
public class TurretData : ScriptableObject
{
    public string turretName;
    public GameObject turretPrefab;
    public float cost;
    public Sprite turretIcon; 

    // You can create a method to compare costs if needed
    public static int CompareByCost(TurretData a, TurretData b)
    {
        return a.cost.CompareTo(b.cost);
    }
}
