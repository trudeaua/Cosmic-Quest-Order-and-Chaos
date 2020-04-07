using UnityEngine;

public class TreantRoot : MonoBehaviour
{
    [Header("Combat Settings")]
    public float minDamage;
    public float maxDamage;
    
    private Animator _anim;
    private RaycastHit[] _hits = new RaycastHit[32];

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        
        // Set idle animation randomly to one of three
        _anim.SetInteger("IdleAnim", Random.Range(0, 2));
        
        // Rotate around the Y-axis randomly to give variation
        transform.Rotate(Vector3.up, Random.Range(0, 360), Space.World);
    }

    /// <summary>
    /// Event function to start the attack sequence for this root
    /// </summary>
    public void StartAttack()
    {
        if (Random.Range(0f, 1f) > 0.4f)
            _anim.SetTrigger("Spawn");
    }

    /// <summary>
    /// Event function for root attack
    /// </summary>
    public void PerformDamage()
    {
        int numHits = Physics.RaycastNonAlloc(transform.position, Vector3.up, _hits, 20f);

        // Apply damage to any unlucky players above the root
        for (int i = 0; i < numHits; i++)
        {
            if (!_hits[i].transform.CompareTag("Player"))
                continue;
            
            float damage = Random.Range(minDamage, maxDamage);
            _hits[i].transform.GetComponent<PlayerStatsController>().TakeDamage(damage);
        }
    }
}
