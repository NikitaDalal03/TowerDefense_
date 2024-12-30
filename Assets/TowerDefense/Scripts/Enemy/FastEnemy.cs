using UnityEngine;

public class FastEnemy : Enemy
{
    public override void MoveAlongPath()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
}
