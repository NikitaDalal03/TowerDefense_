using UnityEngine;

public class EnemyB : Enemy
{
    public override void Start()
    {
        base.Start();
        health = 100f;
        speed = 5f;
    }
}
