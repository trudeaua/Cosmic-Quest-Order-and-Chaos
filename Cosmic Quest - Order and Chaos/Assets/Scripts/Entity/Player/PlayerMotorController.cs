using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotorController : MonoBehaviour
{
    public float speed = 6.0f;
    public float rotationSpeed = 10.0f;

    private Rigidbody _rb;
    private Animator _anim;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private Vector3 _moveDirection;
    private Vector3 _lookDirection;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        // Ensure player is not kinematic
        _rb.isKinematic = false;
    }

    private void OnDisable()
    {
        // Set kinematic when disabled so the player stops moving
        _rb.isKinematic = true;
    }

    private void Start()
    {
        // TODO Temporary - player should be registered after lobby
        PlayerManager.Instance.RegisterPlayer(gameObject);
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        Vector3 inputMoveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y);
        Vector3 inputLookDirection = new Vector3(_lookInput.x, 0f, _lookInput.y);

        if (_lookInput != Vector2.zero)
        {
            // Rotate towards look direction
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, Quaternion.LookRotation(inputLookDirection, Vector3.up), rotationSpeed * Time.deltaTime));
        }
        else if (_moveInput != Vector2.zero)
        {
            // Rotate towards direction of movement
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, Quaternion.LookRotation(inputMoveDirection, Vector3.up), rotationSpeed * Time.deltaTime));
        }

        // Trigger walking animation
        _anim.SetFloat("WalkSpeed", _moveInput == Vector2.zero ? 0f : _moveInput.magnitude);

        // Apply movement speed
        inputMoveDirection *= speed * Time.deltaTime;

        // Move position
        _rb.MovePosition(_rb.position + inputMoveDirection);
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }
}
