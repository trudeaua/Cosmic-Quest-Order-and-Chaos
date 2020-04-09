using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteractionController : MonoBehaviour
{
    [Tooltip("Max distance of objects that the player can interact with")]
    public float interactionRadius = 4f;
    [Tooltip("The sweeping angle in which the player can detect interactables")]
    public float interactionAngle = 60f;
    
    private PlayerCombatController _combat;
    private PlayerStatsController _stats;
    private Animator _anim;
    private Collider _col;
    private Interactable _currentObject;
    private Collider[] _hits = new Collider[32];

    public bool IsInteracting => _currentObject;
    
    private void Start()
    {
        _combat = GetComponent<PlayerCombatController>();
        _anim = GetComponentInChildren<Animator>();
        _stats = GetComponent<PlayerStatsController>();
        _stats.onDeath.AddListener(StopInteract);
        _col = GetComponent<Collider>();
    }

    public void StopInteract()
    {
        if (!_currentObject)
            return;
        
        // Decide which animation to do
        if (_currentObject is Draggable)
        {
            _anim.SetBool("PickedUp", false);
        }

        _currentObject.StopInteract(transform);
        _currentObject = null;
    }
    
    /// <summary>
    /// Toggle the interaction sequence
    /// </summary>
    /// <param name="value">Value of the input controller interact button state</param>
    private void OnInteract(InputValue value)
    {
        // Only trigger on button pressed down
        if (value.isPressed)
        {
            // If currently interacting with a "non-held" object, stop interacting
            if (_currentObject)
            {
                StopInteract();
            }
            
            // Ensure the player isn't currently attacking before attempting interaction
            if (_combat.AttackCooldown > 0)
                return;
            
            // Attempt to interact with the first interactable in the player's view
            Interactable interactable = GetInteractableInView();
            if (interactable is null)
                return;

            if (!interactable.CanInteract(transform))
            {
                return;
            }

            // Decide which animation to do
            if (interactable is Draggable)
            {
                _anim.SetBool("PickedUp", true);
            }
            else if (interactable is Lever)
            {
                _anim.SetTrigger("InteractStanding");
            }
            else if (interactable is Collectable)
            {
                _anim.SetTrigger("InteractGround");
            }

            // Attempt interaction
            interactable.StartInteract(transform);
            
            if (!interactable.isTrigger)
                _currentObject = interactable;
        }
        else if (_currentObject && _currentObject.isHeld)
        {
            // Stop interacting with a "held" object
            StopInteract();
        }
    }

    /// <summary>
    /// Gets the nearest interactable object within the player's view
    /// </summary>
    /// <returns>Reference to the interactable object</returns>
    private Interactable GetInteractableInView()
    {
        float minDistance = Mathf.Infinity;
        Interactable interactable = null;
        // Disable the player's collider briefly for the interactable proximity checks
        _col.enabled = false;
        
        int numHits = Physics.OverlapSphereNonAlloc(transform.position, interactionRadius, _hits);

        for (int i = 0; i < numHits; i++)
        {
            Interactable interactableObject = _hits[i].GetComponent<Interactable>();

            // Ensure the object is an interactable and is reachable by the player
            if (interactableObject is null ||
                !(Physics.Linecast(transform.position, _hits[i].transform.position, out RaycastHit hitInfo) &&
                  hitInfo.transform.Equals(_hits[i].transform)))
                continue;
            
            float dist = Vector3.Distance(transform.position, _hits[i].transform.position);
            float angle = Mathf.Abs(Vector3.SignedAngle(transform.forward,
                _hits[i].transform.position - transform.position, Vector3.up));
            
            // If object is within view and is the closest then select this for interaction
            if (angle < interactionAngle / 2f && dist < minDistance)
            {
                minDistance = dist;
                interactable = interactableObject;
            }
        }

        // Reenable collider when done
        _col.enabled = true;

        return interactable;
    }
}
