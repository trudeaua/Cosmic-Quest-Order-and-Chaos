using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem; // TODO switch to new input system

[RequireComponent(typeof(PlayerCombat))]
public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    public float rotationSpeed = 100.0f;
    public float gravity = 20.0f;

    private CharacterController characterController;
    private PlayerCombat playerCombat;

    private Vector2 moveInput;
    private Vector3 lookInput;

    private Vector3 moveDirection;
    private Vector3 lookDirection;
    
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        playerCombat = GetComponent<PlayerCombat>();
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

    private void OnDeviceLost()
    {
        Debug.Log("Input device lost connection");
    }

    private void OnDeviceRegained()
    {
        Debug.Log("Input device regained connection");
    }

    private void OnMove(InputValue value)
    {
        Debug.Log("Moving");
        /*if (characterController.isGrounded)
        {
            moveInput = value.Get<Vector2>();
            moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
            moveDirection *= speed;
        }*/
        
    }

    private void OnLook()
    {
        Debug.Log("Looking");
    }

    private void OnPrimaryAttack()
    {
        Debug.Log("Attacking");
        playerCombat.PrimaryAttack();
    }
}
