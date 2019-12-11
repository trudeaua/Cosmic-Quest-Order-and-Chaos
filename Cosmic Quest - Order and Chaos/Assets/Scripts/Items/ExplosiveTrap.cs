using System.Collections;
using UnityEngine;

public class ExplosiveTrap : MonoBehaviour
{
    public float maxDamage = 5f;
    public float explosionForce = 400f;
    public float explosionRadius = 5f;
    public float stunTime = 2f;
    public float armTime = 1.5f;

    private bool _isArmed;
    private bool _isDetonated;
    private EntityStatsController _thrower;
    // audio to play on explosion
    private EntityAudioClip audioClip;

    private Collider[] _hits;
    public GameObject explosiveTrapVFX;

    private void Awake()
    {
        _hits = new Collider[32];
    }

    public void PlaceTrap(EntityStatsController thrower, Vector3 position)
    {
        _thrower = thrower;
        _isDetonated = false;
        transform.position = position;
        _isArmed = false;
        
        // Set self active
        gameObject.SetActive(true);
        StartCoroutine("ArmTrap");
    }

    public void SetAudio(EntityAudioClip audioClip)
    {
        this.audioClip = audioClip;
    }

    private IEnumerator ArmTrap()
    {
        yield return new WaitForSeconds(armTime);
        _isArmed = true;
    }
    
    private void Detonate()
    {
        _isDetonated = true;
        ExplosionEffect();
        // Play explosion effect
        gameObject.SetActive(false);
    }

    private void ExplosionEffect()
    {
        PerformExplosionAnimation();
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
        if (_isArmed && other.CompareTag("Enemy") && !_isDetonated)
        {
            Detonate();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_isArmed && other.CompareTag("Enemy") && !_isDetonated)
        {
            Detonate();
        }
    }

    protected void PerformExplosionAnimation()
    {
        GameObject vfx;
        StartCoroutine(_thrower.PlayAudio(audioClip));
        vfx = Instantiate(explosiveTrapVFX, gameObject.transform.position, Quaternion.identity);

        var ps = GetFirstPS(vfx);

        Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax + 1);
    }

    private ParticleSystem GetFirstPS(GameObject vfx)
    {
        var ps = vfx.GetComponent<ParticleSystem>();
        if (ps == null && vfx.transform.childCount > 0)
        {
            foreach (Transform t in vfx.transform)
            {
                ps = t.GetComponent<ParticleSystem>();
                if (ps != null)
                    return ps;
            }
        }
        return ps;
    }
}
