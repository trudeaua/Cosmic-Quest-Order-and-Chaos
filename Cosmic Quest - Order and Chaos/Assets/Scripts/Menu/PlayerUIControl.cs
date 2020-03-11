using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIControl : MonoBehaviour
{
    private int assignedPlayer;
    private void Start()
    {
        assignedPlayer = PlayerManager.Instance.AssignUIControlToPlayer(gameObject);
        if (assignedPlayer >= 0)
        {
            name = "Player " + (assignedPlayer + 1) + " UI Control";
            if (GameManager.Instance.CurrentState == GameManager.GameState.Menu)
            {
                MainMenuController.Instance.AssignMultiplayerUIControl(gameObject, assignedPlayer);
            }
            else if (assignedPlayer == 0 && GameManager.Instance.CurrentState == GameManager.GameState.SelectingLevel)
            {
                LevelOverlayController.Instance.AssignMultiplayerUIControl(gameObject);
            }
        }
        else
        {
            Debug.LogError("UI Control not assigned, no available player");
        }
    }

    public void OnMenuCancel(InputValue value)
    {
        // only player 1 may activate this so we check if their player num is 0
        if (assignedPlayer == 0 && GameManager.Instance.CurrentState == GameManager.GameState.Menu)
        {
            if (!value.isPressed)
            {
                return;
            }
            MainMenuController.Instance.PopMenu();
        }
        if (assignedPlayer == 0 && GameManager.Instance.CurrentState == GameManager.GameState.SelectingLevel)
        {
            if (!value.isPressed)
            {
                return;
            }
            OverworldController.Instance.DeselectLevel();
        }
    }

    public void OnMenuSelect(InputValue value)
    {
        // only player 1 may activate this so we check if their player num is 0
        if (assignedPlayer == 0 && GameManager.Instance.CurrentState == GameManager.GameState.SelectingLevel)
        {
            if (!value.isPressed)
            {
                return;
            }
            OverworldController.Instance.SelectLevel();
        }
    }

    public void OnMenuNavigate(InputValue value)
    {
        // only player 1 may activate this so we check if their player num is 0
        if (assignedPlayer == 0 && GameManager.Instance.CurrentState == GameManager.GameState.SelectingLevel)
        {
            Vector2 input = value.Get<Vector2>();
            if (Mathf.Approximately(input.x, 0) && Mathf.Approximately(input.y, 0))
            {
                return;
            }
            float angle = Vector2.SignedAngle(Vector2.up, input);
            if (angle >= -22.5 && angle < 22.5)
            {
                OverworldController.Instance.NavigateUp();
            }
            else if (angle >= 22.5 && angle < 67.5)
            {
                OverworldController.Instance.NavigateUpLeft();
            }
            else if (angle >= 67.5 && angle < 112.5)
            {
                OverworldController.Instance.NavigateLeft();
            }
            else if (angle >= 112.5 && angle < 157.5)
            {
                OverworldController.Instance.NavigateDownLeft();
            }
            else if (angle >= 157.5 || angle < -157.5)
            {
                OverworldController.Instance.NavigateDown();
            }
            else if (angle >= -157.5 && angle < -112.5)
            {
                OverworldController.Instance.NavigateDownRight();
            }
            else if (angle >= -112.5 && angle < -67.5)
            {
                OverworldController.Instance.NavigateRight();
            }
            else if (angle >= -67.5 && angle < -22.5)
            {
                OverworldController.Instance.NavigateUpRight();
            }
        }
    }
}
