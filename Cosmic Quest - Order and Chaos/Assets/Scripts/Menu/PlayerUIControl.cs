using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerUIControl : MonoBehaviour
{
    private int assignedPlayer = -1;
    private void Awake()
    {
        assignedPlayer = PlayerManager.Instance.AssignUIControlToPlayer(gameObject);
        if (assignedPlayer >= 0)
        {
            DontDestroyOnLoad(gameObject);
            SceneManager.activeSceneChanged += SceneSwitched;
            name = "Player " + (assignedPlayer + 1) + " UI Control";
            AssignMultiplayerUIControl();
        }
        else
        {
            Debug.LogError("UI Control not assigned, no available player");
        }
    }

    /// <summary>
    /// Description: Re-assigns the UI control when the scene switches
    /// Rationale: UI control should persist across menus
    /// </summary>
    /// <param name="current">current scene</param>
    /// <param name="next">next scene</param>
    public void SceneSwitched(Scene current, Scene next)
    {
        StartCoroutine(AssignUIControl(10, next));
    }

    /// <summary>
    /// Assigns the UI control to a scene after it is loaded
    /// </summary>
    /// <param name="timeout">Time to wait before stopping execution</param>
    /// <param name="scene">The scene being loaded</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator AssignUIControl(float timeout, Scene scene)
    {
        float timer = 0;
        while (timer < timeout)
        {
            if (scene.isLoaded)
            {
                // Input wasn't registering across scenes. Disabling and re-enabling the gameobject fixed it.
                gameObject.SetActive(false);
                gameObject.SetActive(true);

                AssignMultiplayerUIControl();
                break;
            }
            timer += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Assigns the UI Control based on the state of the game
    /// </summary>
    public void AssignMultiplayerUIControl()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Menu)
        {
            MainMenuController.Instance.AssignMultiplayerUIControl(gameObject, assignedPlayer);
        }
        else if (assignedPlayer == 0 && GameManager.Instance.CurrentState == GameManager.GameState.SelectingLevel)
        {
            LevelOverlayController.Instance.AssignMultiplayerUIControl(gameObject);
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
