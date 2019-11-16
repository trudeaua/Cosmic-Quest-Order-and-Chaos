using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsController : EntityStatsController
{
    // Player specific stats
    public RegenerableStat stamina;
    public RegenerableStat mana;

    protected override void Update()
    {
        base.Update();
        
        stamina.Regen();
        mana.Regen();
    }
    
    protected override void Die()
    {
        Debug.Log("Player died");
        isDead = true;
        transform.gameObject.SetActive(false);
    }
}
