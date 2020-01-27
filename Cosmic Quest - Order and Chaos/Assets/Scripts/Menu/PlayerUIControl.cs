using UnityEngine;

public class PlayerUIControl : MonoBehaviour
{
    void Start()
    {
        int assignedPlayer = PlayerManager.Instance.AssignUIControlToPlayer(gameObject);
        if (assignedPlayer >= 0)
        {
            name = "Player " + (assignedPlayer + 1) + " UI Control";
            MainMenuController.Instance.AssignMultiplayerUIControl(gameObject, assignedPlayer);
        }
        else
        {
            Debug.LogError("UI Control not assigned, no available player");
        }
    }
}
