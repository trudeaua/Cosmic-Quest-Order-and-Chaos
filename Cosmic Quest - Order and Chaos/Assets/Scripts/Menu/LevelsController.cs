using System.Linq;
using System.Collections;
using UnityEngine;

public class LevelsController : MonoBehaviour
{
    public enum LevelMenuState
    {
        Transitioning,
        Selecting,
        Selected
    }

    #region Singleton
    public static LevelsController Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one levels controller should be in the scene!");
    }
    #endregion

    public GameObject cursor;
    public Camera mainCamera;
    public GameObject previewScreen;
    private Vector3 cameraOffset;
    private Vector3 initialCameraPos;
    private float selectionCooldown = 0.25f;
    private LevelPreview[] levelPreviews;
    private LevelPreview currentlySelected;
    private LevelMenuState CurrentState;

    private void Start()
    {
        GameManager.Instance.SetSelectingLevelState();
        levelPreviews = GetComponentsInChildren<LevelPreview>();
        cameraOffset = mainCamera.transform.position;
        if (levelPreviews.Length > 0)
        {
            currentlySelected = levelPreviews.LastOrDefault(e => e.chaosVoid.started);
            if (!currentlySelected)
            {
                currentlySelected = levelPreviews[0];
            }
            cursor.transform.position = new Vector3(currentlySelected.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
            mainCamera.transform.position = new Vector3(currentlySelected.transform.position.x, mainCamera.transform.position.y, cameraOffset.z + mainCamera.transform.position.z);
            StartCoroutine(PreviewMenuController.Instance.SetTitle(currentlySelected.chaosVoid.scene.name, 0));
        }
        initialCameraPos = mainCamera.transform.position;
        SetSelectingState();
        // show paths to levels only if the level has been cleared
        foreach (LevelPreview levelPreview in levelPreviews)
        {
            if (levelPreview.pathToNextLevel != null) {
                levelPreview.pathToNextLevel.SetActive(levelPreview.chaosVoid.cleared);
            }
        }
    }

    private void Update()
    {
        if (selectionCooldown > 0)
        {
            selectionCooldown -= Time.deltaTime;
            if (selectionCooldown < 0)
            {
                selectionCooldown = 0;
                SetSelectingState();
            }
        }
    }

    /// <summary>
    /// Set the state of the level controller to transitioning [to a level]
    /// </summary>
    public void SetTransitioningState()
    {
        CurrentState = LevelMenuState.Transitioning;
    }

    /// <summary>
    /// Set the state of the level controller to selecting [a level]
    /// </summary>
    public void SetSelectingState()
    {
        CurrentState = LevelMenuState.Selecting;
    }

    /// <summary>
    /// Set the state of the level controller to [a level has been] selected
    /// </summary>
    public void SetSelectedState()
    {
        CurrentState = LevelMenuState.Selected;
    }

    /// <summary>
    /// Description: Navigates to the level listed as "above" the currently selected level.
    /// Rationale: Users should be able to easily navigate to a level above the currently selected one.
    /// </summary>
    public void NavigateUp()
    {
        if (currentlySelected.selectOnUp != null)
        {
            Navigate(currentlySelected.selectOnUp);
        }
    }

    /// <summary>
    /// Description: Navigates to the level listed as "above" the currently selected level.
    /// Rationale: Users should be able to easily navigate to a level above the currently selected one.
    /// </summary>
    public void NavigateDown()
    {
        if (currentlySelected.selectOnDown != null)
        {
            Navigate(currentlySelected.selectOnDown);
        }
    }

    /// <summary>
    /// Description: Navigates to the level listed as "below" the currently selected level.
    /// Rationale: Users should be able to easily navigate to a level below the currently selected one.
    /// </summary>
    public void NavigateLeft()
    {
        if (currentlySelected.selectOnLeft != null)
        {
            Navigate(currentlySelected.selectOnLeft);
        }
    }

    /// <summary>
    /// Description: Navigates to the level listed as "to the right of" the currently selected level.
    /// Rationale: Users should be able to easily navigate to a level to the right of the currently selected one.
    /// </summary>
    public void NavigateRight()
    {
        if (currentlySelected.selectOnRight != null)
        {
            Navigate(currentlySelected.selectOnRight);
        }
    }

    /// <summary>
    /// Description: Zoom in on a level and display the preview screen
    /// Rationale: Users should be able to confirm whether they want to play the level or not and view information about the level
    /// </summary>
    public void SelectLevel()
    {
        if (CurrentState == LevelMenuState.Selecting)
        {
            SetSelectedState();
            initialCameraPos = mainCamera.transform.position;

            Vector3 start = mainCamera.transform.position;
            Vector3 stop = start + mainCamera.transform.forward * 20;
            StartCoroutine(ZoomCamera(0.5f, start, stop));
            StartCoroutine(PreviewLevel(0.5f));
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
            SetSelectingState();

            Vector3 start = mainCamera.transform.position;
            Vector3 stop = initialCameraPos;
            StartCoroutine(UnpreviewLevel(0));
            StartCoroutine(ZoomCamera(0.5f, start, stop));
        }
    }

    /// <summary>
    /// Description: Load and play the level associated with the currently selected level
    /// Rationale: Users should be able to play different levels
    /// </summary>
    public void PlayLevel()
    {
        if (CurrentState == LevelMenuState.Selected)
        {
            Vector3 start = mainCamera.transform.position;
            Vector3 stop = currentlySelected.transform.position;
            float transitionTime = 0.5f;
            StartCoroutine(ZoomCamera(transitionTime, start, stop));
            float timer = transitionTime * 2;
            while (timer > 0)
            {
                timer -= Time.deltaTime;
            }
            LevelManager.Instance.StartChaosVoid(currentlySelected.chaosVoid);
        }
    }

    /// <summary>
    /// Description: Moves the main camera and level selection cursor to the selected level
    /// Rationale: Main camera and level selection cursor should move as the user selects a level to signify which level is selected
    /// </summary>
    /// <param name="levelPreview">The selected level</param>
    private void Navigate(LevelPreview levelPreview)
    {
        if (selectionCooldown > 0 || CurrentState == LevelMenuState.Selected || levelPreview.chaosVoid.isLocked)
        {
            return;
        }
        
        SetTransitioningState();
        currentlySelected = levelPreview;
        StartCoroutine(PreviewMenuController.Instance.SetTitle(levelPreview.chaosVoid.scene.name, 0.25f));
        StartCoroutine(MoveCursor(0.25f));
        StartCoroutine(MoveCamera(0.25f));
        selectionCooldown = 0.25f;
    }

    /// <summary>
    /// Description: Moves the level selection cursor position to the currently selected level position
    /// Rationale: Level selection cursor should move as the user selects a level to signify which level is selected
    /// </summary>
    /// <param name="time">Time in seconds in which to transition the cursor to the selected level position</param>
    private IEnumerator MoveCursor(float time)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            float newX = Mathf.Lerp(cursor.transform.position.x, currentlySelected.transform.position.x, elapsed / time);
            float newZ = Mathf.Lerp(cursor.transform.position.z, currentlySelected.transform.position.z, elapsed / time);
            cursor.transform.position = new Vector3(newX, cursor.transform.position.y, newZ);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
    /// <summary>
    /// Description: Moves the main camera position to put the currently selected level in the centre of the view
    /// Rationale: Main camera should move as the user selects a level to signify which level is selected
    /// </summary>
    /// <param name="time">Time in seconds in which to transition the main camera to the selected level position</param>
    private IEnumerator MoveCamera(float time)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            float newX = Mathf.Lerp(mainCamera.transform.position.x, cameraOffset.x + currentlySelected.transform.position.x, elapsed / time);
            float newZ = Mathf.Lerp(mainCamera.transform.position.z, cameraOffset.z + currentlySelected.transform.position.z, elapsed / time);
            mainCamera.transform.position = new Vector3(newX, mainCamera.transform.position.y, newZ);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Description: Translates the main camera's position from a start position to a stop position
    /// Rationale: Zooming in/out on a level signifies that it is selected/deselected
    /// </summary>
    /// <param name="time">Time in seconds in which to transition the camera from the start position to the stop position</param>
    /// <param name="start">Position to start the camera zoom at</param>
    /// <param name="stop">Position to stop the camera zoom at</param>
    private IEnumerator ZoomCamera(float time, Vector3 start, Vector3 stop)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            Vector3 newPos = Vector3.Lerp(start, stop, elapsed / time);
            mainCamera.transform.position = newPos;
            elapsed += Time.deltaTime;
            yield return null;
        }
    }

    /// <summary>
    /// Displays the UI associated with the currently selected level
    /// </summary>
    /// <param name="delay">Time to wait before showing the UI</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator PreviewLevel(float delay)
    {
        if (delay < 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(delay);
        PreviewMenuController.Instance.PreviewLevel();
    }

    /// <summary>
    /// Hides the UI associated with the currently selected level
    /// </summary>
    /// <param name="delay">Time to wait before hiding the UI</param>
    /// <returns>An IEnumerator</returns>
    private IEnumerator UnpreviewLevel(float delay)
    {
        if (delay < 0)
        {
            yield return null;
        }
        yield return new WaitForSeconds(delay);
        PreviewMenuController.Instance.UnpreviewLevel();
    }
}
