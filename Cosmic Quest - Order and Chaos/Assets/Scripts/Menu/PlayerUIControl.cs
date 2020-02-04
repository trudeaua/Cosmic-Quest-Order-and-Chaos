using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerUIControl : MonoBehaviour
{
    private int assignedPlayer;
    private float cooldown = 0.2f;
    private void Start()
    {
        assignedPlayer = PlayerManager.Instance.AssignUIControlToPlayer(gameObject);
        if (assignedPlayer >= 0)
        {
            name = "Player " + (assignedPlayer + 1) + " UI Control";
            if (GameSceneManager.Instance.currentSceneName == "MenuStaging")
            {
                MainMenuController.Instance.AssignMultiplayerUIControl(gameObject, assignedPlayer);
            }
        }
        else
        {
            Debug.LogError("UI Control not assigned, no available player");
        }
    }

    private void Update()
    {
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0)
            {
                cooldown = 0;
            }
        }
    }

    public void OnMenuCancel(InputValue value)
    {
        // only player 1 may activate this
        if (assignedPlayer == 0 && GameSceneManager.Instance.currentSceneName == "MenuStaging")
        {
            if (!value.isPressed)
            {
                return;
            }
            MainMenuController.Instance.PopMenu();
        }
    }

    public void OnMenuSelect()
    {
        if (assignedPlayer == 0 && GameSceneManager.Instance.currentSceneName == "LevelsScene")
        {
            LevelsController.Instance.SelectLevel();
        }
    }

    public void OnMenuNavigate(InputValue value)
    {
        // only player 1 may activate this
        if (assignedPlayer == 0 && GameSceneManager.Instance.currentSceneName == "LevelsScene")
        {
            Vector2 input = value.Get<Vector2>();
            int horizontalInput = input.x > 0 ? 1 : -1;
            int verticalInput = input.y > 0 ? 1 : -1;
            if ((Mathf.Approximately(input.x, 0) && Mathf.Approximately(input.y, 0)) || cooldown > 0)
            {
                return;
            }
            if (Mathf.Abs(input.x) > Mathf.Abs(input.y))
            {
                if (horizontalInput < 0)
                {
                    // left
                    LevelsController.Instance.NavigateLeft();
                }
                else
                {
                    // right
                    LevelsController.Instance.NavigateRight();
                }
            }
            else
            {
                if (verticalInput < 0)
                {
                    // down
                    LevelsController.Instance.NavigateDown();
                }
                else
                {
                    // up
                    LevelsController.Instance.NavigateUp();
                }
            }
            cooldown = 0.2f;
        }
    }
}
