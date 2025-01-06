using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3 positionOffset = new Vector3(0f, 5f, -10f);
    public Vector3 rotationOffset = new Vector3(30f, 0f, 0f);

    private Vector3 velocity;
    private Vector3 playerMovementInput;
    private Vector3 playerMouseInput;

    private float xRot;

    [SerializeField] private Transform playerCamera;
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed;
    [SerializeField] private float sensitivity;

    void Start()
    {
        transform.position = positionOffset;
        transform.rotation = Quaternion.Euler(rotationOffset);
    }

    void Update()
    {
        playerMovementInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical"));
        playerMouseInput = new Vector3(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        MovePlayer();
        MovePlayerCamera();
    }

    public void MovePlayer()
    {
        Vector3 forward = transform.forward;  
        Vector3 right = transform.right;   

        forward.y = 0f;  
        right.y = 0f;    
        forward.Normalize();  
        right.Normalize();   

        Vector3 moveDirection = (forward * playerMovementInput.z + right * playerMovementInput.x).normalized;

        //if (Input.GetKey(KeyCode.Space))
        //{
        //    velocity.y = 1f;  // Jump
        //}
        //else if (Input.GetKey(KeyCode.LeftShift))
        //{
        //    velocity.y = -1f;  // Fall 
        //}

        controller.Move(moveDirection * speed * Time.deltaTime);
        controller.Move(velocity * Time.deltaTime);

        velocity.y = 0f;
    }

    public void MovePlayerCamera()
    {
        if (Input.GetMouseButtonDown(1))
        {
            transform.Rotate(0f, playerMouseInput.x * sensitivity, 0f);

            xRot -= playerMouseInput.y * sensitivity;
            xRot = Mathf.Clamp(xRot, -80f, 80f); 
            playerCamera.transform.localRotation = Quaternion.Euler(xRot, 0f, 0f);
        }
    }
}
