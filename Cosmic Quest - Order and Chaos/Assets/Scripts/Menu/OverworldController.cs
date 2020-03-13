using System.Linq;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class OverworldController : MonoBehaviour
{
    public enum LevelMenuState
    {
        Transitioning,
        Selecting,
        Selected
    }

    #region Singleton
    public static OverworldController Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            GameManager.Instance.SetSelectingLevelState();
        }
        else
            Debug.LogWarning("Only one overworld controller should be in the scene!");
    }
    #endregion

    [Tooltip("Cursor that indicates the currently selected level")]
    public GameObject cursor;
    [Tooltip("UI overlay of a selected level")]
    public GameObject previewScreen;
    [Header("Overworld Navigation")]
    [Tooltip("How much to zoom into a level by upon selecting it")]
    [Range(0, 40)]
    public float zoomLevel = 20;
    [Tooltip("How fast the camera zooms into a level upon selecting it")]
    [Range(0, 1)]
    public float zoomSpeed = 0.5f;
    [Tooltip("How fast the camera moves to a level upon navigating to it")]
    [Range(0, 1)]
    public float navigationSpeed = 0.25f;
    public LevelMenuState CurrentState;
    private Vector3 cameraOffset;
    private OverworldLevel[] levelPreviews;
    private OverworldLevel currentlySelected;

    private void Start()
    {
        levelPreviews = GetComponentsInChildren<OverworldLevel>();
        // store initial position of camera
        cameraOffset = Camera.main.transform.position;
        if (levelPreviews.Length > 0)
        {
            // set camera + cursor position to the highest started level
            currentlySelected = levelPreviews.LastOrDefault(e => e.chaosVoid.started);
            if (!currentlySelected)
            {
                currentlySelected = levelPreviews[0];
            }
            Vector3 start = Camera.main.transform.position;
            Vector3 stop = new Vector3(cameraOffset.x + currentlySelected.transform.position.x, Camera.main.transform.position.y, cameraOffset.z + currentlySelected.transform.position.z);
            StartCoroutine(MoveCamera(1, start, stop));
            StartCoroutine(LevelOverlayController.Instance.SetTitle(currentlySelected.chaosVoid.scene.name, 0));
        }
        SetSelectingState();
        
    }

    /// <summary>
    /// Description: Set the state of the overworld controller to transitioning [to a level]
    /// Rationale: Should be able to control state of the overworld controller
    /// </summary>
    public void SetTransitioningState()
    {
        CurrentState = LevelMenuState.Transitioning;
    }

    /// <summary>
    /// Description: Set the state of the overworld controller to selecting [a level]
    /// Rationale: Should be able to control state of the overworld controller
    /// </summary>
    public void SetSelectingState()
    {
        CurrentState = LevelMenuState.Selecting;
    }

    /// <summary>
    /// Description: Set the state of the overworld controller to [a level has been]
    /// Rationale: Should be able to control state of the overworld controller
    /// </summary>
    public void SetSelectedState()
    {
        CurrentState = LevelMenuState.Selected;
    }

    /// <summary>
    /// Description: Set the state of the overworld controller after a certain amount of time
    /// Rationale: Should be able to control state of the overworld controller
    /// </summary>
    /// <param name="delay">Time to wait before setting the state of the overworld controller</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator SetCurrentState(LevelMenuState levelMenuState, float delay)
    {
        if (delay > 0)
        {
            yield return new WaitForSeconds(delay);
        }
        CurrentState = levelMenuState;
    }

    /// <summary>
    /// Description: Load and play the scene associated with the currently selected level
    /// Rationale: Users should be able to play different levels
    /// </summary>
    public void PlayLevel()
    {
        if (CurrentState == LevelMenuState.Selected)
        {
            // zoom into the chaos void to simulate entering it
            Vector3 start = Camera.main.transform.position;
            Vector3 stop = currentlySelected.transform.position;
            StartCoroutine(MoveCamera(zoomSpeed, start, stop));
            float timer = zoomSpeed * 2;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            LevelManager.Instance.StartChaosVoid(currentlySelected.chaosVoid);
        }
    }

    /// <summary>
    /// Description: Navigates to the level listed as above the currently selected level.
    /// Rationale: Users should be able to navigate the overworld map
    /// </summary>
    public void NavigateUp()
    {
        if (currentlySelected.selectOnUp != null)
        {
            Navigate(currentlySelected.selectOnUp);
        }
    }

    /// <summary>
    /// Description: Navigates to the level above and to the left of the currently selected level.
    /// Rationale: Users should be able to navigate the overworld map
    /// </summary>
    public void NavigateUpLeft()
    {
        if (currentlySelected.selectOnUpLeft != null)
        {
            Navigate(currentlySelected.selectOnUpLeft);
        }
    }

    /// <summary>
    /// Description: Navigates to the level listed as above and to the right of the currently selected level.
    /// Rationale: Users should be able to navigate the overworld map
    /// </summary>
    public void NavigateUpRight()
    {
        if (currentlySelected.selectOnUpRight != null)
        {
            Navigate(currentlySelected.selectOnUpRight);
        }
    }

    /// <summary>
    /// Description: Navigates to the level below the currently selected level.
    /// Rationale: Users should be able to navigate the overworld map
    /// </summary>
    public void NavigateDown()
    {
        if (currentlySelected.selectOnDown != null)
        {
            Navigate(currentlySelected.selectOnDown);
        }
    }

    /// <summary>
    /// Description: Navigates to the level below and to the left of the currently selected level.
    /// Rationale: Users should be able to navigate the overworld map
    /// </summary>
    public void NavigateDownLeft()
    {
        if (currentlySelected.selectOnDownLeft != null)
        {
            Navigate(currentlySelected.selectOnDownLeft);
        }
    }

    /// <summary>
    /// Description: Navigates to the level below and to the right of the currently selected level.
    /// Rationale: Users should be able to navigate the overworld map
    /// </summary>
    public void NavigateDownRight()
    {
        if (currentlySelected.selectOnDownRight != null)
        {
            Navigate(currentlySelected.selectOnDownRight);
        }
    }

    /// <summary>
    /// Description: Navigates to the level to the left of the currently selected level.
    /// Rationale: Users should be able to navigate the overworld map
    /// </summary>
    public void NavigateLeft()
    {
        if (currentlySelected.selectOnLeft != null)
        {
            Navigate(currentlySelected.selectOnLeft);
        }
    }

    /// <summary>
    /// Description: Navigates to the level to the right of the currently selected level.
    /// Rationale: Users should be able to navigate the overworld map
    /// </summary>
    public void NavigateRight()
    {
        if (currentlySelected.selectOnRight != null)
        {
            Navigate(currentlySelected.selectOnRight);
        }
    }

    /// <summary>
    /// Description: Zoom in on a level and display the level overlay screen
    /// Rationale: Users should be able to confirm whether they want to play the level or not and view information about the level
    /// </summary>
    public void SelectLevel()
    {
        if (CurrentState == LevelMenuState.Selecting)
        {
            SetSelectedState();
            Vector3 start = Camera.main.transform.position;
            Vector3 stop = start + Camera.main.transform.forward * zoomLevel;
            StartCoroutine(ShowLevelOverlay(1 - zoomSpeed));
            StartCoroutine(MoveCamera(zoomSpeed, start, stop));
        }
    }

    /// <summary>
    /// Description: Zoom out on a selected level and hide the preview screen
    /// Rationale: Users should be able to back out of playing the level and go back to the map
    /// </summary>
    public void DeselectLevel()
    {
        if (CurrentState == LevelMenuState.Selected)
        {
            Vector3 start = Camera.main.transform.position;
            Vector3 stop = start - Camera.main.transform.forward * zoomLevel;
            StartCoroutine(HideLevelOverlay(0));
            StartCoroutine(MoveCamera(zoomSpeed, start, stop));
            StartCoroutine(SetCurrentState(LevelMenuState.Selecting, 1 - zoomSpeed));
        }
    }

    /// <summary>
    /// Description: Moves the main camera and level selection cursor to the selected level
    /// Rationale: Main camera and level selection cursor should move as the user selects a level to signify which level is selected
    /// </summary>
    /// <param name="overworldLevel">The selected level</param>
    private void Navigate(OverworldLevel overworldLevel)
    {
        if (CurrentState != LevelMenuState.Selecting || overworldLevel.chaosVoid.isLocked)
        {
            return;
        }
        currentlySelected = overworldLevel;
        SetTransitioningState();
        Vector3 start = Camera.main.transform.position;
        Vector3 stop = new Vector3(cameraOffset.x + currentlySelected.transform.position.x, Camera.main.transform.position.y, cameraOffset.z + currentlySelected.transform.position.z);
        StartCoroutine(MoveCursor(navigationSpeed));
        StartCoroutine(MoveCamera(navigationSpeed, start, stop));
        StartCoroutine(SetCurrentState(LevelMenuState.Selecting, 1 - navigationSpeed));
        StartCoroutine(LevelOverlayController.Instance.SetTitle(overworldLevel.chaosVoid.scene.name, 1 - navigationSpeed));
    }

    /// <summary>
    /// Description: Moves the level selection cursor position to the currently selected level position
    /// Rationale: Level selection cursor should move as the user selects a level to signify which level is selected
    /// </summary>
    /// <param name="speed">Speed at which to transition the camera from the start position to the stop position</param>
    private IEnumerator MoveCursor(float speed)
    {
        float time = 1 - speed;
        float elapsed = 0;
        if (time > 0)
        {
            while (elapsed < time)
            {
                float newX = Mathf.Lerp(cursor.transform.position.x, currentlySelected.transform.position.x, elapsed / time);
                float newZ = Mathf.Lerp(cursor.transform.position.z, currentlySelected.transform.position.z, elapsed / time);
                cursor.transform.position = new Vector3(newX, cursor.transform.position.y, newZ);
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            cursor.transform.position = new Vector3(currentlySelected.transform.position.x, cursor.transform.position.y, currentlySelected.transform.position.z);
        }
    }

    /// <summary>
    /// Description: Translates the main camera's position from a start position to a stop position
    /// Rationale: Moving the camera signifies an object being selected
    /// </summary>
    /// <param name="speed">Speed at which to transition the camera from the start position to the stop position</param>
    /// <param name="start">Position to start the camera zoom at</param>
    /// <param name="stop">Position to stop the camera zoom at</param>
    private IEnumerator MoveCamera(float speed, Vector3 start, Vector3 stop)
    {
        float time = 1 - speed;
        float elapsed = 0;
        if (time > 0)
        {
            while (!Mathf.Approximately(Vector3.Distance(Camera.main.transform.position, stop), 0))
            {
                Vector3 newPos = Vector3.Lerp(start, stop, elapsed / time);
                Camera.main.transform.position = newPos;
                elapsed += Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            Camera.main.transform.position = stop;
        }
    }

    /// <summary>
    /// Description: Displays the UI associated with the currently selected level after a certain amount of time
    /// Rationale: Should be able to control when the UI is shown
    /// </summary>
    /// <param name="delay">Time to wait before showing the UI</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator ShowLevelOverlay(float delay)
    {
        if (delay < 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(delay);
        LevelOverlayController.Instance.ShowOverlay();
    }

    /// <summary>
    /// Description: Hides the UI associated with the currently selected level after a certain amount of time
    /// Rationale: Should be able to control when the UI is hidden
    /// </summary>
    /// <param name="delay">Time to wait before hiding the UI</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator HideLevelOverlay(float delay)
    {
        if (delay < 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(delay);
        LevelOverlayController.Instance.HideOverlay();
    }
}
