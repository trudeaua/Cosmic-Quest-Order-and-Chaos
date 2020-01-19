using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        int assignedPlayer = PlayerManager.AssignUIControlToPlayer(gameObject);
        if (assignedPlayer > 0)
        {
            Debug.Log("UI Control Assigned to Player " + assignedPlayer);
            name = "Player " + assignedPlayer + " UI Control";
            MenuController.AddMultiplayerUIControl(gameObject);
        }
        else
        {
            Debug.LogError("UI Control not assigned, no available player");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
