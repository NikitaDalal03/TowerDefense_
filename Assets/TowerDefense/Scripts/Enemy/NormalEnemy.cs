using UnityEngine;

public class EnemyA : Enemy
{
    public override void Start()
    {
        base.Start();
        health = 200f;
        speed = 3f;
    }
}
