using System.Collections;
using UnityEngine;

public class TreantRoot : MonoBehaviour
{
    [Header("Combat Settings")]
    public float minDamage;
    public float maxDamage;
    
    private Animator _anim;
    private Collider _col;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _col = GetComponent<Collider>();
        _col.enabled = false;
        
        // Set idle animation randomly to one of three
        _anim.SetInteger("IdleAnim", Random.Range(0, 2));
        
        // Rotate around the Y-axis randomly to give variation
        transform.Rotate(Vector3.up, Random.Range(0, 360), Space.Self);
    }

    /// <summary>
    /// Event function to start the attack sequence for this root with a random delay
    /// </summary>
    public IEnumerator StartAttack()
    {
        yield return new WaitForSeconds(Random.Range(0f, 0.5f));
        _col.enabled = true;
        _anim.SetTrigger("Spawn");
    }

    /// <summary>
    /// Event function that handles end of attack tasks for the treant root
    /// </summary>
    public void StopAttack()
    {
        _col.enabled = false;
    }

    /// <summary>
    /// Event function for dealing damage to players on collision enter
    /// </summary>
    /// <param name="other">The object that collided with the root</param>
    private void OnTriggerEnter(Collider other)
    {
        // Ignore collisions unless with player
        if (!other.transform.CompareTag("Player"))
            return;
        
        // Deal damage to the player
        float damage = Random.Range(minDamage, maxDamage);
        other.transform.GetComponent<PlayerStatsController>().TakeDamage(damage);
    }
}
