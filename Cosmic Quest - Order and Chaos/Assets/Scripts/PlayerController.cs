using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// using UnityEngine.Experimental.Input; TODO switch to new input system

public class PlayerController : MonoBehaviour
{
    // public InputMaster controls;

    public float speed = 6.0f;
    public float rotationSpeed = 100.0f;
    public float gravity = 20.0f;

    private CharacterController characterController;

    private Vector3 moveDirection = Vector3.zero;
    private Vector3 lookDirection = Vector3.zero;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (characterController.isGrounded)
        {
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            lookDirection = new Vector3(Input.GetAxis("RightH"), 0.0f, Input.GetAxis("RightV"));

            if (lookDirection != Vector3.zero)
            {
                // Look direction from right joystick
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(lookDirection, Vector3.up), rotationSpeed * 5f * Time.deltaTime);
            }
            else if (moveDirection != Vector3.zero)
            {
                // Look direction towards movement direction
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), rotationSpeed * 5f * Time.deltaTime);
            }

            moveDirection *= speed;
        }

        // Apply gravity to the player
        moveDirection.y -= gravity * Time.deltaTime;
        
        // Move the player
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
