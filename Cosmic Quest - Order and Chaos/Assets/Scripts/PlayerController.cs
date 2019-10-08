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

    private GameControls controls;

    private CharacterController controller;
    private PlayerCombat playerCombat;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private Vector3 moveDirection;
    private Vector3 lookDirection;
    
    private void Awake()
    {
        controls = new GameControls();

        controls.Player.PrimaryAttack.performed += ctx => playerCombat.PrimaryAttack();

        controller = GetComponent<CharacterController>();
        playerCombat = GetComponent<PlayerCombat>();
    }

    private void Start()
    {
        // TEMP
        PlayerManager.instance.RegisterPlayer(gameObject);
    }

    private void Update()
    {
        if (controller.isGrounded)
        {
            // Input lag through callback function, so we poll the controls
            moveInput = controls.Player.Move.ReadValue<Vector2>();
            moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);

            lookInput = controls.Player.Look.ReadValue<Vector2>();
            lookDirection = new Vector3(lookInput.x, 0f, lookInput.y);

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

        moveDirection.y -= gravity * Time.deltaTime;
        
        // Move the player
        controller.Move(moveDirection * Time.deltaTime);
    }

    // TODO remove me?
    public void OnPlayerJoined()
    {
        PlayerManager.instance.RegisterPlayer(gameObject);
    }

    // TODO remove me?
    public void OnPlayerLeft()
    {
        PlayerManager.instance.DeregisterPlayer(gameObject);
    }

    public void OnDeviceLost()
    {
        Debug.Log("Input device lost connection");
    }

    public void OnDeviceRegained()
    {
        Debug.Log("Input device regained connection");
    }

    private void OnEnable()
    {
        controls.Player.Enable();
    }

    private void OnDisable()
    {
        controls.Player.Disable();
    }
}
