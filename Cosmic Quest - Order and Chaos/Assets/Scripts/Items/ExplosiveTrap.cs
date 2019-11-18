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
        Collider[] hits = new Collider[32]; // TODO is 32 hits an okay amount to initialize to?
        int numHits = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, hits, EntityStatsController.EntityLayer);

        for (int i = 0; i < numHits; i++)
        {
            // TODO Not triggering well... Bad hit detection, bad damage calculation
            // If enemy can be hit from the point of explosion, then apply explosive damage
            if (hits[i].transform.CompareTag("Enemy") &&
                Physics.Linecast(transform.position, hits[i].transform.position, out RaycastHit hit) &&
                hit.transform.CompareTag("Enemy"))//hit.transform == hits[i].transform)
            {
                hits[i].transform.GetComponent<EnemyStatsController>().TakeExplosionDamage(_thrower, maxDamage, stunTime, explosionForce, transform.position, explosionRadius);
            }
        }
        
        // Play explosion effect
        yield return new WaitForSeconds(1f);

        gameObject.SetActive(false);
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
