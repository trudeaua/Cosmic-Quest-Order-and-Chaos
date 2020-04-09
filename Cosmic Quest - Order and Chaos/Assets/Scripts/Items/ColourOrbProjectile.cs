using System.Collections.Generic;
using UnityEngine;

public class ColourOrbProjectile : DamageProjectile
{
    public ParticleSystem[] colourParticleSystems;
    
    private CharacterColour _colour;
    private ColourOrbInteractable _interactable;
    private bool LaunchedByPlayer => (LauncherStats as RhakStatsController) is null;

    protected override void Awake()
    {
        base.Awake();

        _interactable = GetComponent<ColourOrbInteractable>();
    }

    /// <summary>
    /// Sets the colour of the orb to the set colour when enabled
    /// </summary>
    private void OnEnable()
    {
        if (LaunchedByPlayer)
            return;
        
        // Select an orb colour randomly
        List<CharacterColour> orbColours = (LauncherStats as RhakStatsController).OrbsRequired;
        _colour = orbColours[Random.Range(0, orbColours.Count - 1)];
        Color orbColour = PlayerManager.colours.GetColour(_colour);
        
        // Set the colour of the particle systems to the orb colour
        foreach (ParticleSystem p in colourParticleSystems)
        {
            VfxHelper.SetParticleSystemColour(p, orbColour);
        }
        
        // Set interactable colour
        _interactable.colour = _colour;
    }

    protected override void OnTriggerEnter(Collider other)
    {
        GameObject col = other.gameObject;

        // Ignore collisions with launcher or if currently being held
        if (col == LauncherStats.gameObject || _interactable.IsPickedUp)
            return;
        
        // if other object is a player, deal damage
        if (col.CompareTag("Player"))
        {
            EntityStatsController target = col.GetComponent<EntityStatsController>();
            target.TakeDamage(LauncherStats, Damage);
        }
        else if (col.CompareTag("Enemy") && LaunchedByPlayer)
        {
            RhakStatsController target = col.GetComponent<RhakStatsController>();
            
            // Ensure that the boss was hit
            if (target)
            {
                target.TakeDamageFromOrb(_colour);
            }
        }
        
        // Don't worry about collisions with colliders that are triggers
        if (!other.isTrigger)
            gameObject.SetActive(false);
    }
}
