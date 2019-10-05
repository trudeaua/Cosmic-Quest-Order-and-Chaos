using UnityEngine;

/*
 * A base class for interactable objects to inherit from.
 */
public class Interactable : MonoBehaviour
{
    public float radius = 3f;

    public bool CanInteract(Vector3 position)
    {
        return Vector3.Distance(position, transform.position) <= radius;
    }

    private void OnDrawGizmosSelected()
    {
        // Draw interaction radius when selected in the editor
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
