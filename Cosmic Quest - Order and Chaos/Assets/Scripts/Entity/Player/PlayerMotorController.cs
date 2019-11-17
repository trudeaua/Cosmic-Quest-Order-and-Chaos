using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotorController : MonoBehaviour
{
    public float maxVelocity = 6.0f;
    public float maxAcceleration = 20.0f;
    public float rotationSpeed = 10.0f;

    [Tooltip("Max distance of objects that the player can interact with")]
    public float interactionRadius = 4f;

    private Rigidbody _rb;
    private Animator _anim;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    private Vector3 _moveDirection;
    private Vector3 _lookDirection;
    
    private CameraController _cameraController;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        Debug.Log(_anim.name);
        _cameraController = Camera.main.GetComponent<CameraController>();
        
        // TODO Temporary - player should be registered after lobby
        PlayerManager.RegisterPlayer(gameObject);
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
        // for overriding legs when player is moving
        var walkLayerIndex = _anim.GetLayerIndex("WalkLayer");
        // for overriding torso when mage/healer is running
        var idleLayerIndex = _anim.GetLayerIndex("IdleLayer");
        // Animate player legs, legs will still move as they attack
        if (_moveInput != Vector2.zero)
        {
            _anim.SetLayerWeight(walkLayerIndex, .9f);
            if (_anim.GetLayerIndex("IdleLayer") >= 0)
            {
                _anim.SetLayerWeight(idleLayerIndex, 1);
            }
        }
        // Don't animate player legs
        else
        {
            _anim.SetLayerWeight(walkLayerIndex, 0);
            if (_anim.GetLayerIndex("IdleLayer") >= 0)
            {
                _anim.SetLayerWeight(idleLayerIndex, 0);
            }
        }

        // Trigger walking animation
        _anim.SetFloat("WalkSpeed", _moveInput == Vector2.zero ? 0f : _moveInput.magnitude);
        _anim.SetFloat("Direction", Vector3.Angle(inputMoveDirection, inputLookDirection) < 90 ? 1f * _moveInput.magnitude : -1f *_moveInput.magnitude );

        inputMoveDirection *= maxVelocity;
        AccelerateTo(inputMoveDirection);

        // Don't clamp if player is stationary
        if (inputMoveDirection != Vector3.zero)
        {
            Vector3 clamped = _cameraController.ClampToScreenEdge(_rb.position);
            if (clamped != _rb.position)
            {
                // TODO weird behaviour with gravity when position is clamped
                clamped.y = _rb.position.y;
                _rb.MovePosition(clamped);
            }
        }
    }

    private void AccelerateTo(Vector3 targetVelocity)
    {
        Vector3 dV = targetVelocity - _rb.velocity;
        Vector3 accel = dV / Time.fixedDeltaTime;

        if (accel.sqrMagnitude > maxAcceleration * maxAcceleration)
            accel = accel.normalized * maxAcceleration;
        
        _rb.AddForce(accel, ForceMode.Acceleration);
    }

    private void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }

    private void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }

    private void OnInteract(InputValue value)
    {
        // Only trigger on button pressed down
        if (value.isPressed)
        {
            // Attempt to interact with the first interactable in the player's view
            if (Physics.Raycast(transform.position + Vector3.up, transform.TransformDirection(Vector3.forward), out RaycastHit hit, interactionRadius))
            {
                Interactable interactable = hit.transform.GetComponent<Interactable>();
                if (interactable != null)
                {
                    // Attempt interaction
                    interactable.Interact(transform);
                }
            }
        }
    } 
}
