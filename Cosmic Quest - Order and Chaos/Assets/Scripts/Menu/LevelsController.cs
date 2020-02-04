using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelsController : MonoBehaviour
{
    #region Singleton
    public static LevelsController Instance;

    protected virtual void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Debug.LogWarning("Only one levels controller should be in the scene!");
    }
    #endregion

    public GameObject cursor;
    public Camera mainCamera;
    private Vector3 initialCameraPos;
    private float selectionCooldown = 0.25f;
    private LevelPreview[] levelPreviews;
    private LevelPreview currentlySelected;

    // Start is called before the first frame update
    void Start()
    {
        initialCameraPos = mainCamera.transform.position;
        levelPreviews = GetComponentsInChildren<LevelPreview>();
        if (levelPreviews.Length > 0)
        {
            currentlySelected = levelPreviews[0];
            cursor.transform.position = new Vector3(currentlySelected.transform.position.x, cursor.transform.position.y, cursor.transform.position.z);
            mainCamera.transform.position = new Vector3(currentlySelected.transform.position.x, mainCamera.transform.position.y, mainCamera.transform.position.z);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (selectionCooldown > 0)
        {
            selectionCooldown -= Time.deltaTime;
            if (selectionCooldown < 0)
            {
                selectionCooldown = 0;
            }
        }
    }

    public void NavigateUp()
    {
        if (currentlySelected.selectOnUp != null)
        {
            Navigate(currentlySelected.selectOnUp);
        }
    }

    public void NavigateDown()
    {
        if (currentlySelected.selectOnDown != null)
        {
            Navigate(currentlySelected.selectOnDown);
        }
    }

    public void NavigateLeft()
    {
        if (currentlySelected.selectOnLeft != null)
        {
            Navigate(currentlySelected.selectOnLeft);
        }
    }

    public void NavigateRight()
    {
        if (currentlySelected.selectOnRight != null)
        {
            Navigate(currentlySelected.selectOnRight);
        }
    }

    public void SelectLevel()
    {
        StartCoroutine(GameSceneManager.Instance.LoadYourAsyncScene(currentlySelected.levelSceneName));
    }

    private void Navigate(LevelPreview levelPreview)
    {
        if (selectionCooldown > 0)
        {
            return;
        }
        currentlySelected = levelPreview;
        StartCoroutine(MoveCursor(0.25f));
        StartCoroutine(MoveCamera(0.25f));
        selectionCooldown = 0.25f;
    }

    private IEnumerator MoveCursor(float time)
    {
        //cursor.transform.position = currentlySelected.transform.position;
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

    private IEnumerator MoveCamera(float time)
    {
        float elapsed = 0;
        while (elapsed < time)
        {
            float newX = Mathf.Lerp(mainCamera.transform.position.x, initialCameraPos.x + currentlySelected.transform.position.x, elapsed / time);
            float newZ = Mathf.Lerp(mainCamera.transform.position.z, initialCameraPos.z + currentlySelected.transform.position.z, elapsed / time);
            mainCamera.transform.position = new Vector3(newX, mainCamera.transform.position.y, newZ);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }
}
