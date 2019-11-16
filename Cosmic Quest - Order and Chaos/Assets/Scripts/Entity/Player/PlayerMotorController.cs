using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotorController : MonoBehaviour
{
    public float speed = 6.0f;
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
        _cameraController = Camera.main.GetComponent<CameraController>();
        
        // TODO Temporary - player should be registered after lobby
        PlayerManager.Instance.RegisterPlayer(gameObject);
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
        // Animate player legs, legs will still move as they attack
        if (_moveInput != Vector2.zero)
        {
            _anim.SetLayerWeight(1, 1);
        }
        // Don't animate player legs
        else
        {
            _anim.SetLayerWeight(1, 0);

        }

        // Trigger walking animation
        _anim.SetFloat("WalkSpeed", _moveInput == Vector2.zero ? 0f : _moveInput.magnitude);
        _anim.SetFloat("Direction", Vector3.Angle(inputMoveDirection, inputLookDirection) < 90 ? 1f * (_moveInput.magnitude + speed*0.03f) : -1f *(_moveInput.magnitude + speed*0.03f) );

        // Apply movement speed
        inputMoveDirection *= speed * Time.deltaTime;

        _rb.MovePosition(_cameraController.ClampToScreenEdge(_rb.position + inputMoveDirection));
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
