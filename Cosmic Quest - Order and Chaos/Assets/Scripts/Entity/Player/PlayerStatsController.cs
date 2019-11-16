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
        Debug.Log(transform.name + " died.");
        isDead = true;
        StartCoroutine(PlayerDeath());
    }
    
    private IEnumerator PlayerDeath()
    {
        Anim.SetTrigger("Die");
        yield return new WaitForSeconds(2.5f);
        transform.gameObject.SetActive(false);
    }
}
