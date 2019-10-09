using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 6.0f;
    public float rotationSpeed = 10.0f;

    private Rigidbody rb;
    private Animator anim;

    private Vector2 moveInput;
    private Vector2 lookInput;

    private Vector3 moveDirection;
    private Vector3 lookDirection;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        // Ensure player is not kinematic
        rb.isKinematic = false;
    }

    private void OnDisable()
    {
        // Set kinematic when disabled so the player stops moving
        rb.isKinematic = true;
    }

    private void Start()
    {
        // TEMP
        PlayerManager.instance.RegisterPlayer(gameObject);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 moveDirection = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 lookDirection = new Vector3(lookInput.x, 0f, lookInput.y);

        if (lookInput != Vector2.zero)
        {
            // Rotate towards look direction
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(lookDirection, Vector3.up), rotationSpeed * Time.deltaTime));
        }
        else if (moveInput != Vector2.zero)
        {
            // Rotate towards direction of movement
            rb.MoveRotation(Quaternion.Slerp(rb.rotation, Quaternion.LookRotation(moveDirection, Vector3.up), rotationSpeed * Time.deltaTime));
        }

        if (moveInput != Vector2.zero)
        {
            anim.SetFloat("WalkSpeed", moveInput.magnitude);
        }
        else
        {
            anim.SetFloat("WalkSpeed", 0f);
        }

        // Apply movement speed
        moveDirection *= speed * Time.deltaTime;

        // Move position
        rb.MovePosition(rb.position + moveDirection);
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        lookInput = value.Get<Vector2>();
    }
}
