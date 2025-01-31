using UnityEngine;

[CreateAssetMenu(fileName = "NewTurretData", menuName = "Turret/TurretData", order = 0)]
public class TurretData : ScriptableObject
{
    public string turretName;
    public GameObject turretPrefab;
    public float cost;
}
