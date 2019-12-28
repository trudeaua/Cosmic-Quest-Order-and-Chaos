using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossColourController : MonoBehaviour
{
    public int NumPlayers;
    public CharacterColour colour;

    void Start()
    {
        colour = GetComponent<EnemyStatsController>().characterColour;    
        //NumPlayers = gameObject.Find("GameManager").GetComponent<PlayerManager>().Players.Count;
    }

    void Update()
    {
        
    }

    private IEnumerator SetColour()
    {
        yield return new WaitForSeconds(5f);
    }
}
