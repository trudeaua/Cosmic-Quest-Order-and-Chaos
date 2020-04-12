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

    private Collider[] _hits;
    public GameObject explosiveTrapVFX;

    // source to play explosion sfx from
    private AudioSource source;
    // explosion sfx to play
    [SerializeField] private AudioHelper.EntityAudioClip audioClip;

    public GameObject trapModel;

    private void Awake()
    {
        _hits = new Collider[32];
        source = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Place the explosive trap
    /// </summary>
    /// <param name="thrower">Stats of the entity that placed the trap</param>
    /// <param name="position">Position to place the trap</param>
    /// <returns></returns>
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

    /// <summary>
    /// Set the explosion audio source and clip
    /// </summary>
    /// <param name="source">Source to play audio from</param>
    /// <param name="audioClip">Audio clip to play</param>
    public void SetExplosionAudio(AudioSource source, AudioHelper.EntityAudioClip audioClip)
    {
        this.source = source;
        this.audioClip = audioClip;
    }

    /// <summary>
    /// Arm the trap
    /// </summary>
    /// <returns>An IEnumerator</returns>
    private IEnumerator ArmTrap()
    {
        yield return new WaitForSeconds(armTime);
        _isArmed = true;
    }

    /// <summary>
    /// Detonate the trap
    /// </summary>
    private void Detonate()
    {
        _isDetonated = true;
        // Play explosion effect
        ExplosionEffect();
        StartCoroutine(RemoveTrapFromScene());
    }

    /// <summary>
    /// Remove the trap from the scene
    /// </summary>
    /// <returns>An IEnumerator</returns>
    private IEnumerator RemoveTrapFromScene()
    {
        trapModel.SetActive(false);
        yield return new WaitForSeconds(2f);
        trapModel.SetActive(true);
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Perform the explosion
    /// </summary>
    private void ExplosionEffect()
    {
        PerformExplosionAnimation();
        StartCoroutine(AudioHelper.PlayAudioOverlap(source, audioClip));
        // play the explosion sound
        int numHits = Physics.OverlapSphereNonAlloc(transform.position, explosionRadius, _hits, EntityStatsController.EntityLayer);

        for (int i = 0; i < numHits; i++)
        {
            if (!_hits[i].transform.CompareTag("Enemy"))
                continue;

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

    /// <summary>
    /// Perform the explosion animation
    /// </summary>
    protected void PerformExplosionAnimation()
    {
        GameObject vfx;

        vfx = Instantiate(explosiveTrapVFX, gameObject.transform.position, Quaternion.identity);

        var ps = GetFirstPS(vfx);

        Destroy(vfx, ps.main.duration + ps.main.startLifetime.constantMax + 1);
    }

    /// <summary>
    /// Get the VFX particle system
    /// </summary>
    /// <param name="vfx">A gameobject</param>
    /// <returns>The first particle system found in the children of `vfx`</returns>
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