using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUIControl : MonoBehaviour
{
    void Start()
    {
        int assignedPlayer = PlayerManager.AssignUIControlToPlayer(gameObject);
        if (assignedPlayer >= 0)
        {
            name = "Player " + (assignedPlayer + 1) + " UI Control";
            MenuController.AssignMultiplayerUIControl(gameObject, assignedPlayer);
        }
        else
        {
            Debug.LogError("UI Control not assigned, no available player");
        }
    }
}
