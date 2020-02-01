using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMotorController : MonoBehaviour
{
    public float maxVelocity = 6.0f;
    public float maxAcceleration = 25.0f;
    public float rotationSpeed = 10.0f;
    private float _speedModifier = 1f;
    
    private Rigidbody _rb;
    private Animator _anim;

    private Vector2 _moveInput;
    private Vector2 _lookInput;

    // TODO temporary until playermanager is finalized
    public bool doRegister = true;

    private CameraController _cameraController;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _anim = GetComponentInChildren<Animator>();
        _cameraController = Camera.main.GetComponent<CameraController>();
        // TODO Temporary - player should be registered after lobby
        if (doRegister)
        {
            PlayerManager.RegisterPlayer(gameObject);
        }
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

    private void OnDestroy()
    {
        // Ensure there's no invalid references hanging around
        if (doRegister)
        {
            PlayerManager.DeregisterPlayer(gameObject);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }
    /// <summary>
    /// Move the player
    /// </summary>
    private void Move()
    {
        Vector3 inputMoveDirection = new Vector3(_moveInput.x, 0f, _moveInput.y) * _speedModifier;
        Vector3 inputLookDirection = new Vector3(_lookInput.x, 0f, _lookInput.y);

        if (_lookInput != Vector2.zero)
        {
            // Rotate towards look direction
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, Quaternion.LookRotation(inputLookDirection, Vector3.up), rotationSpeed * _speedModifier * Time.deltaTime));
        }
        else if (_moveInput != Vector2.zero)
        {
            // Rotate towards direction of movement
            _rb.MoveRotation(Quaternion.Slerp(_rb.rotation, Quaternion.LookRotation(inputMoveDirection, Vector3.up), rotationSpeed * _speedModifier * Time.deltaTime));
        }
        
        // for overriding legs when player is moving
        int walkLayerIndex = _anim.GetLayerIndex("WalkLayer");
        // for overriding legs when ranger is shooting
        int attackLayerIndex = _anim.GetLayerIndex("AttackLayer");
        if (attackLayerIndex >= 0)
        {
            // weight must be greater than walk layer weight
            _anim.SetLayerWeight(attackLayerIndex, 1);
        }
        
        // Animate player legs, legs will still move as they attack
        if (_moveInput != Vector2.zero)
        {
            // override default base layer walk animations
            _anim.SetLayerWeight(walkLayerIndex, .9f);
        }
        else
        {
            // Don't animate player legs
            _anim.SetLayerWeight(walkLayerIndex, 0);
        }

        float signedInputLookAngle = Vector3.SignedAngle(inputMoveDirection, inputLookDirection, Vector3.up);
        float inputLookAngle = Mathf.Abs(signedInputLookAngle);

        // Trigger walking animation
        _anim.SetFloat("WalkSpeed", inputMoveDirection == Vector3.zero ? 0f : inputMoveDirection.magnitude);
        // Set animation playback speed (if moving backwards animation will play in reverse)
        _anim.SetFloat("Direction", inputLookAngle < 90 ? 1f * inputMoveDirection.magnitude : -1f * inputMoveDirection.magnitude);
        // Set whether the strafe animation should be mirrored or not
        _anim.SetBool("MirrorStrafe", 
            (signedInputLookAngle >= 95f && signedInputLookAngle <= 145f) || 
            (signedInputLookAngle >= -95f && signedInputLookAngle <= -35f));
        // Set whether the player should strafe or not
        _anim.SetBool("Strafe", (inputLookAngle >= 35f && inputLookAngle <= 145f));

        inputMoveDirection *= maxVelocity;
        AccelerateTo(inputMoveDirection);

        // TODO add proper gravity to players. Should be able to detect if currently on the "ground", which is when it apply gravity

        // Don't clamp if player is stationary
        if (inputMoveDirection != Vector3.zero)
        {
            Vector3 clamped = _cameraController.ClampToScreenEdge(_rb.position);
            if (clamped != _rb.position)
            {
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

    public void ApplyMovementModifier(float modifier)
    {
        _speedModifier = modifier;
    }

    public void ResetMovementModifier()
    {
        _speedModifier = 1f;
    }

    public IEnumerator ApplyTimedMovementModifier(float modifier, float time)
    {
        _speedModifier = modifier;
        yield return new WaitForSeconds(time);
        _speedModifier = 1f;
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
