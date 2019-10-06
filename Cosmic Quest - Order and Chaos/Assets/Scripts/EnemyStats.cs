using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats : EntityStats
{
    public override void Die()
    {
        Debug.Log(transform.name + " died.");
        transform.gameObject.SetActive(false);
    }
}
