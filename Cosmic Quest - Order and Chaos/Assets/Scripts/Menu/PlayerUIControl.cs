using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIControl : MonoBehaviour
{
    private int assignedPlayer;
    void Start()
    {
        assignedPlayer = PlayerManager.Instance.AssignUIControlToPlayer(gameObject);
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

    public void OnMenuCancel(InputValue value)
    {
        if (!value.isPressed)
        {
            return;
        }
        if (assignedPlayer == 0)
        {
            MainMenuController.Instance.PopMenu();
        }
    }
}
