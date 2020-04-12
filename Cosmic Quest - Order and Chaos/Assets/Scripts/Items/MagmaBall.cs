using UnityEngine;

public class MagmaBall : MonoBehaviour
{
    public float minDamage;
    public float maxDamage;
    public float impactRadius;
    public GameObject impactVFX;

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
    
    private void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;
        
        // if other object is a player then deal damage
        if (col.CompareTag("Player"))
        {
            EntityStatsController target = col.GetComponent<EntityStatsController>();
            target.TakeDamage(Random.Range(minDamage, maxDamage));
            gameObject.SetActive(false);
        }
        else if (col.CompareTag("Ground"))
        {
            // Damage any players within impact radius
            float damage = Random.Range(minDamage, maxDamage);
            foreach (GameObject player in PlayerManager.Instance.AlivePlayers)
            {
                if (Vector3.Distance(transform.position, col.transform.position) <= impactRadius)
                {
                    player.GetComponent<EntityStatsController>().TakeDamage(damage);
                }
            }
            
            gameObject.SetActive(false);
        }
    }
}
