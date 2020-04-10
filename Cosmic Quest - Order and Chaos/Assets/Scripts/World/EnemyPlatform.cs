using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Dervied Platform class for enemy detection
public class EnemyPlatform : Platform
{
    protected override void OnTriggerEnter (Collider other) 
    {
        if (!_isActivated && other.CompareTag("Enemy") && other.GetComponent<EnemyStatsController>().characterColour == colour)
        {
            _anim.SetBool("PlatformActivated", true);

            _audio.PlayDelayed(0);
            _isActivated = true;
            
            onActivation?.Invoke(true);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        return;
    }
}
