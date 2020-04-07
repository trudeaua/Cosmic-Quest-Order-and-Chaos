using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreantSeed : DamageProjectile
{
    [Range(0, 1)] public float spawnPlantProbability = 0.3f;
    public List<GameObject> plantPrefabs;
    
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }

    private void Update()
    {
        // Despawn if below the map
        if (transform.position.y < -10)
        {
            gameObject.SetActive(false);
        }
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        
        // if collision is with a player then deal damage
        if (col.CompareTag("Player"))
        {
            EntityStatsController target = col.GetComponent<EntityStatsController>();
            target.TakeDamage(Damage);
            gameObject.SetActive(false);
        }
        else if (col.CompareTag("Ground"))
        {
            // Possibly spawn a random plant
            if (Random.Range(0f, 1f) > spawnPlantProbability)
            {
                GameObject plantPrefab = plantPrefabs[Random.Range(0, plantPrefabs.Count - 1)];
                Instantiate(plantPrefab, new Vector3(transform.position.x, 0, transform.position.z), Quaternion.identity);
            }
            
            gameObject.SetActive(false);
        }
    }
}
