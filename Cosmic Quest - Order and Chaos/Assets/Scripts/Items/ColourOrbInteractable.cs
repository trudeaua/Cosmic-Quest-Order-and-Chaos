using UnityEngine;

public class ColourOrbInteractable : Draggable
{
    private Rigidbody _rb;
    private ColourOrbProjectile _projectile;

    protected override void Awake()
    {
        base.Awake();

        _rb = GetComponent<Rigidbody>();
        _projectile = GetComponent<ColourOrbProjectile>();
        
        // Make sure we don't try setting the material colour
        isParticleSystem = true;
    }

    public override void Dropped(Transform target)
    {
        base.Dropped(target);
        
        // launch projectile
        _projectile.Launch(target.GetComponent<EntityStatsController>(), target.forward, 300f, 30f);
    }

    public override void PickedUp(Transform target)
    {
        base.PickedUp(target);
        
        // Reset forces on Rigidbody
        _rb.velocity = Vector3.zero;
        _rb.angularVelocity = Vector3.zero;
    }
}
