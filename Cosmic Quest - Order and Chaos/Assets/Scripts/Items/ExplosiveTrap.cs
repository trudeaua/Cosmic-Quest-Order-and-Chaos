using System.Collections;
using UnityEngine;

public class ExplosiveTrap : MonoBehaviour
{
    public float maxDamage = 5f;
    public float explosionForce = 400f;
    public float explosionRadius = 5f;
    public float stunTime = 2f;

    private bool _isDetonated;
    private EntityStatsController _thrower;

    private Collider[] _hits;

    private void Awake()
    {
        _hits = new Collider[32];
    }

    public void PlaceTrap(EntityStatsController thrower, Vector3 position)
    {
        _thrower = thrower;
        _isDetonated = false;
        transform.position = position;
        
        // Set self active
        gameObject.SetActive(true);
    }
    
    private IEnumerator Detonate()
    {
        ExplosionEffect();
        
        // Play explosion effect
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
    }

    private void ExplosionEffect()
    {
        int numHits = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, _hits, EntityStatsController.EntityLayer);

        for (int i = 0; i < numHits; i++)
        {
            if (!_hits[i].transform.CompareTag("Enemy"))
                continue;
            
            // TODO check for if enemy is behind cover
            _hits[i].transform.GetComponent<EnemyStatsController>().TakeExplosionDamage(_thrower, maxDamage, stunTime, explosionForce, transform.position, explosionRadius);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") && !_isDetonated)
        {
            _isDetonated = true;
            StartCoroutine(Detonate());
        }
    }
}
