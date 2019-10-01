using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;

    public float speed = 5.0f;
    public float gravity = 20.0f;
    
    private Vector3 moveDirection = Vector3.zero;
    
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
            // We are grounded, so recalculate move direction directly from axes
            moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0.0f, Input.GetAxis("Vertical"));
            moveDirection *= speed;
        }

        // Apply gravity to the player
        moveDirection.y -= gravity * Time.deltaTime;
        
        // Move the player
        characterController.Move(moveDirection * Time.deltaTime);
    }
}
