using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerJoinHelper : MonoBehaviour
{
    public void PlayerJoined(PlayerInput playerInput)
    {
        PlayerManager.Instance.OnPlayerJoined(playerInput);
    }

    public void PlayerLeft(PlayerInput playerInput)
    {
        PlayerManager.Instance.OnPlayerLeft(playerInput);
    }
}
