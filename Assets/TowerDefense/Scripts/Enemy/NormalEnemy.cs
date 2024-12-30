using UnityEngine;

public class NormalEnemy : Enemy
{
    public override void MoveAlongPath()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}


