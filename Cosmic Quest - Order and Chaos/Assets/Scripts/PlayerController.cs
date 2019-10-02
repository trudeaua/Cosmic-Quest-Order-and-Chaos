using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    public float speed = 6.0f;
    public float rotationSpeed = 500.0f;
    public float gravity = 20.0f;
    
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;
    
    // Start is called before the first frame update
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    private void Update()
    {
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            lookDirection = new Vector3(Input.GetAxis("RightH"), 0.0f, Input.GetAxis("RightV"));

            if (lookDirection != Vector3.zero)
            {
                // Look direction from right joystick
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDirection, Vector3.up), rotationSpeed * Time.deltaTime);
            }
            else if (moveDirection != Vector3.zero)
            {
                // Look direction towards movement direction
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), rotationSpeed * Time.deltaTime);
            }

            moveDirection *= speed;
        }

        // Apply gravity to the player
        moveDirection.y -= gravity * Time.deltaTime;
        
        // Move the player
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
