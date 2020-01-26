using UnityEngine;

public class PlayerUIControl : MonoBehaviour
{
    void Start()
    {
        int assignedPlayer = PlayerManager._instance.AssignUIControlToPlayer(gameObject);
        if (assignedPlayer >= 0)
        {
            name = "Player " + (assignedPlayer + 1) + " UI Control";
            MainMenuController._instance.AssignMultiplayerUIControl(gameObject, assignedPlayer);
        }
        else
        {
            Debug.LogError("UI Control not assigned, no available player");
        }
    }
}
