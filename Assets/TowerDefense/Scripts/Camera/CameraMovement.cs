using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{

    public float moveSpeed = 5f;

    private float screenEdgeBuffer = 50f;

    public Vector3 positionOffset = new Vector3(0f, 5f, -10f);

    public Vector3 rotationOffset = new Vector3(30f, 0f, 0f);


    public float minX = -10f; //left
    public float maxX = 10f;  //right
    public float minZ = -10f; //backward
    public float maxZ = 10f;  //forward

    void Start()
    {
        transform.position = positionOffset;
        transform.rotation = Quaternion.Euler(rotationOffset);
    }

    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;

        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        Vector3 direction = Vector3.zero;


        if (mousePosition.x < screenEdgeBuffer)
        {
            direction += Vector3.left;
        }

        if (mousePosition.x > screenWidth - screenEdgeBuffer)
        {
            direction += Vector3.right;
        }


        if (mousePosition.y > screenHeight - screenEdgeBuffer)
        {
            direction += Vector3.forward;
        }


        if (mousePosition.y < screenEdgeBuffer)
        {
            direction -= Vector3.forward;
        }

        Vector3 newPosition = transform.position + direction * moveSpeed * Time.deltaTime;

        newPosition.x = Mathf.Clamp(newPosition.x, minX, maxX);//x-axis(left-right)
        newPosition.z = Mathf.Clamp(newPosition.z, minZ, maxZ); //z-axis(forward-backward)

        transform.position = newPosition;
    }
}
